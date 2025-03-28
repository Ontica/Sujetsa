/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Services Layer                        *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Service provider                      *
*  Type     : OrderTransformerItems                           License   : Please read LICENSE.txt file       *
*                                                                                                            *
*  Summary  : Transforms a OrderItem(OVDet) from NK to Empiria Trade.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Data;
using Empiria.Json;
using Empiria.Trade.Integration.ETL.Data;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>Transforms a order item(OVDet) from NK to Empiria Trade.</summary>
  public class OrderItemsTransformer {

    private readonly string _connectionString;

    internal OrderItemsTransformer(string connectionString) {
      Assertion.Require(connectionString, nameof(connectionString));

      _connectionString = connectionString;
    }

    public void Execute() {

      FixedList<OrderItemsNK> sourceData = ReadSourceData();

      //FixedList<OrderItemsNK> toTransformData = ExtractDataToTransform(sourceData);

      FixedList<OrderItemsData> transformedData = Transform(sourceData);

      WriteTargetData(transformedData);
    }

    private FixedList<OrderItemsNK> ExtractDataToTransform(FixedList<OrderItemsNK> sourceData) {
      return sourceData.FindAll(x => x.OldBinaryChecksum != x.BinaryChecksum || x.OldBinaryChecksum == 0);
    }

    private FixedList<OrderItemsNK> ReadSourceData() {
      var sql = "SELECT O.OV,O.Producto,O.Unidad,O.Referencia,O.Cantidad,O.Precio,O.Descuento,O.Det,O.Almacen,O.OldBinaryChecksum,O.BinaryChecksum "+
          "FROM sources.OVDET_TARGET O " +
          "JOIN sources.OV_TARGET V  ON V.OV = O.OV AND V.FECHA >= '2025-03-01' " +
          "AND(O.OldBinaryChecksum != O.BinaryChecksum " +
          "OR O.OldBinaryChecksum = 0)";

      var connectionString = GetNKConnectionString();

      var inputDataService = new TransformerDataServices(connectionString);

      return inputDataService.ReadData<OrderItemsNK>(sql);
    }


    private FixedList<OrderItemsData> Transform(FixedList<OrderItemsNK> toTransformData) {
      return toTransformData.Select(x => Transform(x))
                            .ToFixedList();
    }


    private OrderItemsData Transform(OrderItemsNK toTransformData) {
      string connectionString = GetEmpiriaConnectionString();
      var dataServices = new TransformerDataServices(connectionString);
      if (toTransformData.OldBinaryChecksum == 0) {
        return new OrderItemsData {
          Order_Item_Id = dataServices.GetNextId("OMS_Order_Items"),
          Order_Item_UID = System.Guid.NewGuid().ToString(),
          Order_Item_Type_Id = 4001,////// de types
          Order_Item_Order_Id = dataServices.GetOrderIdFromOMSOrders(toTransformData.OV),//////ir a oms orders por el id de la orden
          Order_Item_Product_Id = dataServices.GetProductIdFromOMSProducts(toTransformData.Producto), ////////ir a oms_PRODUCTOS POR EL ID del producto
          Order_Item_Description = Empiria.EmpiriaString.BuildKeywords(toTransformData.Producto, toTransformData.Unidad,  toTransformData.Referencia),
          Order_Item_Product_Unit_Id = (int) dataServices.ReturnIdForProductBaseUnitId(toTransformData.Unidad),
          Order_Item_Product_Qty = toTransformData.Cantidad,
          Order_Item_Unit_Price = toTransformData.Precio,
          Order_Item_Discount = toTransformData.Descuento,
          Order_Item_Currency_Id = 600,///// de [SimpleObjects] MXN
          Order_Item_Related_Item_Id = -1,
          Order_Item_Requisition_Item_Id = toTransformData.Det,
          Order_Item_Requested_By_Id = dataServices.GetRequestedUserIdFromOMSOrders(toTransformData.OV),//////ir a oms orders por el id
          Order_Item_Budget_Account_Id = -1,
          Order_Item_Project_Id = -1,
          Order_Item_Provider_Id = (int) dataServices.GetWareHouseIdFromCommonStorage(toTransformData.Almacen),
          Order_Item_Per_Each_Item_Id = -1,
          Order_Item_Ext_Data = "",
          Order_Item_Keywords = Empiria.EmpiriaString.BuildKeywords(toTransformData.OV, toTransformData.Producto),
          Order_Item_Position = toTransformData.Det,
          Order_Item_Posted_By_Id = dataServices.GetPostedUserIdFromOMSOrders(toTransformData.OV),//////ir a oms orders por el id
          Order_Item_Posting_Time = dataServices.GetPostedDateFromOMSOrders(toTransformData.OV), //buscar la fecha
          Order_Item_Status = Convert.ToChar(dataServices.GetOrderItemStatusFromOMSOrders(toTransformData.OV))/////(char) 'A' /////PENDIENTE ir por status a mos orders
        };
      } else {
        return new OrderItemsData {
          Order_Item_Id = dataServices.GetOrderIdFromOMSOrdersItems(toTransformData.OV, toTransformData.Det),
          Order_Item_UID = dataServices.GetOrderUIDFromOMSOrdersItems(toTransformData.OV, toTransformData.Det),
          Order_Item_Type_Id = 4001, ////// de types
          Order_Item_Order_Id = dataServices.GetOrderIdFromOMSOrders(toTransformData.OV),//////ir a oms orders por el id de la orden
          Order_Item_Product_Id = dataServices.GetProductIdFromOMSProducts(toTransformData.Producto), ////////ir a oms_PRODUCTOS POR EL ID del producto
          Order_Item_Description = Empiria.EmpiriaString.BuildKeywords(toTransformData.Producto, toTransformData.Unidad,  toTransformData.Referencia),
          Order_Item_Product_Unit_Id = (int) dataServices.ReturnIdForProductBaseUnitId(toTransformData.Unidad),
          Order_Item_Product_Qty = toTransformData.Cantidad,
          Order_Item_Unit_Price = toTransformData.Precio,
          Order_Item_Discount = toTransformData.Descuento,
          Order_Item_Currency_Id = 600,///// de [SimpleObjects] MXN
          Order_Item_Related_Item_Id = -1,
          Order_Item_Requisition_Item_Id = toTransformData.Det,
          Order_Item_Requested_By_Id = dataServices.GetRequestedUserIdFromOMSOrders(toTransformData.OV),//////ir a oms orders por el id
          Order_Item_Budget_Account_Id = -1,
          Order_Item_Project_Id = -1,
          Order_Item_Provider_Id = (int) dataServices.GetWareHouseIdFromCommonStorage(toTransformData.Almacen),
          Order_Item_Per_Each_Item_Id = -1,
          Order_Item_Ext_Data = "",
          Order_Item_Keywords = Empiria.EmpiriaString.BuildKeywords(toTransformData.OV, toTransformData.Producto),
          Order_Item_Position = toTransformData.Det,
          Order_Item_Posted_By_Id = dataServices.GetPostedUserIdFromOMSOrders(toTransformData.OV),//////ir a oms orders por el id
          Order_Item_Posting_Time = dataServices.GetPostingDateFromOMSOrders(toTransformData.OV),//toTransformData.Fecha_Cierre,
          Order_Item_Status = Convert.ToChar(dataServices.GetOrderItemStatusFromOMSOrders(toTransformData.OV))/////(char) 'A' /////PENDIENTE ir por status a mos orders
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

  }  // class OrderItemsTransformer
  } // namespace Empiria.Trade.Integration.ETL.Transformers

