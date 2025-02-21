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
using System.IO;

using System.Data;
using System.Data.SqlClient;

using Newtonsoft.Json;

using Empiria.Trade.Integration.ETL.Data;

namespace Empiria.Trade.Integration.ETL {

  public class ETLService {

    public void Execute(bool isTest) {
      dynamic config = GetConnectionsInfo(isTest);

      string SSConnection = config.ConnectionStrings.SSConnection;
      string FBConnection = config.ConnectionStrings.FBConnection;

      ETL(SSConnection, FBConnection);
    }

    #region Helpers

    private int ETL(string pSSConnection, string pFBConnection) {
      int Ejecutado = 0;

      using (SqlConnection connectionSS = SqlServerDataServices.CreateConnection(pSSConnection)) {
        connectionSS.Open();

        try {

          SqlServerDataServices Datasqlsvr = new SqlServerDataServices();
          var initialTableList = Datasqlsvr.GetTablesList(pSSConnection);

          foreach (DataRow row in initialTableList.Rows) {
            var tableName = row[0].ToString();
            var tableToTruncate = Datasqlsvr.GetTableToTruncate(tableName, pSSConnection);
            string queryTruncate = $"TRUNCATE TABLE {tableToTruncate}";

            using (SqlCommand cmdTruncate = new SqlCommand(queryTruncate, connectionSS)) {
              cmdTruncate.ExecuteNonQuery();
            }

            string selectToFB = $"SELECT * FROM {tableName}";
            FirebirdDataServices dataSelect = new FirebirdDataServices();
            var dataFromFB = dataSelect.ReadDataFromTable(selectToFB, pFBConnection);

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connectionSS)) {
              bulkCopy.DestinationTableName = tableToTruncate;
              bulkCopy.BatchSize = dataFromFB.Rows.Count;
              bulkCopy.WriteToServer(dataFromFB);
            }

          }  // foreach

          Ejecutado = Datasqlsvr.ExecuteMergeStoredProcedure(pSSConnection);

          return Ejecutado;

        } catch (Exception ex) {
          throw new Exception("Error al ejecutar el ETL." + Ejecutado, ex);
        }

      }  // using
    }


    private dynamic GetConnectionsInfo(bool pisTest) {
      string jsonFilePath = "";
      try {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        jsonFilePath = pisTest
            ? Path.Combine(new DirectoryInfo(baseDirectory).Parent.FullName, @"Assets\ConnectionStringsFBSS")
            : Path.Combine(baseDirectory, "..", "..", "..", @"Assets\ConnectionStringsFBSS.json");

        string jsonContent = File.ReadAllText(jsonFilePath);
        dynamic config = JsonConvert.DeserializeObject(jsonContent);

        return config;
      } catch (Exception ex) {
        throw new Exception($"Error en HelperFBSS.GetConnectionsInfo(): {ex.Message} - Ruta del archivo: {jsonFilePath}", ex);
      }
    }

    #endregion Helpers

  }  // class ETLService

} // namespace Empiria.Trade.Integration.ETL
