/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Services Layer                        *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Service provider                      *
*  Type     : OrderReturnTransformer                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Transforms a Order(Devolucion) from NK to Empiria Trade.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;
using Empiria.Json;
using Empiria.Trade.Integration.ETL.Data;
using Newtonsoft.Json;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>Transforms a order(Devolucion) from NK to Empiria Trade.</summary>
  public class OrderReturnTransformer {

    private readonly string _connectionString;

    internal OrderReturnTransformer(string connectionString) {
      Assertion.Require(connectionString, nameof(connectionString));

      _connectionString = connectionString;
    }

    public void Execute() {

      FixedList<OrderReturnNK> sourceData = ReadSourceData();

      FixedList<OrderData> transformedData = Transform(sourceData);

      int result = WriteTargetData(transformedData);
      if (result == 1) {
        // si fue exitoso el guardado de orders, guardar items
        var orderItemsReturnTransformer = new OrderItemsReturnTransformer(_connectionString);
        FixedList<OrderItemsReturnNK> sourceDataItems = orderItemsReturnTransformer.ReadSourceData();

        FixedList<OrderItemsData> transformedDataItems = orderItemsReturnTransformer.Transform(sourceDataItems);

        orderItemsReturnTransformer.WriteTargetData(transformedDataItems);
      } else {
        Assertion.EnsureFailed("Error al guardar Order Return");
      }
    }


    private FixedList<OrderReturnNK> ReadSourceData() {
      var sql = "SELECT O.Devolucion,O.Aplicada,O.Fecha,O.Cliente,O.Usuario,O.Observaciones,O.Cancelada,O.FechaCaptura " +
                " ,O.Factura,O.ICMOV,O.MotivoDevolucion,O.Almacen,O.Referencia,O.Concepto,O.Estatus,O.NotaCredito " +
                " ,O.Folio,O.Serie,O.Usuario_Cancelo,O.Fecha_Cancelo,O.MD,O.Moneda,O.Aplicar_Cargofactura,O.PCS, O.Tipo_NC " +
                " ,O.Vendedor,O.Telemarketer,O.SAT_Formapago,O.Terminada,O.BinaryChecksum,O.OldBinaryChecksum " +
                " FROM sources.DEVOLUCION_TARGET O " +
                " Where O.Fecha >= '2025-01-01' And(O.Oldbinarychecksum != O.Binarychecksum " +
                " Or O.Oldbinarychecksum = 0 Or O.Oldbinarychecksum IS NULL) ";

      var connectionString = GetNKConnectionString();

      var inputDataService = new TransformerDataServices(connectionString);

      return inputDataService.ReadData<OrderReturnNK>(sql);
    }


    private FixedList<OrderData> Transform(FixedList<OrderReturnNK> toTransformData) {
      return toTransformData.Select(x => Transform(x))
                            .ToFixedList();
    }


    private OrderData Transform(OrderReturnNK toTransformData) {
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
          Order_Requisition_Id = -1,
          Order_Contract_Id = -1,
          Order_Parent_Id = -1,
          Order_No = toTransformData.Devolucion,
          Order_Name = "",
          Order_Description = toTransformData.Referencia,
          Order_Observations = "",
          Order_Justification = "",
          Order_Identificators = toTransformData.Factura,
          Order_Tags = Empiria.EmpiriaString.BuildKeywords(toTransformData.Factura, toTransformData.Devolucion, toTransformData.Factura, toTransformData.Almacen, toTransformData.ICMOV, toTransformData.NotaCredito, toTransformData.Tipo_NC),
          Order_Start_Date = ExecutionServer.DateMinValue,
          Order_End_Date = ExecutionServer.DateMaxValue,
          Order_Requested_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Usuario, "User"),
          Order_Requested_Time = ExecutionServer.DateMinValue,
          Order_Required_Time = ExecutionServer.DateMaxValue,
          Order_Responsible_Id = dataServices.GetPartyIdFromParties(toTransformData.Usuario, "User"),
          Order_Beneficary_Id = dataServices.GetWareHouseIdFromCommonStorage(toTransformData.Almacen),
          Order_Provider_Id = dataServices.GetPartyIdFromParties(toTransformData.Cliente, "Client"),
          Order_Warehouse_Id = dataServices.GetWareHouseIdFromCommonStorage(toTransformData.Almacen),
          Order_Delivery_Place_Id = -1,
          Order_Project_Id = -1,
          Order_Geo_Origin_Id = -1,
          Order_Currency_Id = 600,
          Order_Budget_Type_Id = -1,
          Order_Base_Budget_Id = -1,
          Order_Source_Id = -1,
          Order_Priority = ' ',
          Order_Conditions_Ext_Data = "",
          Order_Specification_Ext_Data = "",
          Order_Delivery_Ext_Data = "",
          Order_Ext_Data = JsonConvert.SerializeObject(new {Name = "Devolucion"}),
          Order_Keywords = Empiria.EmpiriaString.BuildKeywords(toTransformData.Factura, toTransformData.Devolucion, toTransformData.Factura, toTransformData.Almacen, toTransformData.ICMOV, toTransformData.NotaCredito, toTransformData.Tipo_NC),
          Order_Authorization_Time = toTransformData.Fecha,
          Order_Authorized_By_Id = -1,
          Order_Closing_Time = ExecutionServer.DateMinValue,
          Order_Closed_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Usuario, "User"),
          Order_Posting_Time = toTransformData.FechaCaptura,
          Order_Posted_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Usuario, "User"),
          Order_Status = dataServices.ReturnStatusForOrdersStatus(toTransformData.Cancelada)
         };
      } else {
        return new OrderData {
          Order_Id = dataServices.GetOrderIdFromOMSOrders(toTransformData.Factura),
          Order_UID = dataServices.GetOrderUIDFromOMSOrders(toTransformData.Factura),
          Order_Type_Id = 4009,
          Order_Category_Id = -1,
          Order_Requisition_Id = -1,
          Order_Contract_Id = -1,
          Order_Parent_Id = -1,
          Order_No = toTransformData.Devolucion,
          Order_Name = "",
          Order_Description = toTransformData.Referencia,
          Order_Observations = "",
          Order_Justification = "",
          Order_Identificators = toTransformData.Factura,
          Order_Tags = Empiria.EmpiriaString.BuildKeywords(toTransformData.Factura, toTransformData.Devolucion, toTransformData.Factura, toTransformData.Almacen, toTransformData.ICMOV, toTransformData.NotaCredito, toTransformData.Tipo_NC),
          Order_Start_Date = ExecutionServer.DateMinValue,
          Order_End_Date = ExecutionServer.DateMaxValue,
          Order_Requested_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Usuario, "User"),
          Order_Requested_Time = ExecutionServer.DateMinValue,
          Order_Required_Time = ExecutionServer.DateMaxValue,
          Order_Responsible_Id = dataServices.GetPartyIdFromParties(toTransformData.Usuario, "User"),
          Order_Beneficary_Id = dataServices.GetWareHouseIdFromCommonStorage(toTransformData.Almacen),
          Order_Provider_Id = dataServices.GetPartyIdFromParties(toTransformData.Cliente, "Client"),
          Order_Warehouse_Id = dataServices.GetWareHouseIdFromCommonStorage(toTransformData.Almacen),
          Order_Delivery_Place_Id = -1,
          Order_Project_Id = -1,
          Order_Geo_Origin_Id = -1,
          Order_Currency_Id = 600,
          Order_Budget_Type_Id = -1,
          Order_Base_Budget_Id = -1,
          Order_Source_Id = -1,
          Order_Priority = ' ',
          Order_Conditions_Ext_Data = "",
          Order_Specification_Ext_Data = "",
          Order_Delivery_Ext_Data = "",
          Order_Ext_Data = JsonConvert.SerializeObject(new { Name = "Devolucion" }),
          Order_Keywords = Empiria.EmpiriaString.BuildKeywords(toTransformData.Factura, toTransformData.Devolucion, toTransformData.Factura, toTransformData.Almacen, toTransformData.ICMOV, toTransformData.NotaCredito, toTransformData.Tipo_NC),
          Order_Authorization_Time = toTransformData.Fecha,
          Order_Authorized_By_Id = -1,
          Order_Closing_Time = ExecutionServer.DateMinValue,
          Order_Closed_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Usuario, "User"),
          Order_Posting_Time = toTransformData.FechaCaptura,
          Order_Posted_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Usuario, "User"),
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
        var op = DataOperation.Parse("write_OMS_Order", o.Order_Id, o.Order_UID, o.Order_Type_Id, o.Order_Category_Id, o.Order_Requisition_Id,
         o.Order_Contract_Id, o.Order_Parent_Id, o.Order_No, o.Order_Name, o.Order_Description, o.Order_Observations,
         o.Order_Justification, o.Order_Identificators, o.Order_Tags, o.Order_Start_Date, o.Order_End_Date, o.Order_Requested_By_Id,
         o.Order_Requested_Time, o.Order_Required_Time, o.Order_Responsible_Id, o.Order_Beneficary_Id, o.Order_Provider_Id,
         o.Order_Warehouse_Id, o.Order_Delivery_Place_Id, o.Order_Project_Id, o.Order_Geo_Origin_Id, o.Order_Currency_Id,
         o.Order_Budget_Type_Id, o.Order_Base_Budget_Id, o.Order_Source_Id, o.Order_Priority, o.Order_Conditions_Ext_Data,
         o.Order_Specification_Ext_Data, o.Order_Delivery_Ext_Data, o.Order_Ext_Data, o.Order_Keywords, o.Order_Authorization_Time,
         o.Order_Authorized_By_Id, o.Order_Closing_Time, o.Order_Closed_By_Id, o.Order_Posting_Time, o.Order_Posted_By_Id, o.Order_Status);

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

  }  // class OrderReturnTransformer

} // namespace Empiria.Trade.Integration.ETL.Transformers

