/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Tests Layer                           *
*  Assembly : Empiria.Trade.Integration.Tests              Pattern   : Unit tests                            *
*  Type     : FirebirdDataServicesTests                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Unit tests for FirebirdDataServices.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Json;

using Empiria.Trade.Integration.ETL.Data;


namespace Empiria.Tests.Trade.Integration {

  /// <summary>Unit tests for FirebirdDataServicesTests.</summary>
  public class FirebirdDataServicesTests    {

    #region Facts

    [Fact]
    public void Should_Get_DataTable() {
      string query = "Select * from Linea";
      string connectionString = GetConnectionString();

      var dataServices = new FirebirdDataServices(connectionString);

      var sut = dataServices.GetDataTable(query);

      Assert.NotNull(sut);
     
    }

    #endregion Facts

    #region Helpers

    static private string GetConnectionString() {
      var config = ConfigurationData.Get<JsonObject>("Connection.Strings");

      return config.Get<string>("firebirdConnection");
    }

    #endregion Helpers

  }  // class FirebirdDataServicesTests

}  // namespace Empiria.Tests.Trade.Integration
