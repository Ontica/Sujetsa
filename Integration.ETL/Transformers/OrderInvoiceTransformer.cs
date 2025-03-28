/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Services Layer                        *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Service provider                      *
*  Type     : OrderInvoiceTransformer                      License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Transforms a Order(Factura) from NK to Empiria Trade.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Data;
using Empiria.Json;
using Empiria.Trade.Integration.ETL.Data;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>Transforms a order(Factura) from NK to Empiria Trade.</summary>
  public class OrderInvoiceTransformer {

    private readonly string _connectionString;

    internal OrderInvoiceTransformer(string connectionString) {
      Assertion.Require(connectionString, nameof(connectionString));

      _connectionString = connectionString;
    }

    public void Execute() {

      FixedList<OrderInvoiceNK> sourceData = ReadSourceData();

      FixedList<OrderData> transformedData = Transform(sourceData);

      WriteTargetData(transformedData);
    }


    private FixedList<OrderInvoiceNK> ReadSourceData() {
      var sql = "SELECT O.FACTURA,O.TIPO,O.TIPOPAGO,O.FR,O.CLIENTE,O.FORMAPAGO,O.MONEDA," +
        "O.TIPOCAMBIO,O.IMPORTE,O.CARGOS,O.DESCUENTO,O.SUBTOTAL," +
        "O.IVA,O.R_IVA,O.R_ISR,O.TOTAL,O.USUARIO,O.CANCELADA,O.FECHA," +
        "O.PEDIDO,O.APLICADA,O.VENDEDOR,O.IEPS,O.OV,O.ALMACEN,O.ICMOV," +
        "O.CONDICIONDEPAGO,O.FECHA_ENTREGA,O.ENTREGA,O.SUBCLIENTE," +
        "O.VENCIMIENTOS,O.POLIZA,O.SINC_S,O.OC,O.ESTATUS,O.RUTA,O.SUBRUTA," +
        "O.DESGLOSA_IEPS,O.USUARIOCANCELO,O.FECHACANCELO,O.CONSIGNACION," +
        "O.MD,O.TCAMBIO,O.IMPORTE_A,O.DESCUENTO_A,O.SUBTOTAL_A,O.IEPS_A," +
        "O.IVA_A,O.R_IVA_A,O.R_ISR_A,O.TOTAL_A,O.REVERSE_DE,O.SERIE," +
        "O.CONTADOR,O.HORA,O.CFD,O.UBICACION,O.CONSECUTIVO,O.SALIDA," +
        "O.VER,O.TRANSACCION,O.TELEMARKETER,O.USOCFDI,O.SAT_METODOPAGO," +
        "O.SAT_FORMAPAGO,O.SAT_REGIMENFISCAL,O.TIPOCAMBIOUSD," +
        "O.BinaryChecksum,O.OldBinaryChecksum FROM sources.FACTURA_TARGET O " +
        "WHERE O.FECHA >= '2025-01-01' and (O.OldBinaryChecksum != O.BinaryChecksum  " +
        "OR O.OldBinaryChecksum = 0    " +
        "OR O.OldBinaryChecksum IS NULL)";

      var connectionString = GetNKConnectionString();

      var inputDataService = new TransformerDataServices(connectionString);

      return inputDataService.ReadData<OrderInvoiceNK>(sql);
    }


    private FixedList<OrderData> Transform(FixedList<OrderInvoiceNK> toTransformData) {
      return toTransformData.Select(x => Transform(x))
                            .ToFixedList();
    }


    private OrderData Transform(OrderInvoiceNK toTransformData) {
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
          Order_No = toTransformData.Factura,
          Order_Description = Empiria.EmpiriaString.BuildKeywords(toTransformData.Factura, toTransformData.Cliente,  toTransformData.Ov, toTransformData.Almacen, toTransformData.Icmov),
          Order_Identificators = toTransformData.Factura,
          Order_Tags = Empiria.EmpiriaString.BuildKeywords(toTransformData.Factura, toTransformData.Cliente, toTransformData.SubCliente, toTransformData.Almacen),
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
          Order_Priority = ' ',////////////
          Order_Authorization_Time = toTransformData.Fecha,
          Order_Authorized_By_Id = -1,
          Order_Closing_Time = toTransformData.FechaEntrega,
          Order_Closed_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Vendedor),/////////////
          Order_Ext_Data = "",
          Order_Keywords = Empiria.EmpiriaString.BuildKeywords(toTransformData.Factura, toTransformData.Cliente, toTransformData.SubCliente, toTransformData.Almacen),
          Order_Posted_By_Id  = dataServices.GetPartyIdFromParties(toTransformData.Vendedor),
          Order_Posting_Time  = toTransformData.Fecha,
          Order_Status = 'Y'//////toTransformData.Estatus///////////////////
        };
      } else {
        return new OrderData {
          Order_Id = dataServices.GetOrderIdFromOMSOrders(toTransformData.Factura),
          Order_UID = dataServices.GetOrderUIDFromOMSOrders(toTransformData.Factura),
          Order_Type_Id = 4001,
          Order_Category_Id = -1,
          Order_No = toTransformData.Factura,
          Order_Description = Empiria.EmpiriaString.BuildKeywords(toTransformData.Factura, toTransformData.Cliente, toTransformData.Ov, toTransformData.Almacen, toTransformData.Icmov),
          Order_Identificators = toTransformData.Factura,
          Order_Tags = Empiria.EmpiriaString.BuildKeywords(toTransformData.Factura, toTransformData.Cliente, toTransformData.SubCliente, toTransformData.Almacen),
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
          Order_Priority = ' ',////////////
          Order_Authorization_Time = toTransformData.Fecha,
          Order_Authorized_By_Id = -1,
          Order_Closing_Time = toTransformData.FechaEntrega,
          Order_Closed_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Vendedor),/////////////
          Order_Ext_Data = "",
          Order_Keywords = Empiria.EmpiriaString.BuildKeywords(toTransformData.Factura, toTransformData.Cliente, toTransformData.SubCliente, toTransformData.Almacen),
          Order_Posted_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Vendedor),
          Order_Posting_Time = toTransformData.Fecha,
          Order_Status = 'Y'/////toTransformData.Estatus///////////////////
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

  }  // class OrderInvoiceTransformer

} // namespace Empiria.Trade.Integration.ETL.Transformers

