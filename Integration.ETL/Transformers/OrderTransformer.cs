/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Services Layer                        *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Service provider                      *
*  Type     : OrderTransformer                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Transforms a Order(OV) from NK to Empiria Trade.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Data;
using Empiria.Json;
using Empiria.Trade.Integration.ETL.Data;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>Transforms a order(OV) from NK to Empiria Trade.</summary>
  public class OrderTransformer {

    private readonly string _connectionString;

    internal OrderTransformer(string connectionString) {
      Assertion.Require(connectionString, nameof(connectionString));

      _connectionString = connectionString;
    }

    public void Execute() {

      FixedList<OrderNK> sourceData = ReadSourceData();

      FixedList<OrderData> transformedData = Transform(sourceData);

      WriteTargetData(transformedData);
    }

    private FixedList<OrderNK> ReadSourceData() {
      var sql = "SELECT O.OV, O.Orden, O.Prioridad, O.Cliente, O.SubCliente, O.Almacen, O.Aplicado, O.Cancelado, " +
        "O.Vendedor, O.Moneda, O.Usr_Captura, O.Fecha, O.Estatus, O.BinaryChecksum, O.OldBinaryChecksum FROM sources.OV_TARGET O " +
        "WHERE O.FECHA >= '2025-01-01' and (O.OldBinaryChecksum != O.BinaryChecksum  " +
        "OR O.OldBinaryChecksum = 0    " +
        "OR O.OldBinaryChecksum IS NULL)";

      var connectionString = GetNKConnectionString();

      var inputDataService = new TransformerDataServices(connectionString);

      return inputDataService.ReadData<OrderNK>(sql);
    }


    private FixedList<OrderData> Transform(FixedList<OrderNK> toTransformData) {
      return toTransformData.Select(x => Transform(x))
                            .ToFixedList();
    }


    private OrderData Transform(OrderNK toTransformData) {
      string connectionString = GetEmpiriaConnectionString();
      var dataServices = new TransformerDataServices(connectionString);
      string connectionStringNK = GetNKConnectionString();
      var dataServicesNK = new TransformerDataServices(connectionStringNK);
      if (toTransformData.OldBinaryChecksum == 0) {
        return new OrderData {
          Order_Id = dataServices.GetNextId("OMS_Orders"),
          Order_UID = System.Guid.NewGuid().ToString(),
          Order_Type_Id = 4001,
          Order_Category_Id = -1,
          Order_No = toTransformData.OV,
          Order_Description = Empiria.EmpiriaString.BuildKeywords(toTransformData.Orden , dataServices.ReturnOldDescriptionForPriority(toTransformData.Prioridad)),
          Order_Identificators = toTransformData.OV,
          Order_Tags = Empiria.EmpiriaString.BuildKeywords(toTransformData.Cliente, toTransformData.SubCliente, toTransformData.Almacen),
          Order_Requested_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Cliente),
          Order_Responsible_Id = dataServices.GetPartyIdFromParties(toTransformData.Vendedor),
          Order_Beneficary_Id = dataServices.GetPartyIdFromParties(toTransformData.Cliente),
          Order_Provider_Id  =  dataServices.GetWareHouseIdFromCommonStorage(toTransformData.Almacen),
          Order_Budget_Id = -1,
          Order_Requisition_Id = -1,
          Order_Contract_Id = -1,
          Order_Project_Id = -1,
          Order_Currency_Id = 600,
          Order_Source_Id = -1,
          Order_Priority = dataServices.ReturnIdForPriority(toTransformData.Prioridad),//////////
          Order_Authorization_Time = ExecutionServer.DateMinValue,
          Order_Authorized_By_Id = -1, 
          Order_Closing_Time = dataServicesNK.GetClosedDateFromOvUbicacionConsecutivo(toTransformData.OV),
          Order_Closed_By_Id = dataServices.GetPartyIdFromParties(dataServicesNK.GetClosedIdFromOvUbicacionConsecutivo(toTransformData.OV).ToString()),
          Order_Ext_Data = "",
          Order_Keywords = Empiria.EmpiriaString.BuildKeywords(toTransformData.OV, toTransformData.Cliente, toTransformData.Almacen, toTransformData.Moneda),
          Order_Posted_By_Id  = dataServices.GetPartyIdFromParties(toTransformData.Usr_Captura),
          Order_Posting_Time  = toTransformData.Fecha,
          Order_Status = Convert.ToChar(toTransformData.Estatus)
        };
      } else {
        return new OrderData {
          Order_Id = dataServices.GetOrderIdFromOMSOrders(toTransformData.OV),
          Order_UID = dataServices.GetOrderUIDFromOMSOrders(toTransformData.OV),
          Order_Type_Id = 4001,
          Order_Category_Id = -1,
          Order_No = toTransformData.OV,
          Order_Description = Empiria.EmpiriaString.BuildKeywords(toTransformData.Orden, dataServices.ReturnOldDescriptionForPriority(toTransformData.Prioridad)),
          Order_Identificators = toTransformData.OV,
          Order_Tags = Empiria.EmpiriaString.BuildKeywords(toTransformData.Cliente, toTransformData.SubCliente, toTransformData.Almacen),
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
          Order_Priority = dataServices.ReturnIdForPriority(toTransformData.Prioridad),
          Order_Authorization_Time = ExecutionServer.DateMinValue,
          Order_Authorized_By_Id = -1, 
          Order_Closing_Time = dataServicesNK.GetClosedDateFromOvUbicacionConsecutivo(toTransformData.OV),
          Order_Closed_By_Id = dataServices.GetPartyIdFromParties(dataServicesNK.GetClosedIdFromOvUbicacionConsecutivo(toTransformData.OV).ToString()),
          Order_Ext_Data = "",
          Order_Keywords = Empiria.EmpiriaString.BuildKeywords(toTransformData.OV, toTransformData.Cliente, toTransformData.Almacen, toTransformData.Moneda),
          Order_Posted_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Usr_Captura),
          Order_Posting_Time = toTransformData.Fecha,
          Order_Status = Convert.ToChar(toTransformData.Estatus)
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

  }  // class OrderTransformer

} // namespace Empiria.Trade.Integration.ETL.Transformers

