
using System.Collections.Generic;
using System.Threading.Tasks;

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

      var service = new SchemaServices(true);

      var dt = service.GetDataCountFromDb();

      Assert.NotNull(dt);

    }


    [Fact]
    public void GetProductListFromFbTest() {

      var service = new SchemaServices(true);

      List<ProductosAdapter> list = service.GetDataFromDb();

      Assert.NotNull(list);

    }


    [Fact]
    public void InsertListFromFbToSqlTest() {

      var service = new SchemaServices(true);

      List<ProductosAdapter> productsToUpdate = service.GetDataFromDb();

      string message = service.InsertProductToSql(productsToUpdate);

      Assert.NotNull(message);

    }


    [Fact]
    public async Task InsertProductToSqlAsyncTest() {

      var service = new SchemaServices(true);

      List<ProductosAdapter> productsToUpdate = service.GetDataFromDb();

      string message = await service.InsertProductToSqlAsync(productsToUpdate)
                                    .ConfigureAwait(false);

      Assert.NotNull(message);

    }


    [Fact]
    public void GetListFromSqlTest() {

      var service = new SchemaServices(true);
      var list = service.GetListFromSql();

      Assert.NotNull(list);

    }

    #endregion Facts

  }
}
