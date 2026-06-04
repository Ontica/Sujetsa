/* Empiria Sujetsa.Trade *************************************************************************************
*                                                                                                            *
*  Module   : Reporting Services                            Component : Service Layer                        *
*  Assembly : Empiria.Sujetsa.Reporting.Core.dll            Pattern   : Service provider                     *
*  Type     : OrdersReportingService                        License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Provides services used to generate order entries reports.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using Empiria.Inventory.Adapters;
using Empiria.Office;
using Empiria.Services;
using Empiria.Storage;

namespace Empiria.Sujetsa.Reporting {

  /// <summary>Provides services used to generate order entries reports.</summary>
  public class OrdersReportingService : Service {

    #region Constructors and parsers

    private OrdersReportingService() {
      // no-op
    }

    static public OrdersReportingService ServiceInteractor() {
      return Service.CreateInstance<OrdersReportingService>();
    }

    #endregion Constructors and parsers


    #region Services

    public FileDto Export(FixedList<InventoryEntryReportDto> entries) {
      Assertion.Require(entries, nameof(entries));

      var templateUID = $"{this.GetType().Name}.PurchaseOrder";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var exporter = new InventoryEntryToExcelBuilder(templateConfig);

      ExcelFile excelFile = exporter.CreateExcelFile(entries);

      return excelFile.ToFileDto();
    }

    #endregion Services

  } // class OrdersReportingService

} // namespace Empiria.Sujetsa.Reporting
