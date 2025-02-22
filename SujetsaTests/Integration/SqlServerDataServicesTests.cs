/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Tests Layer                           *
*  Assembly : Empiria.Trade.Integration.Tests              Pattern   : Unit tests                            *
*  Type     : SqlServerDataServicesTests                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Unit tests for SqlServerDataServices.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Json;

using Empiria.Trade.Integration.ETL.Data;

namespace Empiria.Tests.Trade.Integration {

  /// <summary>Unit tests for SqlServerDataServices.</summary>
  public class SqlServerDataServicesTests {

    #region Facts

    [Fact]
    public void Should_Get_Tables_List() {
      string connectionString = GetConnectionString();

      var dataServices = new SqlServerDataServices(connectionString);

      FixedList<ETLTable> sut = dataServices.GetTablesList();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }

    #endregion Facts

    #region Helpers

    static private string GetConnectionString() {
      var config = ConfigurationData.Get<JsonObject>("Connection.Strings");

      return config.Get<string>("sqlServerConnection");
    }

    #endregion Helpers

  }  // class SqlServerDataServicesTests

}  // namespace Empiria.Tests.Trade.Integration
