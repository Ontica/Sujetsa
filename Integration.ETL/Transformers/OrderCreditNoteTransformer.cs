﻿/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Services Layer                        *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Service provider                      *
*  Type     : OrderReturnTransformer                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Transforms a Order(NOTACREDITO) from NK to Empiria Trade.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;
using Empiria.Json;
using Empiria.Trade.Integration.ETL.Data;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>Transforms a order(NOTACREDITO) from NK to Empiria Trade.</summary>
  public class OrderCreditNoteTransformer {

    private readonly string _connectionString;

    internal OrderCreditNoteTransformer(string connectionString) {
      Assertion.Require(connectionString, nameof(connectionString));

      _connectionString = connectionString;
    }

    public void Execute() {

      FixedList<OrderCreditNoteNK> sourceData = ReadSourceData();

      FixedList<OrderData> transformedData = Transform(sourceData);

      WriteTargetData(transformedData);
    }


    private FixedList<OrderCreditNoteNK> ReadSourceData() {
      var sql = "SELECT O.NOTACREDITO,O.TIPO, O.TIPOPAGO, O.TIPO_NC, O.FECHA, O.CLIENTE,  O.USUARIO, O.OBSERVACIONES," +
        " O.CANCELADA,O.FECHACAPTURA, O.TIPOVENTA,O.FACTURA, O.CONCEPTODESC, O.POLIZA, O.ICMOV, O.CBPAGO,  O.SINC_S, " +
        " O.SUBCLIENTE, O.MOTIVODEVOLUCION, O.ALMACEN,O.DEVOLUCION, O.USUARIOCANCELO, O.FECHA_CANCELO,O.MD, O.MONEDA," +
        " O.SERIE, O.CONTADOR, O.CBTIPO, O.MOTIVODESCUENTO, O.TRANSACCION, O.VENDEDOR, O.TELEMARKETER, O.BinaryChecksum," +
        " O.OldBinaryChecksum FROM sources.NOTACREDITO_TARGET O" +
        " Where O.Fecha >= '2025-01-01' AND O.TIPO = 'D' And(O.Oldbinarychecksum != O.Binarychecksum " +
        " Or O.Oldbinarychecksum = 0 Or O.Oldbinarychecksum IS NULL) ";

      var connectionString = GetNKConnectionString();

      var inputDataService = new TransformerDataServices(connectionString);

      return inputDataService.ReadData<OrderCreditNoteNK>(sql);
    }


    private FixedList<OrderData> Transform(FixedList<OrderCreditNoteNK> toTransformData) {
      return toTransformData.Select(x => Transform(x))
                            .ToFixedList();
    }


    private OrderData Transform(OrderCreditNoteNK toTransformData) {
      string connectionString = GetEmpiriaConnectionString();
      var dataServices = new TransformerDataServices(connectionString);
      string connectionStringNK = GetNKConnectionString();
      var dataServicesNK = new TransformerDataServices(connectionStringNK);
      if (toTransformData.OldBinaryChecksum == 0) {
        return new OrderData {
          Order_Id = dataServices.GetNextId("OMS_Orders"),
          Order_UID = System.Guid.NewGuid().ToString(),
          Order_Type_Id = 4009,
          Order_Category_Id = -1,
          Order_No = toTransformData.NotaCredito,
          Order_Description = toTransformData.Observaciones,
          Order_Identificators = toTransformData.Factura,
          Order_Tags = Empiria.EmpiriaString.BuildKeywords(toTransformData.NotaCredito, toTransformData.Factura, toTransformData.Devolucion, toTransformData.Factura, toTransformData.Almacen, toTransformData.Icmov, toTransformData.NotaCredito, toTransformData.Tipo_NC),
          Order_Requested_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Usuario),
          Order_Responsible_Id = dataServices.GetPartyIdFromParties(toTransformData.Usuario),
          Order_Beneficary_Id = dataServices.GetWareHouseIdFromCommonStorage(toTransformData.Almacen),
          Order_Provider_Id  = dataServices.GetPartyIdFromParties(toTransformData.Cliente),
          Order_Budget_Id = -1,
          Order_Requisition_Id = -1,
          Order_Contract_Id = -1,
          Order_Project_Id = -1,
          Order_Currency_Id = 600,
          Order_Source_Id = -1,
          Order_Priority = 'N',
          Order_Authorization_Time = toTransformData.Fecha,
          Order_Authorized_By_Id = -1,
          Order_Closing_Time = ExecutionServer.DateMinValue,
          Order_Closed_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Usuario),
          Order_Ext_Data = "",
          Order_Keywords = Empiria.EmpiriaString.BuildKeywords(toTransformData.NotaCredito, toTransformData.Factura, toTransformData.Devolucion, toTransformData.Factura, toTransformData.Almacen, toTransformData.Icmov, toTransformData.NotaCredito, toTransformData.Tipo_NC),
          Order_Posted_By_Id  = dataServices.GetPartyIdFromParties(toTransformData.Usuario),
          Order_Posting_Time  = toTransformData.FechaCaptura,
          Order_Status = dataServices.ReturnStatusForOrdersStatus(toTransformData.Cancelada)
        };
      } else {
        return new OrderData {
          Order_Id = dataServices.GetOrderIdFromOMSOrders(toTransformData.NotaCredito),
          Order_UID = dataServices.GetOrderUIDFromOMSOrders(toTransformData.NotaCredito),
          Order_Type_Id = 4009,
          Order_Category_Id = -1,
          Order_No = toTransformData.NotaCredito,
          Order_Description = toTransformData.Observaciones,
          Order_Identificators = toTransformData.Factura,
          Order_Tags = Empiria.EmpiriaString.BuildKeywords(toTransformData.NotaCredito, toTransformData.Factura, toTransformData.Devolucion, toTransformData.Factura, toTransformData.Almacen, toTransformData.Icmov, toTransformData.NotaCredito, toTransformData.Tipo_NC),
          Order_Requested_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Usuario),
          Order_Responsible_Id = dataServices.GetPartyIdFromParties(toTransformData.Usuario),
          Order_Beneficary_Id = dataServices.GetWareHouseIdFromCommonStorage(toTransformData.Almacen),
          Order_Provider_Id = dataServices.GetPartyIdFromParties(toTransformData.Cliente),
          Order_Budget_Id = -1,
          Order_Requisition_Id = -1,
          Order_Contract_Id = -1,
          Order_Project_Id = -1,
          Order_Currency_Id = 600,
          Order_Source_Id = -1,
          Order_Priority = 'N',
          Order_Authorization_Time = toTransformData.Fecha,
          Order_Authorized_By_Id = -1,
          Order_Closing_Time = ExecutionServer.DateMinValue,
          Order_Closed_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Usuario),
          Order_Ext_Data = "",
          Order_Keywords = Empiria.EmpiriaString.BuildKeywords(toTransformData.NotaCredito, toTransformData.Factura, toTransformData.Devolucion, toTransformData.Factura, toTransformData.Almacen, toTransformData.Icmov, toTransformData.NotaCredito, toTransformData.Tipo_NC),
          Order_Posted_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Usuario),
          Order_Posting_Time = toTransformData.FechaCaptura,
          Order_Status = dataServices.ReturnStatusForOrdersStatus(toTransformData.Cancelada)
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

  }  // class OrderCreditNoteTransformer

} // namespace Empiria.Trade.Integration.ETL.Transformers

