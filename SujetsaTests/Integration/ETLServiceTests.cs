/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Tests Layer                           *
*  Assembly : Empiria.Trade.Integration.Tests              Pattern   : Unit tests                            *
*  Type     : ETLService                                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Unit tests for ETLService.                                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Trade.Integration.ETL;

namespace Empiria.Tests.Trade.Integration {

  /// <summary>Unit tests for ETLService.</summary>
  public class ETLServiceTests {

    #region Facts

    [Fact]
    public void Should_Execute() {

      var service = new ETLService();

      service.Execute();

      Assert.NotNull(service);
    }


    [Fact]
    public async void Should_ExecuteAll() {
      var sut = new ETLService();

      await sut.ExecuteAll();

    }


    [Fact]
    public void Should_ExecuteReverseETL() {

      var service = new ETLService();

      var Order_No = "INV-LHATKP4I";

      service.ExecuteReverseETL(Order_No);

      Assert.NotNull(service);
    }

    #endregion Facts

  }  // class ETLServiceTests

}  // namespace Empiria.Tests.Trade.Integration
