/* Empiria Operations ****************************************************************************************
*                                                                                                            *
*  Module   : Reporting Services                            Component : Service Layer                        *
*  Assembly : Empiria.Sujetsa.Reporting.Core.dll            Pattern   : Service provider                     *
*  Type     : InventoryEntryReportingService                License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Provides services used to generate Inventory entries reports.                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Inventory.Adapters;

using Empiria.Office;
using Empiria.Services;
using Empiria.Storage;

namespace Empiria.Sujetsa.Reporting {

  /// <summary>Provides services used to generate Inventory entries reports.</summary>
  public class InventoryEntryReportingService : Service {

    #region Constructors and parsers

    private InventoryEntryReportingService() {
      // no-op
    }

    static public InventoryEntryReportingService ServiceInteractor() {
      return Service.CreateInstance<InventoryEntryReportingService>();
    }

    #endregion Constructors and parsers

    #region Services

    public FileDto ExportInventoryEntryReportToExcel(FixedList<InventoryEntryReportDto> entries) {
      Assertion.Require(entries, nameof(entries));

      var templateUID = $"{this.GetType().Name}.ExportInventoryEntryReportToExcel";

      var templateConfig = FileTemplateConfig.Parse(templateUID);

      var exporter = new InventoryEntryToExcelBuilder(templateConfig);

      ExcelFile excelFile = exporter.CreateExcelFile(entries);

      return excelFile.ToFileDto();
    }


    #endregion Services

  } // class InventoryEntryReportingService

} // namespace Empiria.Sujetsa.Reporting
