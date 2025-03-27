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
    public void Should_Get_Party_Id_From_Parties() {
      string connectionString = GetEmpiriaConnectionString();

      string cliente = "Administrador";

      var dataServices = new TransformerDataServices(connectionString);
      var sut = dataServices.GetPartyIdFromParties(cliente);

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
    public void Should_Read_Data() {
      string connectionString = GetNKConnectionString();

      var con = new TransformerDataServices(connectionString);

      var sql = "SELECT * FROM sources.PRODUCTO_TARGET";

      FixedList<ProductNK> sut = con.ReadData<ProductNK>(sql);

      Assert.NotNull(sut);
    }


    [Fact]
    public void Should_Return_Valid_Row_Count() {
      string tableName = "sources.PRODUCTO_TARGET";

      string connectionString = GetNKConnectionString();

      var sut = new TransformerDataServices(connectionString);

      int rowCount = sut.RowCounter(tableName);

      Assert.True(rowCount >= 0);
    }


    [Fact]
    public void Should_Get_WareHouse_Id_From_CommonStorage() {
      string connectionString = GetEmpiriaConnectionString();

      string almacen = "AG";

      var dataServices = new TransformerDataServices(connectionString);
      var sut = dataServices.GetWareHouseIdFromCommonStorage(almacen);

      Assert.True(sut > 145);
      Assert.Equal(146, sut);
    }


    [Fact]
    public void Should_Party_Transformer_Execute() {


      string tableName = "DBO.OMS_Parties";

      string connectionString = GetEmpiriaConnectionString();

      var partyTransformer = new PartyTransformer(connectionString);

      partyTransformer.Execute();

      var sut = new TransformerDataServices(connectionString);

      int rowCount = sut.RowCounter(tableName);

      Assert.True(rowCount >= 0);
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

