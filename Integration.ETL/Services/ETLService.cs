/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Services Layer                        *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Service provider                      *
*  Type     : ETLService                                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides testing constants.                                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using System.Data;
using System.Data.SqlClient;

using Empiria.Json;

using Empiria.Trade.Integration.ETL.Data;

namespace Empiria.Trade.Integration.ETL {

  public class ETLService {

    private readonly string _sqlServerConnection;
    private readonly string _firebirdConnection;

    public ETLService() {
      var config = ConfigurationData.Get<JsonObject>("Connection.Strings");

      _sqlServerConnection = config.Get<string>("sqlServerConnection");
      _firebirdConnection = config.Get<string>("firebirdConnection");
    }


    public int Execute() {
      int Ejecutado = 0;

      using (SqlConnection connectionSS = SqlServerDataServices.CreateConnection(_sqlServerConnection)) {
        connectionSS.Open();

        try {

          SqlServerDataServices Datasqlsvr = new SqlServerDataServices();
          var initialTableList = Datasqlsvr.GetTablesList(_sqlServerConnection);

          foreach (DataRow row in initialTableList.Rows) {
            var tableName = row[0].ToString();
            var tableToTruncate = Datasqlsvr.GetTableToTruncate(tableName, _sqlServerConnection);
            string queryTruncate = $"TRUNCATE TABLE {tableToTruncate}";

            using (SqlCommand cmdTruncate = new SqlCommand(queryTruncate, connectionSS)) {
              cmdTruncate.ExecuteNonQuery();
            }

            string selectToFB = $"SELECT * FROM {tableName}";
            FirebirdDataServices dataSelect = new FirebirdDataServices();
            var dataFromFB = dataSelect.ReadDataFromTable(selectToFB, _firebirdConnection);

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connectionSS)) {
              bulkCopy.DestinationTableName = tableToTruncate;
              bulkCopy.BatchSize = dataFromFB.Rows.Count;
              bulkCopy.WriteToServer(dataFromFB);
            }

          }  // foreach

          Ejecutado = Datasqlsvr.ExecuteMergeStoredProcedure(_sqlServerConnection);

          return Ejecutado;

        } catch (Exception ex) {
          throw new Exception("Error al ejecutar el ETL." + Ejecutado, ex);
        }

      }  // using
    }

  }  // class ETLService

} // namespace Empiria.Trade.Integration.ETL
