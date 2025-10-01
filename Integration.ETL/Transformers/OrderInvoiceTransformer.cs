/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Services Layer                        *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Service provider                      *
*  Type     : OrderInvoiceTransformer                      License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Transforms a Order(Factura) from NK to Empiria Trade.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;
using Empiria.Json;
using Empiria.Trade.Integration.ETL.Data;
using Newtonsoft.Json;

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

      int result = WriteTargetData(transformedData);
      if (result == 1) {
        // si fue exitoso el guardado de orders, guardar items
        var orderItemsInvoiceTransformer = new OrderItemsInvoiceTransformer(_connectionString);
        FixedList<OrderItemsInvoiceNK> sourceDataItems = orderItemsInvoiceTransformer.ReadSourceData();

        FixedList<OrderItemsData> transformedDataItems = orderItemsInvoiceTransformer.Transform(sourceDataItems);

        orderItemsInvoiceTransformer.WriteTargetData(transformedDataItems);

      } else {
        Assertion.EnsureFailed("Error al guardar Order Invoice");
      }     

    }


    private FixedList<OrderInvoiceNK> ReadSourceData() {
      var sql = " Select O.Factura,O.Tipo,O.Tipopago,O.Fr,O.Cliente,O.Formapago,O.Moneda, " +
        "O.Tipocambio,O.Importe,O.Cargos,O.Descuento,O.Subtotal, " +
        "O.Iva,O.R_Iva,O.R_Isr,O.Total,O.Usuario,O.Cancelada,O.Fecha, " +
        "O.Pedido,O.Aplicada,O.Vendedor,O.Ieps,O.Ov,O.Almacen,O.Icmov, " +
        "O.Condiciondepago,O.Fecha_Entrega,O.Entrega,O.Subcliente, " +
        "O.Vencimientos,O.Poliza,O.Sinc_S,O.Oc,O.Estatus,O.Ruta,O.Subruta, " +
        "O.Desglosa_Ieps,O.Usuariocancelo,O.Fechacancelo,O.Consignacion, " +
        "O.Md,O.Tcambio,O.Importe_A,O.Descuento_A,O.Subtotal_A,O.Ieps_A, " +
        "O.Iva_A,O.R_Iva_A,O.R_Isr_A,O.Total_A,O.Reverse_De,O.Serie, " +
        "O.Contador,O.Hora,O.Cfd,O.Ubicacion,O.Consecutivo,O.Salida, " +
        "O.Ver,O.Transaccion,O.Telemarketer,O.Usocfdi,O.Sat_Metodopago, " +
        "O.Sat_Formapago,O.Sat_Regimenfiscal,O.Tipocambiousd, O.Cancelada, " +
        "O.Binarychecksum,O.Oldbinarychecksum From Sources.Factura_Target O " +
        "Where O.Fecha >= '2025-01-01' And(O.Oldbinarychecksum != O.Binarychecksum " +
        "Or O.Oldbinarychecksum = 0 " +
        "Or O.Oldbinarychecksum IS NULL)";

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
          Order_Type_Id = 4011,
          Order_Category_Id = -1,
          Order_No = toTransformData.Factura,
          Order_Description = Empiria.EmpiriaString.BuildKeywords(toTransformData.Factura, toTransformData.Cliente,  toTransformData.Ov, toTransformData.Almacen, toTransformData.Icmov),
          Order_Identificators = toTransformData.Factura,
          Order_Tags = Empiria.EmpiriaString.BuildKeywords(toTransformData.Factura, toTransformData.Cliente, toTransformData.SubCliente, toTransformData.Almacen),
          Order_Requested_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Cliente),
          Order_Responsible_Id = dataServices.GetPartyIdFromParties(toTransformData.Vendedor),
          Order_Beneficary_Id = dataServices.GetPartyIdFromParties(toTransformData.Cliente),
          Order_Provider_Id =  dataServices.GetWareHouseIdFromCommonStorage(toTransformData.Almacen),
          Order_Budget_Id = -1,
          Order_Requisition_Id = -1,
          Order_Contract_Id = -1,
          Order_Project_Id = -1,
          Order_Currency_Id = 600,
          Order_Source_Id = -1,
          Order_Priority = 'N',
          Order_Authorization_Time = toTransformData.Fecha,
          Order_Authorized_By_Id = -1,
          Order_Closing_Time = toTransformData.FechaEntrega,
          Order_Closed_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Vendedor),
          Order_Ext_Data = JsonConvert.SerializeObject(new { Name = "Factura" }),
          Order_Keywords = Empiria.EmpiriaString.BuildKeywords(toTransformData.Factura, toTransformData.Cliente, toTransformData.SubCliente, toTransformData.Almacen),
          Order_Location_Id = dataServices.GetWareHouseIdFromCommonStorage(toTransformData.Almacen),
          Order_Posted_By_Id  = dataServices.GetPartyIdFromParties(toTransformData.Vendedor),
          Order_Posting_Time  = toTransformData.Fecha,
          Order_Status = dataServices.ReturnStatusForOrdersStatus(toTransformData.Cancelada)
        };
      } else {
        return new OrderData {
          Order_Id = dataServices.GetOrderIdFromOMSOrders(toTransformData.Factura),
          Order_UID = dataServices.GetOrderUIDFromOMSOrders(toTransformData.Factura),
          Order_Type_Id = 4011,
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
          Order_Priority =  'N',
          Order_Authorization_Time = toTransformData.Fecha,
          Order_Authorized_By_Id = -1,
          Order_Closing_Time = toTransformData.FechaEntrega,
          Order_Closed_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Vendedor),
          Order_Ext_Data = JsonConvert.SerializeObject(new { Name = "Factura" }),
          Order_Keywords = Empiria.EmpiriaString.BuildKeywords(toTransformData.Factura, toTransformData.Cliente, toTransformData.SubCliente, toTransformData.Almacen),
          Order_Location_Id = dataServices.GetWareHouseIdFromCommonStorage(toTransformData.Almacen),
          Order_Posted_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Vendedor),
          Order_Posting_Time = toTransformData.Fecha,
          Order_Status = dataServices.ReturnStatusForOrdersStatus(toTransformData.Cancelada)
        };
      }
    }
    

    private int WriteTargetData(FixedList<OrderData> transformedData) {
      try {
        foreach (var item in transformedData) {
          WriteTargetData(item);
        }
        return 1; // éxito
      } catch {
        return 0; // fallo
      }
    }


    private void WriteTargetData(OrderData o) {
        var op = DataOperation.Parse("write_OMS_Order", o.Order_Id, o.Order_UID, o.Order_Type_Id, o.Order_Category_Id, o.Order_No
       ,o.Order_Description, o.Order_Identificators, o.Order_Tags, o.Order_Requested_By_Id, o.Order_Responsible_Id, o.Order_Beneficary_Id
       ,o.Order_Provider_Id, o.Order_Budget_Id, o.Order_Requisition_Id, o.Order_Contract_Id, o.Order_Project_Id, o.Order_Currency_Id
       ,o.Order_Source_Id, o.Order_Priority, o.Order_Authorization_Time, o.Order_Authorized_By_Id, o.Order_Closing_Time, o.Order_Closed_By_Id
       ,o.Order_Ext_Data, o.Order_Keywords, o.Order_Location_Id,  o.Order_Posted_By_Id, o.Order_Posting_Time, o.Order_Status);

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

