/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Reporting Management                       Component : Web Api                                 *
*  Assembly : Empiria.Sujetsa.Reporting.dll              Pattern   : Controller                              *
*  Type     : ReportingController                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Query web API used to retrieve inventory reportings.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System.Web.Http;

using Empiria.WebApi;

namespace Empiria.Sujetsa.WebApi {

  /// <summary>Query web API used to retrieve inventory reportings.</summary>
  public class ReportingController : WebApiController {


    [HttpGet]
    [Route("v4/trade-sujetsa/test")]
    public SingleObjectModel GetProductsCount() {

      var message = "Esto es una prueba";

      return new SingleObjectModel(this.Request, message);
    }



  } // class ReportingController

} // namespace Empiria.Sujetsa.WebApi
