using System;
using System.Collections.Generic;
using TradeDataSchemaManager.Adapters;
using TradeDataSchemaManager.Services;
using Xunit;

namespace Empiria.Trade.Tests.Products {

  /// <summary></summary>
  public class ProductTempTests {

    #region Initialization

    public ProductTempTests() {
      // TestsCommonMethods.Authenticate();
    }

    #endregion Initialization


    #region Facts


    [Fact]
    public void GetDataCountFromDbTest() {

      var service = new Services(true);
      var dt = service.GetDataCountFromDb();

      Assert.NotNull(dt);

    }


    [Fact]
    public void GetProductListFromFbTest() {

      var service = new Services(true);
      List<ProductosAdapter> list = service.GetDataFromDb();

      Assert.NotNull(list);

    }


    [Fact]
    public void InsertListFromFbToSqlTest() {

      var service = new Services(true);

      List<ProductosAdapter> productsToUpdate = service.GetDataFromDb();

      string message = service.InsertProductToSql(productsToUpdate);

      Assert.NotNull(message);

    }

    #endregion Facts

  }
}
