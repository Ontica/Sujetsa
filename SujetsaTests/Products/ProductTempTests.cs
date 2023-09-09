using System;

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

      var service = new TradeDataSchemaManager.Services.Services(true);
      var dt = service.GetDataCountFromDb();

      Assert.NotNull(dt);

    }

    #endregion Facts

  }
}
