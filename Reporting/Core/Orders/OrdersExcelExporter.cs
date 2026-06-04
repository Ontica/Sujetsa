/* Empiria Trade      ****************************************************************************************
*                                                                                                            *
*  Module   : Reporting Services                            Component : Service Layer                        *
*  Assembly : Empiria.Sujetsa.Reporting.Core.dll            Pattern   : Report builder                       *
*  Type     : OrdersExcelExporter                           License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Builds an Excel file with order and items entries information.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/


using Empiria.Office;
using Empiria.Orders;
using Empiria.Storage;
using Empiria.Trade.Core;
using Empiria.Trade.Procurement.Adapters;

namespace Empiria.Sujetsa.Reporting {

  /// <summary>Builds an Excel file with order and items entries information.</summary>
  internal class OrdersExcelExporter {

    private readonly FileTemplateConfig _templateConfig;

    private ExcelFile _excelFile;

    public OrdersExcelExporter(FileTemplateConfig templateConfig) {
      Assertion.Require(templateConfig, "templateConfig");

      _templateConfig = templateConfig;
    }

    #region Public methods

    internal ExcelFile CreateExcelFile(IOrderDto orderDto) {
      Assertion.Require(orderDto, "orderDto");

      _excelFile = new ExcelFile(_templateConfig);
      
      _excelFile.Open();

      SetHeader();

      SetTable(orderDto);

      _excelFile.Save();

      _excelFile.Close();

      return _excelFile;
    }

    #endregion Public methods


    #region Private methods

    private void SetHeader() {
      _excelFile.SetCell($"A2", _templateConfig.Title);

      var subTitle = $"";

      _excelFile.SetCell($"A3", subTitle);
    }


    private void SetTable(IOrderDto orderDto) {

      var orderSetTable = new OrdersFillOutExcelExporter();

      switch (_templateConfig.Name) {

        case "PurchaseOrder":

          PurchaseOrderDto dto = (PurchaseOrderDto) orderDto;

          orderSetTable.FillOutPurchaseOrder(_excelFile, dto);
          return;

        default:
          throw Assertion.EnsureNoReachThisCode();
      }
    }

    #endregion Private methods

  } // class OrdersExcelExporter

} // namespace Empiria.Operations.Reporting
