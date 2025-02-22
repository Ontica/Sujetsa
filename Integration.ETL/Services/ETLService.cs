/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Services Layer                        *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Service provider                      *
*  Type     : ETLService                                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides Extract-Transform-Load services for Trade System's integration.                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Json;

using Empiria.Trade.Integration.ETL.Data;

namespace Empiria.Trade.Integration.ETL {

  /// <summary>Provides Extract-Transform-Load services for Trade System's integration.</summary>
  public class ETLService {

    private readonly string _inputSourceConnectionString;
    private readonly string _outputSourceConnectionString;

    public ETLService() {
      var config = ConfigurationData.Get<JsonObject>("Connection.Strings");

      _outputSourceConnectionString = config.Get<string>("sqlServerConnection");
      _inputSourceConnectionString = config.Get<string>("firebirdConnection");
    }


    public void Execute() {

      var inputDataServices = new FirebirdDataServices(_inputSourceConnectionString);
      var outputDataServices = new SqlServerDataServices(_outputSourceConnectionString);

      FixedList<string> tablesList = outputDataServices.GetTablesList();

      foreach (string tableName in tablesList) {

        string tableNameToTruncate = outputDataServices.GetTableToTruncate(tableName);

        outputDataServices.TruncateTable(tableNameToTruncate);

        var newDataToStore = inputDataServices.GetDataTable($"SELECT * FROM {tableName}");

        outputDataServices.StoreDataTable(newDataToStore, tableNameToTruncate);

      }  // foreach

      outputDataServices.ExecuteMergeStoredProcedure();
    }

  }  // class ETLService

} // namespace Empiria.Trade.Integration.ETL
