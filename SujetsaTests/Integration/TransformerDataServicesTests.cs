/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Tests Layer                           *
*  Assembly : Empiria.Trade.Integration.Tests              Pattern   : Unit tests                            *
*  Type     : TransformerDataServicesTests                 License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Unit tests for TransformerDataServicesTests.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Json;

using Empiria.Trade.Integration.ETL.Data;
using Empiria.Trade.Integration.ETL.Transformers;
using System;

namespace Empiria.Tests.Trade.Integration {

  /// <summary>Unit tests for TransformerDataServicesTests.</summary>
  public class TransformerDataServicesTests {

    #region Facts

    [Fact]
    public void Should_Get_Category_Id_From_CommonStorage() {
      string connectionString = GetEmpiriaConnectionString();

      string grupo = "1";
      string subgrupo = "1";

      var dataServices = new TransformerDataServices(connectionString);
      var sut = dataServices.GetCategoryIdFromCommonStorage(grupo,subgrupo);

      Assert.NotNull(sut);
      Assert.True(sut > 0);
      Assert.Equal(39, sut);
    }

    
    [Fact]
    public void Should_Get_Closed_Date_From_OvUbicacion_Consecutivo() {
      string connectionString =  GetNKConnectionString();

      string OV = "OV00145255";

      var dataServices = new TransformerDataServices(connectionString);
      var sut = dataServices.GetClosedDateFromOvUbicacionConsecutivo(OV);

      DateTime expectedDate = new DateTime(2025, 1, 6, 11, 52, 38);

      Assert.Equal(expectedDate, sut);
    }


    [Fact]
    public void Should_Get_Closed_Id_From_OvUbicacion_Consecutivo() {
      string connectionString = GetNKConnectionString();

      string OV = "OV00145255";

      var dataServices = new TransformerDataServices(connectionString);
      var sut = dataServices.GetClosedIdFromOvUbicacionConsecutivo(OV);

      Assert.Equal(04, sut);
    }


    [Fact]
    public void Should_Get_Object_Tags_From_CommonStorage() {
      string connectionString = GetEmpiriaConnectionString();

      string grupo = "1";
      string subgrupo = "1";

      var dataServices = new TransformerDataServices(connectionString);
      var sut = dataServices.GetObjectTagsFromCommonStorage(grupo, subgrupo);

      Assert.NotNull(sut);
      Assert.True(sut.Length > 0);
      Assert.Equal("ABRAZADERA OMEGA", sut);
    }


    [Fact]
    public void Should_Get_Order_Id_From_OMSOrders() {
      string connectionString = GetEmpiriaConnectionString();

      string orden = "OV00132541";

      var dataServices = new TransformerDataServices(connectionString);
      var sut = dataServices.GetOrderIdFromOMSOrders(orden);

      Assert.True(sut > 0);
    }


    [Fact]
    public void Should_Get_Order_UID_From_OMSOrders() {
      string connectionString = GetEmpiriaConnectionString();

      string orden = "OV00132541";

      var dataServices = new TransformerDataServices(connectionString);
      var sut = dataServices.GetOrderUIDFromOMSOrders(orden);

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Get_Order_Item_Provider_Id_From_OMSOrders() {
      string connectionString = GetEmpiriaConnectionString();

      string factura = "A0069214";

      var dataServices = new TransformerDataServices(connectionString);
      var sut = dataServices.GetOrderItemProviderIdFromOMSOrders(factura);

      Assert.Equal(146,sut);
    }


    [Fact]
    public void Should_Get_Party_Id_From_Parties() {
      string connectionString = GetEmpiriaConnectionString();

      string identificator = "ETIQUETAS";

      var dataServices = new TransformerDataServices(connectionString);
      var sut = dataServices.GetPartyIdFromParties(identificator, "User");

      Assert.True(sut > 0);
      Assert.Equal(1, sut);
    }


    [Fact]
    public void Should_Get_Party_UID_From_Parties() {
      string connectionString = GetEmpiriaConnectionString();

      string cliente = "Administrador";

      var dataServices = new TransformerDataServices(connectionString);
      var sut = dataServices.GetPartyUIDFromParties(cliente);

      Assert.NotNull(sut);
      Assert.Equal("d5dc4e55-b935-4459-8fa5-22ce95f65f56", sut);
    }


    [Fact]
    public void Should_Get_Posted_User_Id_From_OMSOrders() {
      string connectionString = GetEmpiriaConnectionString();

      string factura = "A0069214";

      var dataServices = new TransformerDataServices(connectionString);
      var sut = dataServices.GetPostedUserIdFromOMSOrders(factura);

      Assert.Equal(1673, sut);
    }


    [Fact]
    public void Should_Get_Posted_Date_From_OMSOrders() {
      string connectionString = GetEmpiriaConnectionString();

      string factura = "A0069214";

      var dataServices = new TransformerDataServices(connectionString);
      var sut = dataServices.GetPostedDateFromOMSOrders(factura);

      DateTime expectedDate = new DateTime(2025, 1, 6, 0, 0, 0);

      Assert.Equal(expectedDate, sut);
    }


    [Fact]
    public void Should_Get_Product_Id_From_OMSProducts() {
      string connectionString = GetEmpiriaConnectionString();

      string productName = "AOME2";

      var dataServices = new TransformerDataServices(connectionString);
      var sut = dataServices.GetProductIdFromOMSProducts(productName);

      Assert.True(sut > 0);
      Assert.Equal(5, sut);
    }


    [Fact]
    public void Should_Get_Product_UID_From_OMSProducts() {
      string connectionString = GetEmpiriaConnectionString();

      string productName = "AOME2";

      var dataServices = new TransformerDataServices(connectionString);
      var sut = dataServices.GetProductUIDFromOMSProducts(productName);

      Assert.NotNull(sut);
      Assert.Equal("e1cffa3b-b597-48ea-b379-64836dcf7fb4", sut);
    }


    [Fact]
    public void Should_Get_Order_Item_Status_From_OMSOrders() {
      string connectionString = GetEmpiriaConnectionString();

      string factura = "A0069214";

      var dataServices = new TransformerDataServices(connectionString);
      var sut = dataServices.GetOrderItemStatusFromOMSOrders(factura);

      Assert.Equal("Y", sut);
    }


    [Fact]
    public void Should_Get_Order_Id_From_OMS_Order_Items() {
      string connectionString = GetEmpiriaConnectionString();

      int id = 2;
      int det = 2;

      var dataServices = new TransformerDataServices(connectionString);
      var sut = dataServices.GetOrderIdFromOMSOrderItems(id,det);

      Assert.Equal(3, sut);
    }


    [Fact]
    public void Should_Get_Order_UID_From_OMS_Order_Items() {
      string connectionString = GetEmpiriaConnectionString();

      int id = 2;
      int det = 2;

      var dataServices = new TransformerDataServices(connectionString);
      var sut = dataServices.GetOrderUIDFromOMSOrderItems(id, det);

      Assert.Equal("eeace2a6-bf06-4031-b4ee-decfdeb9402a", sut);
    }

    [Fact]
    public void Should_Get_Requested_User_Id_From_OMSOrders() {
      string connectionString = GetEmpiriaConnectionString();

      string factura = "A0069215";

      var dataServices = new TransformerDataServices(connectionString);
      var sut = dataServices.GetRequestedUserIdFromOMSOrders(factura);

      Assert.Equal(843, sut);
    }


    [Fact]
    public void Should_Get_WareHouse_Id_From_CommonStorage() {
      string connectionString = GetEmpiriaConnectionString();

      string almacen = "AG";

      var dataServices = new TransformerDataServices(connectionString);
      var sut = dataServices.GetWareHouseIdFromCommonStorage(almacen);

      Assert.True(sut > 2145);
      Assert.Equal(2146, sut);
    }

    [Fact]
    public void Should_Contact_Transformer_Execute() {


      string tableName = "DBO.Contacts";

      string connectionString = GetEmpiriaConnectionString();

      var contactTransformer = new ContactTransformer(connectionString);

      contactTransformer.Execute();

      var sut = new TransformerDataServices(connectionString);

      int rowCount = sut.RowCounter(tableName);

      Assert.True(rowCount > 0);
    }


    [Fact]
    public void Should_Order_Transformer_Execute() {


      string tableName = "DBO.OMS_Orders";

      string connectionString = GetEmpiriaConnectionString();

      var orderTransformer = new OrderTransformer(connectionString);

      orderTransformer.Execute();

      var sut = new TransformerDataServices(connectionString);

      int rowCount = sut.RowCounter(tableName);

      Assert.True(rowCount >= 0);
    }


    [Fact]
    public void Should_Order_Invoice_Transformer_Execute() {


      string tableName = "DBO.OMS_Orders";

      string connectionString = GetEmpiriaConnectionString();

      var orderInvoiceTransformer = new OrderInvoiceTransformer(connectionString);

      orderInvoiceTransformer.Execute();

      var sut = new TransformerDataServices(connectionString);

      int rowCount = sut.RowCounter(tableName);

      Assert.True(rowCount >= 0);
    }


    [Fact]
    public void Should_Order_Items_InvoiceTransformer_Execute() {


      string tableName = "DBO.OMS_Order_Items";

      string connectionString = GetEmpiriaConnectionString();

      var orderItemsInvoiceTransformer = new OrderItemsInvoiceTransformer(connectionString);

      orderItemsInvoiceTransformer.Execute();

      var sut = new TransformerDataServices(connectionString);

      int rowCount = sut.RowCounter(tableName);

      Assert.True(rowCount >= 0);
    }


    [Fact]
    public void Should_Order_Items_Transformer_Execute() {


      string tableName = "DBO.OMS_Order_Items";

      string connectionString = GetEmpiriaConnectionString();

      var orderItemsTransformer = new OrderItemsTransformer(connectionString);

      orderItemsTransformer.Execute();

      var sut = new TransformerDataServices(connectionString);

      int rowCount = sut.RowCounter(tableName);

      Assert.True(rowCount >= 0);
    }


    [Fact]
    public void Should_Party_Transformer_Execute() {


      string tableName = "DBO.Parties";

      string connectionString = GetEmpiriaConnectionString();

      var partyTransformer = new PartyTransformer(connectionString);

      partyTransformer.Execute();

      var sut = new TransformerDataServices(connectionString);

      int rowCount = sut.RowCounter(tableName);

      Assert.True(rowCount > 0);
    }


    [Fact]
    public void Should_Product_Transformer_Execute() {
    

      string tableName = "DBO.OMS_Products";

      string connectionString = GetEmpiriaConnectionString();

      var productTransformer = new ProductTransformer(connectionString);

      productTransformer.Execute();

      var sut = new TransformerDataServices(connectionString);

      int rowCount = sut.RowCounter(tableName);

      Assert.True(rowCount >= 0);
    }


    [Fact]
    public void Should_Read_Data() {
      string connectionString = GetNKConnectionString();

      var con = new TransformerDataServices(connectionString);

      var sql = "SELECT * FROM sources.PRODUCTO_TARGET";

      FixedList<ProductNK> sut = con.ReadData<ProductNK>(sql);

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Return_Id_For_Priority() {
      string prioridad = "1";
      string connectionString = GetNKConnectionString();

      var dataServices = new TransformerDataServices(connectionString);
      var sut = dataServices.ReturnIdForPriority(prioridad);

      Assert.Equal('U', sut);
    }


    [Fact]
    public void Should_Return_Id_For_Product_Base_Unit_Id() {
      string prioridad = "H87";
      string connectionString = GetNKConnectionString();

      var dataServices = new TransformerDataServices(connectionString);
      var sut = dataServices.ReturnIdForProductBaseUnitId(prioridad);

      Assert.Equal(1110, sut);
    }


    [Fact]
    public void Should_Return_Old_Description_For_Priority() {
      string prioridad = "1";
      string connectionString = GetNKConnectionString();

      var dataServices = new TransformerDataServices(connectionString);
      var sut = dataServices.ReturnOldDescriptionForPriority(prioridad);

      Assert.Equal("OCURRE", sut);
    }


    [Fact]
    public void Should_Return_Status_For_Party_Status() {
      string prioridad = "S";
      string connectionString = GetNKConnectionString();

      var dataServices = new TransformerDataServices(connectionString);
      var sut = dataServices.ReturnStatusForPartyStatus(prioridad);

      Assert.Equal('A', sut);
    }


    [Fact]
    public void Should_Return_Valid_Row_Count() {
      string tableName = "sources.PRODUCTO_TARGET";

      string connectionString = GetNKConnectionString();

      var sut = new TransformerDataServices(connectionString);

      int rowCount = sut.RowCounter(tableName);

      Assert.True(rowCount >= 0);
    }

    #endregion Facts

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

  }  // class TransformerDataServicesTests

}  // namespace Empiria.Tests.Trade.Integration

