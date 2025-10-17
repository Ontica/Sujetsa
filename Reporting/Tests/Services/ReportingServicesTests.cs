/* Empiria Trade      ****************************************************************************************
*                                                                                                            *
*  Module   : Empiria.Sujetsa.Rporting.Test              Component : Test Layer                              *
*  Assembly : Empiria.Inventory.Core.dll                 Pattern   : Test                                    *
*  Type     : ReportingServicesTests                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for ReportingServices.                                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/


using Empiria.Inventory.UseCases;
using Empiria.Sujetsa.Reporting;

using Xunit;


namespace Empiria.Tests.Sujetsa {

  /// <summary>Unit tests for ReportingServices.</summary>
  public class ReportingServicesTests {

    #region Initialization

    public ReportingServicesTests() {
      TestsCommonMethods.Authenticate();
    }

    #endregion Initialization

    [Fact]
    public void GetInventoryEntryReport() {

      var service = InventoryEntryReportingService.ServiceInteractor();
      var usecase = InventoryEntryUseCases.UseCaseInteractor();

      string inventoryOrderUID = "a33c76c7-c266-43ff-bfb2-2b2b820b312a";

      var entryReports = usecase.GetInventoryEntryReport(inventoryOrderUID);

      var sut = service.ExportInventoryEntryReportToExcel(entryReports);

      Assert.NotNull(sut);
    }


  } // class Empiria.Tests.Sujetsa

} // namespace ReportingServicesTests
