
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
    public void ExportSaldosEncerradosTest() {

      using (var usecases = PurchaseOrderUseCases.UseCaseInteractor()) {

        string orderUID = "62322440-38c3-46d4-9593-b31c98455389";

        IOrderDto reportentries = usecases.GetPurchaseOrderDto(orderUID);

        var exporterService = new OrdersReportingService();

        FileDto excelFileDto = exporterService.Export(reportentries);

        Assert.NotNull(excelFileDto);
      }
    }

    #endregion Facts

  }
}
