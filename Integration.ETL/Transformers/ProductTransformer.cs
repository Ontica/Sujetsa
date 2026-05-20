/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Services Layer                        *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Service provider                      *
*  Type     : ProductTransformer                           License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Transforms a product from NK to Empiria Trade.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.Linq;
using Empiria.Data;
using Empiria.Json;
using Empiria.Trade.Integration.ETL.Data;
using Newtonsoft.Json;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>Transforms a product from NK to Empiria Trade.</summary>
  public class ProductTransformer {

    private readonly string _connectionString;
    private readonly string _empiriaConnectionString;
    private readonly string _nkConnectionString;

    internal ProductTransformer(string connectionString) {
      Assertion.Require(connectionString, nameof(connectionString));
      _connectionString = connectionString;
      _empiriaConnectionString = GetEmpiriaConnectionString();
      _nkConnectionString = GetNKConnectionString();
    }

    public void Execute() {
      var sourceData = ReadSourceData();
      var transformedData = Transform(sourceData);
      WriteTargetData(transformedData);

      var outputDataServices = new SqlServerDataServices(_nkConnectionString);
      outputDataServices.ExecuteUpdateProductStatusProcedure();
    }

    private FixedList<ProductNK> ReadSourceData() {
      const string sql = @"
      SELECT PT.PRODUCTO, PT.DESCRIPCION, 
      ISNULL(NULLIF(LTRIM(RTRIM(PT.GRUPO)), ''), '-1') AS GRUPO,
      ISNULL(NULLIF(LTRIM(RTRIM(PT.SUBGRUPO)), ''), '-1') AS SUBGRUPO,
      PT.UNIDAD,  PT.ALTA,     PT.BAJA,     PT.EMPAQUE,     PT.COSTO_BASE,     PT.LINEA,    L.NOMBRE AS LINEA_NOMBRE,
      PT.MARCA,   M.NOMBRE AS MARCA_NOMBRE, PT.CATEGORIA, PT.SUBCATEGORIA, 
      PT.COLOR, C.DESCRIPCION AS COLOR_NOMBRE,PT.TALLA, 
      T.DESCRIPCION AS TALLA_NOMBRE, CAST(PT.PESO AS VARCHAR(20)) AS PESO, PT.MODELO,
      MO.DESCRIPCION AS MODELO_NOMBRE, PT.SECCION, 
      S.SECCION AS SECCION_NOMBRE,
      PT.USR1         AS DIAMETRO,
      PT.USR2         AS LARGO,
      PT.BinaryChecksum,   PT.OldBinaryChecksum
      FROM [sources].[PRODUCTO_TARGET] PT
      LEFT JOIN [sources].[LINEA_TARGET] L
      ON PT.LINEA = L.LINEA
      LEFT JOIN [sources].[MARCA_TARGET] M
      ON PT.MARCA = M.MARCA
      LEFT JOIN [sources].[COLOR_TARGET] C
      ON PT.COLOR = C.COLOR
      LEFT JOIN [sources].[TALLA_TARGET] T
      ON PT.TALLA = T.TALLA
      LEFT JOIN [sources].[MODELO_TARGET] MO
      ON  PT.MODELO = MO.MODELO 
      LEFT JOIN [sources].[SECCION_TARGET] S
      ON  PT.SECCION = S.SECCION 
      WHERE PT.OldBinaryChecksum != PT.BinaryChecksum 
      OR PT.OldBinaryChecksum = 0 
      OR PT.OldBinaryChecksum IS NULL;";

      var inputDataService = new TransformerDataServices(_nkConnectionString);
      return inputDataService.ReadData<ProductNK>(sql);
    }

    private FixedList<ProductData> Transform(FixedList<ProductNK> toTransformData) {
      if (toTransformData.Count == 0) {
        return new FixedList<ProductData>();
      }

      var dataServices = new TransformerDataServices(_empiriaConnectionString);

      // Pre-cargar datos en lotes
      var productos = toTransformData.Select(x => x.Producto).Distinct().ToList();
      var unidades = toTransformData.Select(x => x.UnidadMedida).Distinct().ToList();


      var productCache = PreloadProductData(dataServices, productos);
      var unitCache = PreloadUnitData(dataServices, unidades);
      
      return toTransformData.Select(x => Transform(x, dataServices, productCache, unitCache))
                            .ToFixedList();
    }

    private Dictionary<string, ProductCacheData> PreloadProductData(TransformerDataServices dataServices, List<string> productos) {
      var cache = new Dictionary<string, ProductCacheData>();
      foreach (var producto in productos) {
        try {
          cache[producto] = new ProductCacheData {
            ProductId = dataServices.GetProductIdFromOMSProducts(producto),
            ProductUID = dataServices.GetProductUIDFromOMSProducts(producto)
          };
        } catch {
          // El producto no existe aún (nuevo), se creará 
          cache[producto] = null;
        }
      }
      return cache;
    }

    private Dictionary<string, int> PreloadUnitData(TransformerDataServices dataServices, List<string> unidades) {
      var cache = new Dictionary<string, int>();
      foreach (var unidad in unidades) {
        cache[unidad] = (int) dataServices.ReturnIdForProductBaseUnitId(unidad);
      }
      return cache;
    }
    
    private ProductData Transform(ProductNK source,
                                  TransformerDataServices dataServices,
                                  Dictionary<string, ProductCacheData> productCache,
                                  Dictionary<string, int> unitCache
                                  ) {

      var isNewProduct = source.OldBinaryChecksum == 0;
      var costoBase = source.CostoBase.ToString();


      // Pre-serializar JSON que se usa múltiples veces
      var ext_data = JsonConvert.SerializeObject(new {
        source.Grupo,
        source.SubGrupo,
        CostoBase = costoBase,
        packagingSize = source.Empaque.ToString()
      });
      
      
      var attributes = JsonConvert.SerializeObject(new {
        Linea = source.Linea,
        Linea_Nombre = source.LineaNombre,
        Marca = source.Marca,
        Marca_Nombre = source.MarcaNombre,
        Categoria = source.Categoria,
        Color = source.Color,
        Color_Nombre = source.ColorNombre,
        Talla = source.Talla,
        Talla_Nombre = source.TallaNombre,
        Diametro = source.Usr1,
        Largo = source.Usr2,
        Peso = source.Peso,
        Modelo = source.Modelo,
        Modelo_Nombre = source.ModeloNombre,
        Seccion = source.Seccion,
        Seccion_Nombre = source.SeccionNombre
      });

      var keywords = EmpiriaString.BuildKeywords(
        source.Producto,
        source.Descripcion,
        source.Grupo,
        source.SubGrupo,
        source.UnidadMedida
      );

      // Obtener datos del producto existente (si aplica)
      var existingProduct = productCache[source.Producto];

      return new ProductData {
        Product_Id = isNewProduct
          ? IdGenerator.GetNextId("OMS_Products")
          : existingProduct.ProductId,
        Product_UID = isNewProduct
          ? Guid.NewGuid().ToString()
          : existingProduct.ProductUID,
        Product_Type_Id = 536,
        Product_Category_Id = -1,
        Base_Product_Id = -1,
        Vendor_Id = - 100,
        Product_Name = source.Producto,
        Product_Description = source.Descripcion,
        Product_Internal_Code = source.Producto,
        Product_Identificators = string.Empty, 
        Product_Roles = string.Empty,
        Product_Tags = (source.Producto?.Contains('-') == true
        ? source.Producto.Substring(0, source.Producto.IndexOf('-'))
        : source.Producto ?? string.Empty).Trim().ToUpper(),
        Product_Attributes = attributes,
        Product_Base_Unit_Id = unitCache[source.UnidadMedida],
        Product_Manager_Id = 1,
        Product_Ext_Data = ext_data,
        Product_Keywords = keywords,
        Product_Start_Date = source.FechaAlta,
        Product_End_Date = ExecutionServer.DateMaxValue,
        Product_Historic_Id = 0,
        Product_Posted_By_Id = 1,
        Product_Posting_Time = isNewProduct
          ? source.FechaAlta
          : ExecutionServer.DateMaxValue,
        Product_Status = 'A'
      };
    }

    private void WriteTargetData(FixedList<ProductData> transformedData) {
      if (transformedData.Count == 0)
        return;

      foreach (var item in transformedData) {
        WriteTargetData(item);
      }
    }

    private void WriteTargetData(ProductData o) {
      var op = DataOperation.Parse("write_OMS_Product",
        o.Product_Id, o.Product_UID, o.Product_Type_Id, o.Product_Category_Id,
        o.Base_Product_Id, o.Vendor_Id , o.Product_Name,
        o.Product_Description, o.Product_Internal_Code, o.Product_Identificators, o.Product_Roles,
        o.Product_Tags, o.Product_Attributes, o.Product_Base_Unit_Id, o.Product_Manager_Id,
        o.Product_Ext_Data, o.Product_Keywords, o.Product_Start_Date, o.Product_End_Date,
        o.Product_Historic_Id, o.Product_Posted_By_Id, o.Product_Posting_Time, o.Product_Status);

      DataWriter.Execute(op);
    }

    #region Helpers

    private static string GetEmpiriaConnectionString() {
      var config = ConfigurationData.Get<JsonObject>("Connection.Strings");
      return config.Get<string>("empiriaSqlServerConnection");
    }

    private static string GetNKConnectionString() {
      var config = ConfigurationData.Get<JsonObject>("Connection.Strings");
      return config.Get<string>("sqlServerConnection");
    }

    #endregion

    #region Cache Classes

    private class ProductCacheData {
      public int ProductId {
        get; set;
      }
      public string ProductUID {
        get; set;
      }
    }

    private class CategoryCacheData {
      public int CategoryId {
        get; set;
      }
      public string Tags {
        get; set;
      }
    }

    #endregion
  }
}