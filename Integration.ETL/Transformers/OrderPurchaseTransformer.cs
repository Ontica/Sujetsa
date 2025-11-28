/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Services Layer                        *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Service provider                      *
*  Type     : OrderPurchaseTransformer                     License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Transforms a Order(Compra) from NK to Empiria Trade.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;
using Empiria.Json;
using Empiria.Trade.Integration.ETL.Data;
using Empiria.Trade.Integration.ETL.Transformers;
using Newtonsoft.Json;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>Transforms a order(Compra) from NK to Empiria Trade.</summary>
  public class OrderPurchaseTransformer {

    private readonly string _connectionString;

    internal OrderPurchaseTransformer(string connectionString) {
      Assertion.Require(connectionString, nameof(connectionString));

      _connectionString = connectionString;
    }

    public void Execute() {

      FixedList<OrderPurchaseNK> sourceData = ReadSourceData();

      FixedList<OrderData> transformedData = Transform(sourceData);

       int result = WriteTargetData(transformedData);
      if (result == 1) {
        // si fue exitoso el guardado de orders, guardar items
        var orderItemsPurchaseTransformer = new OrderItemsPurchaseTransformer(_connectionString);
        FixedList<OrderItemsPurchaseNK> sourceDataItems = orderItemsPurchaseTransformer.ReadSourceData();

        FixedList<OrderItemsData> transformedDataItems = orderItemsPurchaseTransformer.Transform(sourceDataItems);

        orderItemsPurchaseTransformer.WriteTargetData(transformedDataItems);

      } else {
        Assertion.EnsureFailed("Error al guardar Order Purchase");
      }

    }


    private FixedList<OrderPurchaseNK> ReadSourceData() {
      var sql = "SELECT O.Compra,	 O.Fecha,	 O.OC,	 O.Almacen,	 O.Proveedor," +
        " O.Factura,	 O.Flete,	 O.Porcentaje_Flete,	 O.Fecha_Recepcion," +
        " O.Fecha_Facturap,	 O.Fecha_Embarquep,	 O.Referencia,	 O.Icmov," +
        " O.Vencimientos,	 O.Aplicado,	 O.Cancelada,	 O.Impreso,	 O.Fecha_PP," +
        " O.Plazo_EN,	 O.Captura,	 O.Usuario,	 O.F_Recep_Factoriginal," +
        " O.Clase_Docto,	 O.Aptipo,	 O.Moneda,	 O.Importacion,	 O.ClaveImpuesto_Flete," +
        " O.Sinc,	 O.Utilizar_Fv_Fija,	 O.Fv_Fija,	 O.CFDP,	 O.TipoCompra," +
        "	 O.R_IVA_PCT,	 O.Serie,	 O.Contador,	 O.Binarychecksum,	 O.Oldbinarychecksum" +
        "  FROM sources.COMPRA_TARGET O " +
        "Where O.Fecha >= '2025-01-01' And(O.Oldbinarychecksum != O.Binarychecksum " +
        "Or O.Oldbinarychecksum = 0 " +
        "Or O.Oldbinarychecksum IS NULL)";

      var connectionString = GetNKConnectionString();

      var inputDataService = new TransformerDataServices(connectionString);

      return inputDataService.ReadData<OrderPurchaseNK>(sql);
    }


    private FixedList<OrderData> Transform(FixedList<OrderPurchaseNK> toTransformData) {
      return toTransformData.Select(x => Transform(x))
                            .ToFixedList();
    }


    private OrderData Transform(OrderPurchaseNK toTransformData) {
      string connectionString = GetEmpiriaConnectionString();
      var dataServices = new TransformerDataServices(connectionString);
      string connectionStringNK = GetNKConnectionString();
      var dataServicesNK = new TransformerDataServices(connectionStringNK);
      if (toTransformData.OldBinaryChecksum == 0) {
        return new OrderData {
          Order_Id = dataServices.GetNextId("OMS_Orders"),
          Order_UID = System.Guid.NewGuid().ToString(),
          Order_Type_Id = 4006,//4005,
          Order_Category_Id = -1,
          Order_Requisition_Id = -1,
          Order_Contract_Id = -1,
          Order_Parent_Id = -1,
          Order_No = toTransformData.Compra,
          Order_Name = "",
          Order_Description = toTransformData.Referencia,
          Order_Observations = "",
          Order_Justification = "",
          Order_Identificators = toTransformData.Factura,
          Order_Tags = Empiria.EmpiriaString.BuildKeywords(toTransformData.Factura, toTransformData.Compra, toTransformData.OC, toTransformData.Almacen, toTransformData.Icmov),
          Order_Start_Date = ExecutionServer.DateMinValue,
          Order_End_Date = ExecutionServer.DateMaxValue,
          Order_Requested_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Usuario, "User"),
          Order_Requested_Time = ExecutionServer.DateMinValue,
          Order_Required_Time = ExecutionServer.DateMaxValue,
          Order_Responsible_Id = dataServices.GetPartyIdFromParties(toTransformData.Usuario, "User"),
          Order_Beneficary_Id = dataServices.GetWareHouseIdFromCommonStorage(toTransformData.Almacen),
          Order_Provider_Id = dataServices.GetPartyIdFromParties(toTransformData.Proveedor, "Supplier"),
          Order_Warehouse_Id = dataServices.GetWareHouseIdFromCommonStorage(toTransformData.Almacen),
          Order_Delivery_Place_Id = -1,
          Order_Project_Id = -1,
          Order_Geo_Origin_Id = -1,
          Order_Currency_Id = 600,
          Order_Budget_Type_Id = -1,
          Order_Base_Budget_Id = -1,
          Order_Source_Id = -1,
          Order_Priority = 'N',
          Order_Conditions_Ext_Data = "",
          Order_Specification_Ext_Data = "",
          Order_Delivery_Ext_Data = "",
          Order_Ext_Data = JsonConvert.SerializeObject(new {Name = "Compra"}),
          Order_Keywords = Empiria.EmpiriaString.BuildKeywords(toTransformData.Factura, toTransformData.Compra, toTransformData.TipoCompra, toTransformData.Almacen),
          Order_Authorization_Time = toTransformData.Fecha,
          Order_Authorized_By_Id = -1,
          Order_Closing_Time = toTransformData.Fecha_Recepcion,
          Order_Closed_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Usuario, "User"),
          Order_Posting_Time = toTransformData.Captura,
          Order_Posted_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Usuario, "User"),
          Order_Status = dataServices.ReturnStatusForOrdersStatus(toTransformData.Cancelada)
         };
      } else {
        return new OrderData {
          Order_Id = dataServices.GetOrderIdFromOMSOrders(toTransformData.Compra),
          Order_UID = dataServices.GetOrderUIDFromOMSOrders(toTransformData.Compra),
          Order_Type_Id = 4006,//4005,
          Order_Category_Id = -1,
          Order_Requisition_Id = -1,
          Order_Contract_Id = -1,
          Order_Parent_Id = -1,
          Order_No = toTransformData.Compra,
          Order_Name = "",
          Order_Description = toTransformData.Referencia,
          Order_Observations = "",
          Order_Justification = "",
          Order_Identificators = toTransformData.Factura,
          Order_Tags = Empiria.EmpiriaString.BuildKeywords(toTransformData.Factura, toTransformData.Compra, toTransformData.OC, toTransformData.Almacen, toTransformData.Icmov),
          Order_Start_Date = ExecutionServer.DateMinValue,
          Order_End_Date = ExecutionServer.DateMaxValue,
          Order_Requested_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Usuario, "User"),
          Order_Requested_Time = ExecutionServer.DateMinValue,
          Order_Required_Time = ExecutionServer.DateMaxValue,
          Order_Responsible_Id = dataServices.GetPartyIdFromParties(toTransformData.Usuario, "User"),
          Order_Beneficary_Id = dataServices.GetWareHouseIdFromCommonStorage(toTransformData.Almacen),
          Order_Provider_Id = dataServices.GetPartyIdFromParties(toTransformData.Proveedor, "Supplier"),
          Order_Warehouse_Id = dataServices.GetWareHouseIdFromCommonStorage(toTransformData.Almacen),
          Order_Delivery_Place_Id = -1,
          Order_Project_Id = -1,
          Order_Geo_Origin_Id = -1,
          Order_Currency_Id = 600,
          Order_Budget_Type_Id = -1,
          Order_Base_Budget_Id = -1,
          Order_Source_Id = -1,
          Order_Priority = 'N',
          Order_Conditions_Ext_Data = "",
          Order_Specification_Ext_Data = "",
          Order_Delivery_Ext_Data = "",
          Order_Ext_Data = JsonConvert.SerializeObject(new { Name = "Compra" }),
          Order_Keywords = Empiria.EmpiriaString.BuildKeywords(toTransformData.Factura, toTransformData.Compra, toTransformData.TipoCompra, toTransformData.Almacen),
          Order_Authorization_Time = toTransformData.Fecha,
          Order_Authorized_By_Id = -1,
          Order_Closing_Time = toTransformData.Fecha_Recepcion,
          Order_Closed_By_Id = dataServices.GetPartyIdFromParties(toTransformData.Usuario, "User"),
          Order_Posting_Time = toTransformData.Captura,
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

  }  // class OrderPurchaseTransformer

} // namespace Empiria.Trade.Integration.ETL.Transformers

