/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Sujetsa Inventory Management               Component : Web Api                                 *
*  Assembly : Empiria.Trade.Sujetsa.WebApi.dll           Pattern   : Controller                              *
*  Type     : InventoryOrdersController                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Query web API used to retrieve Inventory orders.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Threading.Tasks;
using System.Web.Http;

using Empiria.Trade.Inventory.Adapters;
using Empiria.Trade.Inventory.UseCases;

using Empiria.WebApi;

using Empiria.Trade.Integration.ETL;

namespace Empiria.Sujetsa.WebApi {

  /// <summary>Query web API used to retrieve Inventory orders.</summary>
  public class InventoryOrdersController : WebApiController {

    [HttpPost]
    [Route("v4/trade-sujetsa/inventory/orders/{inventoryOrderUID:guid}/close")]
    public SingleObjectModel CloseInventoryOrder([FromUri] string inventoryOrderUID) {

      using (var usecases = InventoryOrderUseCases.UseCaseInteractor()) {

        InventoryOrderDto inventoryOrder = usecases.CloseInventoryOrder(inventoryOrderUID);

        var etlService = new ETLService();

        Task.Run(() => etlService.ExecuteReverseETL(inventoryOrder.InventoryOrderNo))
            .ConfigureAwait(false)
            .GetAwaiter();

        return new SingleObjectModel(this.Request, inventoryOrder);
      }
    }


    [HttpPost]
    [Route("v4/trade-sujetsa/inventory/orders/search")]
    public SingleObjectModel SearchInventoryOrders([FromBody] InventoryOrderQuery query) {

      ETLServiceInvoker.Start();

      using (var usecases = InventoryOrderUseCases.UseCaseInteractor()) {

        InventoryOrderDataDto inventoryOrderDto = usecases.SearchInventoryOrders(query);

        return new SingleObjectModel(this.Request, inventoryOrderDto);
      }
    }


    [HttpPost]
    [Route("v4/trade-sujetsa/inventory/orders/update")]
    public async Task<NoDataModel> UpdateDataUsingETL([FromBody] InventoryOrderQuery query) {

      var etlService = new ETLService();

      await etlService.ExecuteAll();

      return new NoDataModel(base.Request);
    }

  } // class InventoryOrdersController

} // namespace Empiria.Sujetsa.WebApi
