/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Product Management                         Component : Web Api                                 *
*  Assembly : Empiria.Trade.Products.dll                 Pattern   : Controller                              *
*  Type     : ProductTests                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Query web API used to retrieve temporary data products.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System.Threading.Tasks;
using System.Web.Http;
using Empiria.Inventory.Adapters;
using Empiria.Inventory.UseCases;
using Empiria.Storage;
using Empiria.Sujetsa.Reporting;
using Empiria.Trade.Integration.ETL;
using Empiria.WebApi;




namespace Empiria.Sujetsa.WebApi {

  /// <summary>Query web API used to retrieve temporary data products.</summary>
  public class OrdersController : WebApiController {


    [HttpGet]
    [Route("v4/trade-sujetsa/test")]
    public SingleObjectModel GetProductsCount() {

      var message = "Esto es una prueba";

      return new SingleObjectModel(this.Request, message);
    }

    [HttpPost]
    [Route("v4/trade-sujetsa/inventory/orders/{inventoryOrderUID:guid}/close")]
    public SingleObjectModel CloseInventoryOrder([FromUri] string inventoryOrderUID) {

      using (var usecases = InventoryOrderUseCases.UseCaseInteractor()) {

        InventoryHolderDto inventoryOrder = usecases.CloseInventoryOrder(inventoryOrderUID);

        var etlService = new ETLService();

        Task.Run(() => etlService.ExecuteReverseETL(inventoryOrder.Order.OrderNo))
            .ConfigureAwait(false)
            .GetAwaiter();

        return new SingleObjectModel(this.Request, inventoryOrder);
      }
    }


    [HttpPost]
    [Route("v4/trade-sujetsa/inventory/orders/search")]
    public SingleObjectModel SearchInventoryOrderList([FromBody] InventoryOrderQuery query) {

      ETLServiceInvoker.Start();

      using (var usecases = InventoryOrderUseCases.UseCaseInteractor()) {

        InventoryOrderDataDto inventoryOrderDto = usecases.SearchInventoryOrder(query);

        return new SingleObjectModel(this.Request, inventoryOrderDto);
      }
    }


    [HttpPost]
    [Route("v4/trade-sujetsa/inventory/orders/update")]
    public async Task<NoDataModel> UpdateDataUsingETL() {
      ////actualizar datos de sujetsa fb a sql server empiria
      var etlService = new ETLService();

      await etlService.ExecuteAll();

      return new NoDataModel(base.Request);
    }


    [HttpGet]
    [Route("v8/order-management/inventory-orders/{orderUID}/export-entries-report")]
    public SingleObjectModel ExportInventoryEntriesReport([FromUri] string orderUID) {


      using (var usecases = InventoryEntryUseCases.UseCaseInteractor()) {

        FixedList<InventoryEntryReportDto> reportentries = usecases.GetInventoryEntryReport(orderUID);

        FileDto report;

        using (var reporting = InventoryEntryReportingService.ServiceInteractor()) {
          report = reporting.ExportInventoryEntryReportToExcel(reportentries);
        }

        return new SingleObjectModel(this.Request, report);
      }
    }

  } // class ManageDataController

} // namespace Empiria.Trade.WebApi.SujetsaTemp
