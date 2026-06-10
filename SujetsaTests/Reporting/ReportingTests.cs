
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

        string orderUID = "858aeae2-8989-4401-9780-783aab9c9744";

        IOrderDto reportentries = usecases.GetPurchaseOrderDto(orderUID);

        var exporterService = new OrdersReportingService();

        FileDto excelFileDto = exporterService.Export(reportentries);

        Assert.NotNull(excelFileDto);
      }
    }

    #endregion Facts

  }
}
