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
using DocumentFormat.OpenXml.Spreadsheet;
using Empiria.Office;
using Empiria.Trade.Core;
using Empiria.Trade.Procurement.Adapters;

namespace Empiria.Sujetsa.Reporting {

  /// <summary>Fill out table info for a Microsoft Excel file with order and item entries information.</summary>
  internal class OrdersFillOutExcelExporter {

    internal OrdersFillOutExcelExporter() {

    }

    #region Public methods

    public void FillOutPurchaseOrder(ExcelFile _excelFile, PurchaseOrderDto order) {

      var ShippingMethod = order.ShippingMethod.ToString() != "None" ?
                              order.ShippingMethod.ToString() : "";

      var paymentConditions = order.PaymentConditions.ToString() != "None" ?
                              order.PaymentConditions.ToString() : "";

      _excelFile.SetCell($"B4", order.OrderNumber);
      _excelFile.SetCell($"C4", $"Proveedor: {order.Supplier.Name}");
      _excelFile.SetCell($"B5", ShippingMethod);
      _excelFile.SetCell($"C5", $"Condiciones de pago: {paymentConditions}");
      //_excelFile.SetCell($"A6", order.ScheduledTime);
      _excelFile.SetCell($"C6", $"Observaciones: {order.Notes}");
      
      //_excelFile.SetCell($"E6", order.PostingTime);

      FillOutPurchaseOrderItems(_excelFile, order);
    }

    #endregion Public methods

    #region Private methods

    private void FillOutPurchaseOrderItems(ExcelFile _excelFile, PurchaseOrderDto order) {

      var items = order.Items.Select(x => x);
      int i = 8;
      foreach (var item in items) {

        _excelFile.SetCell($"A{i}", item.ProductCode);
        _excelFile.SetCell($"B{i}", item.PresentationName);
        _excelFile.SetCell($"C{i}", item.Notes);
        _excelFile.SetCell($"D{i}", item.Quantity);
        _excelFile.SetCell($"E{i}", item.Price);
        _excelFile.SetCell($"F{i}", item.Total);

        i++;
      }

      _excelFile.SetCell($"A{i}", order.Totals.ItemsCount);
      _excelFile.SetCell($"F{i}", order.Totals.ItemsTotal);
      _excelFile.SetRowBold(i, 6);
      //_excelFile.RemoveColumn("K");
    }

    #endregion Private methods

  } // class OrdersFillOutExcelExporter

} //  namespace Empiria.Sujetsa.Reporting
