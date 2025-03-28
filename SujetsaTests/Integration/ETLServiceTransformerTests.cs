/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Tests Layer                           *
*  Assembly : Empiria.Trade.Integration.Tests              Pattern   : Unit tests                            *
*  Type     : ETLServiceTransformerTests                                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Unit tests for ETLService & Transformers.                                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Trade.Integration.ETL;
using Empiria.Trade.Integration.ETL.Data;
using Empiria.Trade.Integration.ETL.Transformers;
using Empiria.Json;

namespace Empiria.Tests.Trade.Integration {

  /// <summary>Unit tests for ETLService Transformer.</summary>
  public class ETLServiceTransformerTests {

    #region Facts

    [Fact]
    public void Should_Execute_ETL_Service() {

      var service = new ETLService();

      service.Execute();

      Assert.NotNull(service);
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

    #endregion Helpers

  }  // class ETLServiceTests

}  // namespace Empiria.Tests.Trade.Integration
