using Xunit;

namespace Empiria.Tests.Trade.Integration {

  public class ETLServiceTransformerSequenceTests {

    [Fact]
    public void Should_Execute_All_Transformers_In_Sequence() {
      ExecuteAllTransformersInSequence();
    }


    private void ExecuteAllTransformersInSequence() {
      var tests = new ETLServiceTransformerTests();
      tests.Should_Execute_ETL_Service();
      tests.Should_Product_Transformer_Execute();
      tests.Should_Party_Transformer_Execute();
      tests.Should_Contact_Transformer_Execute();

      tests.Should_Order_Invoice_Transformer_Execute();
      tests.Should_Order_Credit_Note_Transformer_Execute();
      tests.Should_Order_Purchase_Transformer_Execute();
      tests.Should_Order_Rem_Transformer_Execute();

      tests.Should_Order_Items_Credit_Note_Transformer_Execute();
      tests.Should_Order_Items_Purchase_Transformer_Execute();
      tests.Should_Order_Items_Rem_Transformer_Execute();
      tests.Should_Order_Items_Invoice_Transformer_Execute();
    }
  }
}
