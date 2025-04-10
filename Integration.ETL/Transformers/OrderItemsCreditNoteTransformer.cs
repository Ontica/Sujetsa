/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Services Layer                        *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Service provider                      *
*  Type     : OrderItemsCreditNoteTransformer                 License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Transforms a OrderItem(NotaCreditoDet) from NK to Empiria Trade.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Data;
using Empiria.Json;
using Empiria.Trade.Integration.ETL.Data;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>Transforms a order item(NotaCreditoDet) from NK to Empiria Trade.</summary>
  public class OrderItemsCreditNoteTransformer {

    private readonly string _connectionString;

    internal OrderItemsCreditNoteTransformer(string connectionString) {
      Assertion.Require(connectionString, nameof(connectionString));

      _connectionString = connectionString;
    }

    public void Execute() {

      FixedList<OrderItemsCreditNoteNK> sourceData = ReadSourceData();

      FixedList<OrderItemsData> transformedData = Transform(sourceData);

      WriteTargetData(transformedData);

      var connectionString = GetNKConnectionString();

      var outputDataServices = new SqlServerDataServices(connectionString);

      outputDataServices.ExecuteUpdateOrderItemsStatusStoredProcedure();
    }


    private FixedList<OrderItemsCreditNoteNK> ReadSourceData() {
      var sql = "SELECT O.NOTACREDITO,O.DET,O.CANTIDAD,O.UNIDAD_VENTA_MENUDEO_C,O.UNIDAD," +
                " O.PRODUCTO,O.PRECIO,O.IMPORTE,O.DESCRIPCION,O.CLAVEIMPUESTO,O.BinaryChecksum," +
                " O.OldBinaryChecksum FROM sources.NOTACREDITODET_TARGET O " +
                " JOIN sources.NOTACREDITO_TARGET V  ON V.NOTACREDITO = O.NOTACREDITO AND V.FECHA >= '2025-01-01'" +
                " AND(O.OldBinaryChecksum != O.BinaryChecksum" +
                " OR O.OldBinaryChecksum = 0) " +
                " AND V.TIPO = 'D'";

      var connectionString = GetNKConnectionString();

      var inputDataService = new TransformerDataServices(connectionString);

      return inputDataService.ReadData<OrderItemsCreditNoteNK>(sql);
    }


    private FixedList<OrderItemsData> Transform(FixedList<OrderItemsCreditNoteNK> toTransformData) {
      return toTransformData.Select(x => Transform(x))
                            .ToFixedList();
    }


    private OrderItemsData Transform(OrderItemsCreditNoteNK toTransformData) {
      string connectionString = GetEmpiriaConnectionString();
      var dataServices = new TransformerDataServices(connectionString);
      if (toTransformData.OldBinaryChecksum == 0) {
        return new OrderItemsData {
          Order_Item_Id = dataServices.GetNextId("OMS_Order_Items"),
          Order_Item_UID = System.Guid.NewGuid().ToString(),
          Order_Item_Type_Id = 4058,
          Order_Item_Order_Id = dataServices.GetOrderIdFromOMSOrders(toTransformData.NotaCredito),
          Order_Item_Product_Id = dataServices.GetProductIdFromOMSProducts(toTransformData.Producto), 
          Order_Item_Description = toTransformData.Descripcion,
          Order_Item_Product_Unit_Id = (int) dataServices.ReturnIdForProductBaseUnitId(toTransformData.Unidad),
          Order_Item_Product_Qty = toTransformData.Cantidad,
          Order_Item_Unit_Price = toTransformData.Precio,
          Order_Item_Discount =0,
          Order_Item_Currency_Id = 600,
          Order_Item_Related_Item_Id = -1,
          Order_Item_Requisition_Item_Id = -1,
          Order_Item_Requested_By_Id = dataServices.GetRequestedUserIdFromOMSOrders(toTransformData.NotaCredito),
          Order_Item_Budget_Account_Id = -1,
          Order_Item_Project_Id = -1,
          Order_Item_Provider_Id = (int) dataServices.GetOrderItemProviderIdFromOMSOrders(toTransformData.NotaCredito),
          Order_Item_Per_Each_Item_Id = -1,
          Order_Item_Ext_Data = "",
          Order_Item_Keywords = Empiria.EmpiriaString.BuildKeywords(toTransformData.NotaCredito, toTransformData.Producto,  toTransformData.ClaveImpuesto),
          Order_Item_Position = toTransformData.Det,
          Order_Item_Posted_By_Id = dataServices.GetPostedUserIdFromOMSOrders(toTransformData.NotaCredito),
          Order_Item_Posting_Time = dataServices.GetPostedDateFromOMSOrders(toTransformData.NotaCredito), 
          Order_Item_Status = Convert.ToChar(dataServices.GetOrderItemStatusFromOMSOrders(toTransformData.NotaCredito))
        };
      } else {
        return new OrderItemsData {
          Order_Item_Id = dataServices.GetOrderIdFromOMSOrderItems(dataServices.GetOrderIdFromOMSOrders(toTransformData.NotaCredito), toTransformData.Det),
          Order_Item_UID = dataServices.GetOrderUIDFromOMSOrderItems(dataServices.GetOrderIdFromOMSOrders(toTransformData.NotaCredito), toTransformData.Det),
          Order_Item_Type_Id = 4058,
          Order_Item_Order_Id = dataServices.GetOrderIdFromOMSOrders(toTransformData.NotaCredito),
          Order_Item_Product_Id = dataServices.GetProductIdFromOMSProducts(toTransformData.Producto),
          Order_Item_Description = toTransformData.Descripcion,
          Order_Item_Product_Unit_Id = (int) dataServices.ReturnIdForProductBaseUnitId(toTransformData.Unidad),
          Order_Item_Product_Qty = toTransformData.Cantidad,
          Order_Item_Unit_Price = toTransformData.Precio,
          Order_Item_Discount =0,
          Order_Item_Currency_Id = 600,
          Order_Item_Related_Item_Id = -1,
          Order_Item_Requisition_Item_Id = -1,
          Order_Item_Requested_By_Id = dataServices.GetRequestedUserIdFromOMSOrders(toTransformData.NotaCredito),
          Order_Item_Budget_Account_Id = -1,
          Order_Item_Project_Id = -1,
          Order_Item_Provider_Id = (int) dataServices.GetOrderItemProviderIdFromOMSOrders(toTransformData.NotaCredito),
          Order_Item_Per_Each_Item_Id = -1,
          Order_Item_Ext_Data = "",
          Order_Item_Keywords = Empiria.EmpiriaString.BuildKeywords(toTransformData.NotaCredito, toTransformData.Producto,  toTransformData.ClaveImpuesto),
          Order_Item_Position = toTransformData.Det,
          Order_Item_Posted_By_Id = dataServices.GetPostedUserIdFromOMSOrders(toTransformData.NotaCredito),
          Order_Item_Posting_Time = dataServices.GetPostedDateFromOMSOrders(toTransformData.NotaCredito),
          Order_Item_Status = Convert.ToChar(dataServices.GetOrderItemStatusFromOMSOrders(toTransformData.NotaCredito))
        };
      }
    }
    

    private void WriteTargetData(FixedList<OrderItemsData> transformedData) {
      foreach (var item in transformedData) {
        WriteTargetData(item);
      }
    }


    private void WriteTargetData(OrderItemsData o) {
        var op = DataOperation.Parse("write_OMS_Order_Item", o.Order_Item_Id,  o.Order_Item_UID,  o.Order_Item_Type_Id,  o.Order_Item_Order_Id,
        o.Order_Item_Product_Id,  o.Order_Item_Description,  o.Order_Item_Product_Unit_Id,  o.Order_Item_Product_Qty,  o.Order_Item_Unit_Price,
        o.Order_Item_Discount,  o.Order_Item_Currency_Id,  o.Order_Item_Related_Item_Id,  o.Order_Item_Requisition_Item_Id,  o.Order_Item_Requested_By_Id,
        o.Order_Item_Budget_Account_Id,  o.Order_Item_Project_Id,  o.Order_Item_Provider_Id,  o.Order_Item_Per_Each_Item_Id,  o.Order_Item_Ext_Data,
        o.Order_Item_Keywords,  o.Order_Item_Position,  o.Order_Item_Posted_By_Id,  o.Order_Item_Posting_Time,  o.Order_Item_Status);

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

  }  // class OrderItemsCreditNoteTransformer
} // namespace Empiria.Trade.Integration.ETL.Transformers

