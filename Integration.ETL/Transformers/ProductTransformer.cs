//29 OCT 2025 ANTES DE MEDIO DIA
/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Services Layer                        *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Service provider                      *
*  Type     : ProductTransformer                           License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Transforms a product from NK to Empiria Trade.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;
using Empiria.Json;
using Empiria.Trade.Integration.ETL.Data;
using Newtonsoft.Json;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>Transforms a product from NK to Empiria Trade.</summary>
  public class ProductTransformer {

    private readonly string _connectionString;

    internal ProductTransformer(string connectionString) {
      Assertion.Require(connectionString, nameof(connectionString));

      _connectionString = connectionString;
    }

    public void Execute() {

      FixedList<ProductNK> sourceData = ReadSourceData();

      FixedList<ProductData> transformedData = Transform(sourceData);

      WriteTargetData(transformedData);

      var connectionString = GetNKConnectionString();

      var outputDataServices = new SqlServerDataServices(connectionString);

      outputDataServices.ExecuteUpdateProductStatusProcedure();

    }


    private FixedList<ProductNK> ReadSourceData() {
      var sql = "SELECT PT.PRODUCTO,PT.DESCRIPCION,PT.GRUPO,PT.SUBGRUPO,PT.UNIDAD,PT.ALTA," +
                "PT.BAJA,PT.EMPAQUE,PT.COSTO_BASE,PT.BinaryChecksum,PT.OldBinaryChecksum " +
                "FROM sources.PRODUCTO_TARGET PT " +
                "WHERE PT.OldBinaryChecksum != PT.BinaryChecksum " +
                "OR PT.OldBinaryChecksum = 0 " +
                "OR PT.OldBinaryChecksum IS NULL ";

      var connectionString = GetNKConnectionString();

      var inputDataService = new TransformerDataServices(connectionString);

      return inputDataService.ReadData<ProductNK>(sql);
    }


    private FixedList<ProductData> Transform(FixedList<ProductNK> toTransformData) {
      return toTransformData.Select(x => Transform(x))
                            .ToFixedList();
    }


    private ProductData Transform(ProductNK toTransformData) {
      string connectionString = GetEmpiriaConnectionString();
      string CostoBase = toTransformData.CostoBase.ToString();
      var dataServices = new TransformerDataServices(connectionString);
      if (toTransformData.OldBinaryChecksum == 0) {
        return new ProductData {
          Product_Id = dataServices.GetNextId("OMS_Products"),
          Product_UID = System.Guid.NewGuid().ToString(),
          Product_Type_Id = 536,
          Product_Category_Id = (int) dataServices.GetCategoryIdFromCommonStorage(toTransformData.Grupo, toTransformData.SubGrupo),
          Product_Name = toTransformData.Producto,
          Product_Description = toTransformData.Descripcion,
          Product_Internal_Code = toTransformData.Producto,
          Product_Identificators = JsonConvert.SerializeObject(new {
            toTransformData.Grupo,
            toTransformData.SubGrupo,
            Object_Category_Id = toTransformData.Grupo,
            Object_Classification_Id = toTransformData.SubGrupo,
            CostoBase
          }),
          Product_Roles = "",
          Product_Tags = dataServices.GetObjectTagsFromCommonStorage(toTransformData.Grupo, toTransformData.SubGrupo),
          Product_Attributes = JsonConvert.SerializeObject(new { packagingSize = (toTransformData.Empaque).ToString() }),
          Product_Base_Unit_Id = (int) dataServices.ReturnIdForProductBaseUnitId(toTransformData.UnidadMedida),
          Product_Manager_Id = 1,
          Product_Ext_Data = JsonConvert.SerializeObject(new {
            toTransformData.Grupo,
            toTransformData.SubGrupo,
            Object_Category_Id = toTransformData.Grupo,
            Object_Classification_Id = toTransformData.SubGrupo,
            CostoBase
          }),
          Product_Keywords = Empiria.EmpiriaString.BuildKeywords(toTransformData.Producto, toTransformData.Descripcion, toTransformData.Grupo, toTransformData.SubGrupo, toTransformData.UnidadMedida),
          Product_Start_Date = toTransformData.FechaAlta,
          Product_End_Date = ExecutionServer.DateMaxValue,
          Product_Historic_Id = 0,
          Product_Posted_By_Id = 1,
          Product_Posting_Time = toTransformData.FechaAlta,
          Product_Status = (char) 'A'
        };
      } else {
        return new ProductData {
          Product_Id = dataServices.GetProductIdFromOMSProducts(toTransformData.Producto),
          Product_UID = dataServices.GetProductUIDFromOMSProducts(toTransformData.Producto),
          Product_Type_Id = 536,
          Product_Category_Id = (int) dataServices.GetCategoryIdFromCommonStorage(toTransformData.Grupo, toTransformData.SubGrupo),
          Product_Name = toTransformData.Producto,
          Product_Description = toTransformData.Descripcion,
          Product_Internal_Code = toTransformData.Producto,
          Product_Identificators = JsonConvert.SerializeObject(new {
            toTransformData.Grupo,
            toTransformData.SubGrupo,
            Object_Category_Id = toTransformData.Grupo,
            Object_Classification_Id = toTransformData.SubGrupo,
            CostoBase
          }),
          Product_Roles = "",
          Product_Tags = dataServices.GetObjectTagsFromCommonStorage(toTransformData.Grupo, toTransformData.SubGrupo),
          Product_Attributes = JsonConvert.SerializeObject(new { packagingSize = (toTransformData.Empaque).ToString() }),
          Product_Base_Unit_Id = (int) dataServices.ReturnIdForProductBaseUnitId(toTransformData.UnidadMedida),
          Product_Manager_Id = 1,
          Product_Ext_Data = JsonConvert.SerializeObject(new {
            toTransformData.Grupo,
            toTransformData.SubGrupo,
            Object_Category_Id = toTransformData.Grupo,
            Object_Classification_Id = toTransformData.SubGrupo,
            CostoBase
          }),
          Product_Keywords = Empiria.EmpiriaString.BuildKeywords(toTransformData.Producto, toTransformData.Descripcion, toTransformData.Grupo, toTransformData.SubGrupo, toTransformData.UnidadMedida),
          Product_Start_Date = toTransformData.FechaAlta,
          Product_End_Date = ExecutionServer.DateMaxValue,
          Product_Historic_Id = 0,
          Product_Posted_By_Id = 1,
          Product_Posting_Time = ExecutionServer.DateMaxValue,
          Product_Status = (char) 'A'
        };
      }
    }


    private void WriteTargetData(FixedList<ProductData> transformedData) {
      foreach (var item in transformedData) {
        WriteTargetData(item);
      }
    }


    private void WriteTargetData(ProductData o) {
      var op = DataOperation.Parse("write_OMS_Product", o.Product_Id, o.Product_UID, o.Product_Type_Id, o.Product_Category_Id, o.Product_Name,
      o.Product_Description, o.Product_Internal_Code, o.Product_Identificators, o.Product_Roles, o.Product_Tags, o.Product_Attributes,
      o.Product_Base_Unit_Id, o.Product_Manager_Id, o.Product_Ext_Data, o.Product_Keywords, o.Product_Start_Date, o.Product_End_Date,
      o.Product_Historic_Id, o.Product_Posted_By_Id, o.Product_Posting_Time, o.Product_Status);

      DataWriter.Execute(op);
    }


    #region Helpers

    static private string GetEmpiriaConnectionString() {
      var config = ConfigurationData.Get<JsonObject>("Connection.Strings");

      return config.Get<string>("empiriaSqlServerConnection");
    }

    static private string GetNKConnectionString() {
      var config = ConfigurationData.Get<JsonObject>("Connection.Strings");

      return config.Get<string>("sqlServerConnection");
    }

    #endregion Helpers

  }  // class ProductTransformer

} // namespace Empiria.Trade.Integration.ETL.Transformers
