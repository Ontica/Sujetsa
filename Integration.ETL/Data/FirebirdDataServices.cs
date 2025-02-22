/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Data Layer                            *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Service provider                      *
*  Type     : FirebirdDataServices                         License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides services to read data from Firebird databases.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Data;

using FirebirdSql.Data.FirebirdClient;

namespace Empiria.Trade.Integration.ETL.Data {

  /// <summary>Provides services to read data from Firebird databases.</summary>
  internal class FirebirdDataServices {

    private readonly string _connectionString;

    internal FirebirdDataServices(string connectionString) {
      Assertion.Require(connectionString, nameof(connectionString));

      _connectionString = connectionString;
    }


    internal DataTable GetDataTable(string query) {
      Assertion.Require(query, nameof(query));

      using (FbConnection dbConnection = OpenConnection()) {

        using (FbDataAdapter dataAdapter = new FbDataAdapter(query, dbConnection)) {
          var dataTable = new DataTable();

          dataAdapter.Fill(dataTable);

          return dataTable;
        }
      }
    }

    #region Helpers

    private FbConnection OpenConnection() {
      var connection = new FbConnection(_connectionString);

      connection.Open();

      return connection;
    }

    #endregion Helpers

  }  // class FirebirdDataServices

}  // namespace Empiria.Trade.Integration.ETL.Data
