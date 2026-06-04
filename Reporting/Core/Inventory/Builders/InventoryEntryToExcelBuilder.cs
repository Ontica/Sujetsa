/* Empiria Trade      ****************************************************************************************
*                                                                                                            *
*  Module   : Reporting Services                            Component : Service Layer                        *
*  Assembly : Empiria.Sujetsa.Reporting.Core.dll            Pattern   : Report builder                       *
*  Type     : InventoryEntryToExcelBuilder                  License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds an Excel file with inventory entries information.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Office;
using Empiria.Storage;

using Empiria.Inventory.Adapters;

namespace Empiria.Sujetsa.Reporting {

  ///<summary>Builds an Excel file with inventory entries information.</summary>
  internal class InventoryEntryToExcelBuilder {

    private readonly FileTemplateConfig _templateConfig;

    private ExcelFile _excelFile;


    public InventoryEntryToExcelBuilder(FileTemplateConfig templateConfig) {
      Assertion.Require(templateConfig, nameof(templateConfig));

      _templateConfig = templateConfig;
    }


    internal ExcelFile CreateExcelFile(FixedList<InventoryEntryReportDto> entries) {
      Assertion.Require(entries, nameof(entries));

      _excelFile = new ExcelFile(_templateConfig);

      _excelFile.Open();

      SetHeader();

      FillOut(entries);

      _excelFile.Save();

      _excelFile.Close();

      return _excelFile;
    }


    private void SetHeader() {
      _excelFile.SetCell(_templateConfig.TitleCell, _templateConfig.Title);
      _excelFile.SetCell(_templateConfig.CurrentTimeCell,
                         $"Generado el: {DateTime.Now.ToString("dd/MMM/yyyy HH:mm")}");
    }


    private void FillOut(FixedList<InventoryEntryReportDto> entries) {
      int i = _templateConfig.FirstRowIndex;

      foreach (var entry in entries) {
        _excelFile.SetCell($"A{i}", entry.ProductCode);
        _excelFile.SetCell($"B{i}", entry.ProductDescription);
        _excelFile.SetCell($"C{i}", entry.PhysicalCount);
        _excelFile.SetCell($"D{i}", entry.Stock);
        _excelFile.SetCell($"E{i}", entry.Variance);
        _excelFile.SetCell($"F{i}", entry.CostVariance);
        _excelFile.SetCell($"G{i}", entry.IsMultiLocation);
        _excelFile.SetCell($"H{i}", entry.LocationReaded);
        _excelFile.SetCell($"I{i}", entry.Locations);

        i++;
      }
    }

  } // class AssetsToExcelBuilder

} // namespace Empiria.Inventory.Reporting
