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
        SELECT PT.PRODUCTO, PT.DESCRIPCION, PT.GRUPO, PT.SUBGRUPO, PT.UNIDAD,
               PT.ALTA, PT.BAJA, PT.EMPAQUE, PT.COSTO_BASE, 
               PT.BinaryChecksum, PT.OldBinaryChecksum
        FROM sources.PRODUCTO_TARGET PT
        WHERE PT.OldBinaryChecksum != PT.BinaryChecksum 
           OR PT.OldBinaryChecksum = 0 
           OR PT.OldBinaryChecksum IS NULL";

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

      // Crear diccionario de combinaciones únicas Grupo-SubGrupo
      var gruposSubgruposDict = new Dictionary<string, (string Grupo, string SubGrupo)>();
      foreach (var item in toTransformData) {
        string key = $"{item.Grupo}|{item.SubGrupo}";
        if (!gruposSubgruposDict.ContainsKey(key)) {
          gruposSubgruposDict[key] = (item.Grupo, item.SubGrupo);
        }
      }

      var productCache = PreloadProductData(dataServices, productos);
      var unitCache = PreloadUnitData(dataServices, unidades);
      var categoryCache = PreloadCategoryData(dataServices, gruposSubgruposDict);

      return toTransformData.Select(x => Transform(x, dataServices, productCache, unitCache, categoryCache))
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
          // El producto no existe aún (nuevo), se creará en Transform
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

    private Dictionary<string, CategoryCacheData> PreloadCategoryData(
      TransformerDataServices dataServices,
      Dictionary<string, (string Grupo, string SubGrupo)> gruposSubgrupos) {

      var cache = new Dictionary<string, CategoryCacheData>();
      foreach (var kvp in gruposSubgrupos) {
        cache[kvp.Key] = new CategoryCacheData {
          CategoryId = (int) dataServices.GetCategoryIdFromCommonStorage(kvp.Value.Grupo, kvp.Value.SubGrupo),
          Tags = dataServices.GetObjectTagsFromCommonStorage(kvp.Value.Grupo, kvp.Value.SubGrupo)
        };
      }
      return cache;
    }

    private ProductData Transform(ProductNK source,
                                  TransformerDataServices dataServices,
                                  Dictionary<string, ProductCacheData> productCache,
                                  Dictionary<string, int> unitCache,
                                  Dictionary<string, CategoryCacheData> categoryCache) {

      var isNewProduct = source.OldBinaryChecksum == 0;
      var costoBase = source.CostoBase.ToString();
      var categoryKey = $"{source.Grupo}|{source.SubGrupo}";
      var categoryData = categoryCache[categoryKey];

      // Pre-serializar JSON que se usa múltiples veces
      var identificators = JsonConvert.SerializeObject(new {
        source.Grupo,
        source.SubGrupo,
        Object_Category_Id = source.Grupo,
        Object_Classification_Id = source.SubGrupo,
        CostoBase = costoBase
      });

      var attributes = JsonConvert.SerializeObject(new {
        packagingSize = source.Empaque.ToString()
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
        Product_Category_Id = categoryData.CategoryId,
        Product_Name = source.Producto,
        Product_Description = source.Descripcion,
        Product_Internal_Code = source.Producto,
        Product_Identificators = identificators,
        Product_Roles = string.Empty,
        Product_Tags = categoryData.Tags,
        Product_Attributes = attributes,
        Product_Base_Unit_Id = unitCache[source.UnidadMedida],
        Product_Manager_Id = 1,
        Product_Ext_Data = identificators,
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
        o.Product_Id, o.Product_UID, o.Product_Type_Id, o.Product_Category_Id, o.Product_Name,
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