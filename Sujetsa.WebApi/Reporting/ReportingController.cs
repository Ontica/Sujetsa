/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Reporting Management                       Component : Web Api                                 *
*  Assembly : Empiria.Sujetsa.Reporting.dll              Pattern   : Controller                              *
*  Type     : SujetsaReportingController                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Query web API used to retrieve inventory reportings.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System.Web.Http;
using Empiria.Inventory.Adapters;
using Empiria.Inventory.UseCases;
using Empiria.Storage;
using Empiria.Sujetsa.Reporting;
using Empiria.WebApi;

namespace Empiria.Sujetsa.WebApi {

  /// <summary>Query web API used to retrieve inventory reportings.</summary>
  public class SujetsaReportingController : WebApiController {

    [HttpGet]
    [Route("v8/order-management/inventory-orders/{orderUID}/items/export-report")]
    public SingleObjectModel ExportInventoryItemsReport([FromUri] string orderUID) {


      using (var usecases = InventoryEntryUseCases.UseCaseInteractor()) {

        FixedList<InventoryEntryReportDto> reportentries = usecases.GetInventoryEntryReport(orderUID);

        FileDto report;

        using (var reporting = InventoryEntryReportingService.ServiceInteractor()) {
          report = reporting.ExportInventoryEntryReportToExcel(reportentries);
        }

        return new SingleObjectModel(this.Request, report);
      }
    }


  } // class SujetsaReportingController

} // namespace Empiria.Sujetsa.WebApi
