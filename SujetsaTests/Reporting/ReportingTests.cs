
using System;
using Xunit;
using Empiria.Storage;
using Empiria.Trade.Core;
using Empiria.Trade.Procurement.UseCases;
using Empiria.Sujetsa.Reporting;


namespace Empiria.Trade.Tests.Reporting {

  /// <summary></summary>
  public class ReportingTests {

    #region Initialization

    public ReportingTests() {
      // TestsCommonMethods.Authenticate();
    }

    #endregion Initialization

    #region Facts

    [Fact]
    public void ExportPurchaseOrderItemsTest() {

      using (var usecases = PurchaseOrderUseCases.UseCaseInteractor()) {

        string orderUID = "3aefae94-50bd-4a54-a58a-128b9a479945";

        IOrderDto reportentries = usecases.GetPurchaseOrderDto(orderUID);

        var exporterService = new OrdersReportingService();

        FileDto excelFileDto = exporterService.Export(reportentries);

        Assert.NotNull(excelFileDto);
      }
    }

    #endregion Facts

  }
}
