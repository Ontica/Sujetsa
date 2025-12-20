/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Services Layer                        *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Service provider                      *
*  Type     : OrderItemsPurchaseTransformer                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Transforms a OrderItem(CompraDet) from NK to Empiria Trade.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.Linq;
using Empiria.Data;
using Empiria.Json;
using Empiria.Trade.Integration.ETL.Data;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>Transforms a order item(CompraDet) from NK to Empiria Trade.</summary>
  public class OrderItemsPurchaseTransformer {

    private readonly string _connectionString;
    private readonly string _empiriaConnectionString;
    private readonly string _nkConnectionString;

    internal OrderItemsPurchaseTransformer(string connectionString) {
      Assertion.Require(connectionString, nameof(connectionString));
      _connectionString = connectionString;
      _empiriaConnectionString = GetEmpiriaConnectionString();
      _nkConnectionString = GetNKConnectionString();
    }

    public void Execute() {
      var sourceData = ReadSourceData();
      var transformedData = Transform(sourceData);
      WriteTargetData(transformedData);
    }

    public FixedList<OrderItemsPurchaseNK> ReadSourceData() {
      const string sql = @"
        SELECT O.COMPRA, O.DET, O.PRODUCTO, O.CLAVE, O.CANTIDAD, O.UNIDAD, 
               O.PRECIO, O.COSTO, O.DESCUENTOS, O.SUBTOTAL, O.IVA, O.TOTAL, 
               O.BinaryChecksum, O.OldBinaryChecksum
        FROM sources.COMPRADET_TARGET O
        INNER JOIN sources.COMPRA_TARGET V ON V.COMPRA = O.COMPRA
        WHERE V.FECHA >= '2025-12-01'
          AND (O.OldBinaryChecksum != O.BinaryChecksum OR O.OldBinaryChecksum = 0)";

      var inputDataService = new TransformerDataServices(_nkConnectionString);
      return inputDataService.ReadData<OrderItemsPurchaseNK>(sql);
    }

    public FixedList<OrderItemsData> Transform(FixedList<OrderItemsPurchaseNK> toTransformData) {
      if (toTransformData.Count == 0) {
        return new FixedList<OrderItemsData>();
      }

      var dataServices = new TransformerDataServices(_empiriaConnectionString);

      // Pre-cargar datos en lotes
      var compras = toTransformData.Select(x => x.Compra).Distinct().ToList();
      var productos = toTransformData.Select(x => x.Producto).Distinct().ToList();
      var unidades = toTransformData.Select(x => x.Unidad).Distinct().ToList();

      var orderCache = PreloadOrderData(dataServices, compras);
      var productCache = PreloadProductData(dataServices, productos);
      var unitCache = PreloadUnitData(dataServices, unidades);

      return toTransformData.Select(x => Transform(x, dataServices, orderCache, productCache, unitCache))
                            .ToFixedList();
    }

    private Dictionary<string, OrderCacheData> PreloadOrderData(TransformerDataServices dataServices, List<string> compras) {
      var cache = new Dictionary<string, OrderCacheData>();
      foreach (var compra in compras) {
        cache[compra] = new OrderCacheData {
          OrderId = dataServices.GetOrderIdFromOMSOrders(compra),
          RequestedUserId = dataServices.GetRequestedUserIdFromOMSOrders(compra),
          ProviderId = dataServices.GetOrderItemProviderIdFromOMSOrders(compra),
          PostedUserId = dataServices.GetPostedUserIdFromOMSOrders(compra),
          PostingTime = dataServices.GetPostedDateFromOMSOrders(compra),
          Status = dataServices.GetOrderItemStatusFromOMSOrders(compra)
        };
      }
      return cache;
    }

    private Dictionary<string, int> PreloadProductData(TransformerDataServices dataServices, List<string> productos) {
      var cache = new Dictionary<string, int>();
      foreach (var producto in productos) {
        cache[producto] = dataServices.GetProductIdFromOMSProducts(producto);
      }
      return cache;
    }

    private Dictionary<string, int> PreloadUnitData(TransformerDataServices dataServices, List<string> unidades) {
      var cache = new Dictionary<string, int>();
      foreach (var unidad in unidades) {
        cache[unidad] = (int) dataServices.ReturnIdForProductBaseUnitId(unidad);
      }
      return cache;
    }

    private OrderItemsData Transform(OrderItemsPurchaseNK source,
                                     TransformerDataServices dataServices,
                                     Dictionary<string, OrderCacheData> orderCache,
                                     Dictionary<string, int> productCache,
                                     Dictionary<string, int> unitCache) {

      var orderData = orderCache[source.Compra];
      var keywords = EmpiriaString.BuildKeywords(source.Compra, source.Producto, source.Clave);
      var isNewItem = source.OldBinaryChecksum == 0;

      // Cálculo de descuento (lógica específica de Purchase)
      var discount = string.IsNullOrEmpty(source.Descuentos) ? 0m : Convert.ToDecimal(source.Descuentos);

      // JSON constante pre-serializado
      const string extData = "{\"Name\":\"Compra\"}";

      return new OrderItemsData {
        Order_Item_Id = isNewItem
          ? IdGenerator.GetNextId("OMS_Order_Items")
          : dataServices.GetOrderIdFromOMSOrderItems(orderData.OrderId, source.Det),
        Order_Item_UID = isNewItem
          ? Guid.NewGuid().ToString()
          : dataServices.GetOrderUIDFromOMSOrderItems(orderData.OrderId, source.Det),
        Order_Item_Type_Id = 4054,
        Order_Item_Order_Id = orderData.OrderId,
        Order_Item_Requisition_Id = -1,
        Order_Item_Contract_Id = -1,
        Order_Item_Requisition_Item_Id = -1,
        Order_Item_Contract_Item_Id = -1,
        Order_Item_Related_Item_Id = -1,
        Order_Item_SKU_Id = -1,
        Order_Item_Product_Id = productCache[source.Producto],
        Order_Item_Product_Code = "",
        Order_Item_Product_Name = "",
        Order_Item_Description = keywords,
        Order_Item_Justification = "",
        Order_Item_Product_Unit_Id = unitCache[source.Unidad],
        Order_Item_Requested_Qty = -1,
        Order_Item_Min_Qty = -1,
        Order_Item_Max_Qty = -1,
        Order_Item_Qty = source.Cantidad,
        Order_Item_Start_Date = ExecutionServer.DateMinValue,
        Order_Item_End_Date = ExecutionServer.DateMaxValue,
        Order_Item_Currency_Id = 600,
        Order_Item_Unit_Price = source.Precio,
        Order_Item_Discount = discount,
        Order_Item_Price_Id = -1,
        Order_Item_Project_Id = -1,
        Order_Item_Budget_Id = -1,
        Order_Item_Budget_Account_Id = -1,
        Order_Item_Budget_Entry_Id = -1,
        Order_Item_Geo_Origin_Id = -1,
        Order_Item_Location_Id = -1,
        Order_Item_Config_Ext_Data = "",
        Order_Item_Conditions_Ext_Data = "",
        Order_Item_Spec_Ext_Data = "",
        Order_Item_Ext_Data = extData,
        Order_Item_Keywords = keywords,
        Order_Item_Requested_By_Id = orderData.RequestedUserId,
        Order_Item_Requested_Time = ExecutionServer.DateMinValue,
        Order_Item_Required_Time = ExecutionServer.DateMaxValue,
        Order_Item_Responsible_Id = -1,
        Order_Item_Beneficiary_Id = -1,
        Order_Item_Provider_Id = (int) orderData.ProviderId,
        Order_Item_Received_By_Id = -1,
        Order_Item_Closing_Time = ExecutionServer.DateMaxValue,
        Order_Item_Closed_By_Id = -1,
        Order_Item_Position = source.Det,
        Order_Item_Posting_Time = orderData.PostingTime,
        Order_Item_Posted_By_Id = orderData.PostedUserId,
        Order_Item_Status = Convert.ToChar(orderData.Status)
      };
    }

    public void WriteTargetData(FixedList<OrderItemsData> transformedData) {
      if (transformedData.Count == 0)
        return;

      foreach (var item in transformedData) {
        WriteTargetData(item);
      }
    }

    private void WriteTargetData(OrderItemsData o) {
      var op = DataOperation.Parse("write_OMS_Order_Item", o.Order_Item_Id, o.Order_Item_UID, o.Order_Item_Type_Id, o.Order_Item_Order_Id, o.Order_Item_Requisition_Id, o.Order_Item_Contract_Id,
      o.Order_Item_Requisition_Item_Id, o.Order_Item_Contract_Item_Id, o.Order_Item_Related_Item_Id, o.Order_Item_SKU_Id, o.Order_Item_Product_Id,
      o.Order_Item_Product_Code, o.Order_Item_Product_Name, o.Order_Item_Description, o.Order_Item_Justification, o.Order_Item_Product_Unit_Id,
      o.Order_Item_Requested_Qty, o.Order_Item_Min_Qty, o.Order_Item_Max_Qty, o.Order_Item_Qty, o.Order_Item_Start_Date, o.Order_Item_End_Date,
      o.Order_Item_Currency_Id, o.Order_Item_Unit_Price, o.Order_Item_Discount, o.Order_Item_Price_Id, o.Order_Item_Project_Id, o.Order_Item_Budget_Id,
      o.Order_Item_Budget_Account_Id, o.Order_Item_Budget_Entry_Id, o.Order_Item_Geo_Origin_Id, o.Order_Item_Location_Id, o.Order_Item_Config_Ext_Data,
      o.Order_Item_Conditions_Ext_Data, o.Order_Item_Spec_Ext_Data, o.Order_Item_Ext_Data, o.Order_Item_Keywords, o.Order_Item_Requested_By_Id, o.Order_Item_Requested_Time,
      o.Order_Item_Required_Time, o.Order_Item_Responsible_Id, o.Order_Item_Beneficiary_Id, o.Order_Item_Provider_Id, o.Order_Item_Received_By_Id,
      o.Order_Item_Closing_Time, o.Order_Item_Closed_By_Id, o.Order_Item_Position, o.Order_Item_Posting_Time, o.Order_Item_Posted_By_Id, o.Order_Item_Status);

      DataWriter.Execute(op);
    }

    #region Helpers

    private static string GetEmpiriaConnectionString() {
      var config = ConfigurationData.Get<JsonObject>("Connection.Strings");
      return config.Get<string>("empiriaSqlServerConnection");
    }

    private static string GetNKConnectionString() {
      var config = ConfigurationData.Get<JsonObject>("Connection.Strings");
      return config.Get<string>("sqlServerConnection");
    }

    #endregion

    #region Cache Classes

    private class OrderCacheData {
      public int OrderId {
        get; set;
      }
      public int RequestedUserId {
        get; set;
      }
      public long ProviderId {
        get; set;
      }
      public int PostedUserId {
        get; set;
      }
      public DateTime PostingTime {
        get; set;
      }
      public string Status {
        get; set;
      }
    }

    #endregion
  }
}