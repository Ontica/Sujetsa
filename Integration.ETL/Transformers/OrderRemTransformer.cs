/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Services Layer                        *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Service provider                      *
*  Type     : OrderRemTransformer                     License   : Please read LICENSE.txt file               *
*                                                                                                            *
*  Summary  : Transforms a Order(Remision) from NK to Empiria Trade.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;
using Empiria.Json;
using Empiria.Trade.Integration.ETL.Data;
using Newtonsoft.Json;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>Transforms a order(Remision) from NK to Empiria Trade.</summary>
  public class OrderRemTransformer {

    private readonly string _connectionString;

    internal OrderRemTransformer(string connectionString) {
      Assertion.Require(connectionString, nameof(connectionString));

      _connectionString = connectionString;
    }

    public void Execute() {

      FixedList<OrderRemNK> sourceData = ReadSourceData();

      FixedList<OrderData> transformedData = Transform(sourceData);

      WriteTargetData(transformedData);
    }


    private FixedList<OrderRemNK> ReadSourceData() {
      var sql = "SELECT O.* FROM sources.REML_TARGET O " +
        "Where O.Fecha >= '2025-01-01' And(O.Oldbinarychecksum != O.Binarychecksum " +
        "Or O.Oldbinarychecksum = 0 " +
        "Or O.Oldbinarychecksum IS NULL)";

      var connectionString = GetNKConnectionString();

      var inputDataService = new TransformerDataServices(connectionString);

      return inputDataService.ReadData<OrderRemNK>(sql);
    }


    private FixedList<OrderData> Transform(FixedList<OrderRemNK> toTransformData) {
      return toTransformData.Select(x => Transform(x))
                            .ToFixedList();
    }


    private OrderData Transform(OrderRemNK toTransformData) {
      string connectionString = GetEmpiriaConnectionString();
      var dataServices = new TransformerDataServices(connectionString);
      string connectionStringNK = GetNKConnectionString();
      var dataServicesNK = new TransformerDataServices(connectionStringNK);
      if (toTransformData.OldBinaryChecksum == 0) {
        return new OrderData {
          Order_Id = dataServices.GetNextId("OMS_Orders"),
          Order_UID = System.Guid.NewGuid().ToString(),
          Order_Type_Id = 4011,
          Order_Category_Id = -1,
          Order_No = toTransformData.Reml,
          Order_Description = Empiria.EmpiriaString.BuildKeywords(toTransformData.Reml, toTransformData.Almacen, toTransformData.Cliente, toTransformData.Icmov),
          Order_Identificators = toTransformData.Reml,
          Order_Tags = Empiria.EmpiriaString.BuildKeywords(toTransformData.Reml, toTransformData.Usuario, toTransformData.Cliente, toTransformData.Almacen, toTransformData.Icmov),
          Order_Requested_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Cliente),
          Order_Responsible_Id = dataServices.GetPartyIdFromParties(toTransformData.Vendedor),
          Order_Beneficary_Id = dataServices.GetPartyIdFromParties(toTransformData.Cliente),
          Order_Provider_Id = dataServices.GetWareHouseIdFromCommonStorage(toTransformData.Almacen),
          Order_Budget_Id = -1,
          Order_Requisition_Id = -1,
          Order_Contract_Id = -1,
          Order_Project_Id = -1,
          Order_Currency_Id = 600,
          Order_Source_Id = -1,
          Order_Priority = 'N',
          Order_Authorization_Time = toTransformData.Fecha_Aplicado,
          Order_Authorized_By_Id = -1,
          Order_Closing_Time = toTransformData.Fecha_Aplicado,
          Order_Closed_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Usuario),
          Order_Ext_Data = JsonConvert.SerializeObject(new { Name = "Remision" }),
          Order_Keywords = Empiria.EmpiriaString.BuildKeywords(toTransformData.Reml, toTransformData.Almacen, toTransformData.Cliente, toTransformData.Icmov),
          Order_Posted_By_Id  = dataServices.GetPartyIdFromParties(toTransformData.Usuario),
          Order_Posting_Time  = toTransformData.Fecha,
          Order_Status = dataServices.ReturnStatusForOrdersStatus(toTransformData.Cancelado)
        };
      } else {
        return new OrderData {
          Order_Id = dataServices.GetOrderIdFromOMSOrders(toTransformData.Reml),
          Order_UID = dataServices.GetOrderUIDFromOMSOrders(toTransformData.Reml),
          Order_Type_Id = 4011,
          Order_Category_Id = -1,
          Order_No = toTransformData.Reml,
          Order_Description = Empiria.EmpiriaString.BuildKeywords(toTransformData.Reml, toTransformData.Almacen, toTransformData.Cliente, toTransformData.Icmov),
          Order_Identificators = toTransformData.Reml,
          Order_Tags = Empiria.EmpiriaString.BuildKeywords(toTransformData.Reml, toTransformData.Usuario, toTransformData.Cliente, toTransformData.Almacen, toTransformData.Icmov),
          Order_Requested_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Cliente),
          Order_Responsible_Id = dataServices.GetPartyIdFromParties(toTransformData.Vendedor),
          Order_Beneficary_Id = dataServices.GetPartyIdFromParties(toTransformData.Cliente),
          Order_Provider_Id = dataServices.GetWareHouseIdFromCommonStorage(toTransformData.Almacen),
          Order_Budget_Id = -1,
          Order_Requisition_Id = -1,
          Order_Contract_Id = -1,
          Order_Project_Id = -1,
          Order_Currency_Id = 600,
          Order_Source_Id = -1,
          Order_Priority = ' ',
          Order_Authorization_Time = toTransformData.Fecha_Aplicado,
          Order_Authorized_By_Id = -1,
          Order_Closing_Time = toTransformData.Fecha_Aplicado,
          Order_Closed_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Usuario),
          Order_Ext_Data = JsonConvert.SerializeObject(new { Name = "Remision" }),
          Order_Keywords = Empiria.EmpiriaString.BuildKeywords(toTransformData.Reml, toTransformData.Almacen, toTransformData.Cliente, toTransformData.Icmov),
          Order_Posted_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Usuario),
          Order_Posting_Time = toTransformData.Fecha,
          Order_Status = dataServices.ReturnStatusForOrdersStatus(toTransformData.Cancelado)
        };
      }
    }
    

    private void WriteTargetData(FixedList<OrderData> transformedData) {
      foreach (var item in transformedData) {
        WriteTargetData(item);
      }
    }
      
    
    private void WriteTargetData(OrderData o) {
        var op = DataOperation.Parse("write_OMS_Order", o.Order_Id, o.Order_UID, o.Order_Type_Id, o.Order_Category_Id, o.Order_No
       ,o.Order_Description, o.Order_Identificators, o.Order_Tags, o.Order_Requested_By_Id, o.Order_Responsible_Id, o.Order_Beneficary_Id
       ,o.Order_Provider_Id, o.Order_Budget_Id, o.Order_Requisition_Id, o.Order_Contract_Id, o.Order_Project_Id, o.Order_Currency_Id
       ,o.Order_Source_Id, o.Order_Priority, o.Order_Authorization_Time, o.Order_Authorized_By_Id, o.Order_Closing_Time, o.Order_Closed_By_Id
       ,o.Order_Ext_Data, o.Order_Keywords, o.Order_Posted_By_Id, o.Order_Posting_Time, o.Order_Status);

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

  }  // class OrderRemTransformer

} // namespace Empiria.Trade.Integration.ETL.Transformers

