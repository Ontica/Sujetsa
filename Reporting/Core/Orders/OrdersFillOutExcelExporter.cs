/* Empiria Trade      ****************************************************************************************
*                                                                                                            *
*  Module   : Reporting Services                            Component : Service Layer                        *
*  Assembly : Empiria.Sujetsa.Reporting.Core.dll            Pattern   : Report builder                       *
*  Type     : OrdersFillOutExcelExporter                    License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Fill out table info for a Microsoft Excel file with order and item entries information.        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Spreadsheet;
using Empiria.Financial;
using Empiria.Office;
using Empiria.Trade.Core;
using Empiria.Trade.Procurement.Adapters;

namespace Empiria.Sujetsa.Reporting {

  /// <summary>Fill out table info for a Microsoft Excel file with order and item entries information.</summary>
  internal class OrdersFillOutExcelExporter {


    private readonly System.Drawing.Color DESCRIPCTION_ROW_COLOR = System.Drawing.Color.FromArgb(235, 235, 235);
    private readonly System.Drawing.Color TOTAL_ROW_COLOR = System.Drawing.Color.FromArgb(155, 194, 230);

    internal OrdersFillOutExcelExporter() {

    }

    #region Public methods

    public void FillOutPurchaseOrder(ExcelFile _excelFile, PurchaseOrderDto order) {

      _excelFile.SetCell($"B4", order.OrderNumber);
      _excelFile.SetCell($"C4", $"{order.Supplier.Name}");
      _excelFile.SetCell($"F4", $"Date");
      _excelFile.SetCell($"G4", DateTime.Now.ToString("dd-MM-yyyy"));

      FillOutPurchaseOrderItems(_excelFile, order);
    }

    #endregion Public methods

    #region Private methods

    private void FillOutPurchaseOrderItems(ExcelFile _excelFile, PurchaseOrderDto order) {

      var items = order.Items.Select(x => x);
      int i = 8;

      foreach (var item in items) {

        var totalUnits = item.Quantity * item.PackagingSize;
        var totalMpcs = item.Total / (totalUnits / 1000);

        _excelFile.SetCell($"C{i}", $"{item.Description}, {item.ProductAttrs}");
        _excelFile.SetRowBackgroundStyle(i, 7, DESCRIPCTION_ROW_COLOR);
        i++;
        _excelFile.SetCell($"A{i}", item.ProductCode);
        _excelFile.SetCell($"B{i}", $"{item.ProductAttrsShort}");
        _excelFile.SetCell($"C{i}", item.Quantity);
        _excelFile.SetCell($"D{i}", item.PackingSmallBag);
        _excelFile.SetCell($"E{i}", totalUnits);
        _excelFile.SetCell($"F{i}", Math.Round(totalMpcs, 2, MidpointRounding.AwayFromZero));
        _excelFile.SetCell($"G{i}", Math.Round(item.Total, 2, MidpointRounding.AwayFromZero));

        i++;
      }

      _excelFile.SetCell($"A{i}", order.Totals.ItemsCount);
      _excelFile.SetCell($"E{i}", "Total Amount:");
      _excelFile.SetCell($"F{i}", Currency.Parse(order.Currency.UID).ISOCode);
      _excelFile.SetCell($"G{i}", items.Sum(x => x.Total));
      _excelFile.SetRowBold(i, 7);
      _excelFile.SetRowBackgroundStyle(i, 7, TOTAL_ROW_COLOR);
      i++;
      _excelFile.SetCell($"C{i}", "We hereby confirm the purchase of the goods under the previously " +
                                  "established terms and conditions.");
      i++;
      _excelFile.SetCell($"C{i}", "Please send evidence of the product and packaging so the shipment " +
                                  "can be released.");
      i++;
      _excelFile.SetCell($"C{i}", "Please send the requested product and packaging inspection report " +
                                  "so the cargo can be released prior to shipment.");
    }

    #endregion Private methods

  } // class OrdersFillOutExcelExporter

} //  namespace Empiria.Sujetsa.Reporting
