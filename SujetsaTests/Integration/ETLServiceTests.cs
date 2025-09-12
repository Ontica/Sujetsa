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

      //var tests = new ETLServiceTransformerTests();
      //tests.Should_Execute_ETL_Service();
      //tests.Should_Product_Transformer_Execute();
      //tests.Should_Party_Transformer_Execute();
      //tests.Should_Contact_Transformer_Execute();

      //tests.Should_Order_Invoice_Transformer_Execute();
      //tests.Should_Order_Credit_Note_Transformer_Execute();
      //tests.Should_Order_Purchase_Transformer_Execute();
      //tests.Should_Order_Rem_Transformer_Execute();

      //tests.Should_Order_Items_Credit_Note_Transformer_Execute();
      //tests.Should_Order_Items_Purchase_Transformer_Execute();
      //tests.Should_Order_Items_Rem_Transformer_Execute();
      //tests.Should_Order_Items_Invoice_Transformer_Execute();
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
