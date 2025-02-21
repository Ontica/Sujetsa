using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Data.Common;
using System.Collections.Generic;
using System.IO;
using TradeDataSchemaManager.Data;


namespace TradeDataSchemaManager.Services
{
    internal class HelperFBSS
    {
        public dynamic GetConnectionsInfo(bool pisTest)
        {
            string jsonFilePath="";
            try
            {
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                jsonFilePath = pisTest
                    ? Path.Combine(new DirectoryInfo(baseDirectory).Parent.FullName, @"Assets\ConnectionStringsFBSS")
                    : Path.Combine(baseDirectory, "..", "..", "..", @"Assets\ConnectionStringsFBSS.json");

                string jsonContent = File.ReadAllText(jsonFilePath);
                dynamic config = JsonConvert.DeserializeObject(jsonContent);

                return config;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en HelperFBSS.GetConnectionsInfo(): {ex.Message} - Ruta del archivo: {jsonFilePath}", ex);
            }
        }
        public int ETL(string pSSConnection, string pFBConnection)
        {
            int Ejecutado = 0;
            using (SqlConnection connectionSS = DataServiceSS.getInstance().CreateConnection(pSSConnection))
            {
                connectionSS.Open();
                try
                {
                    DataServiceSS Datasqlsvr = new DataServiceSS();
                    var initialTableList = Datasqlsvr.GetTablesList(pSSConnection);

                    foreach (DataRow row in initialTableList.Rows)
                    {
                        var tableName = row[0].ToString(); 
                        var tableToTruncate = Datasqlsvr.GetTableToTruncate(tableName, pSSConnection);
                        string queryTruncate = $"TRUNCATE TABLE {tableToTruncate}";
                        using (SqlCommand cmdTruncate = new SqlCommand(queryTruncate, connectionSS))
                        {
                            cmdTruncate.ExecuteNonQuery();
                        }

                        string selectToFB = $"SELECT * FROM {tableName}";
                        DataServiceFB dataSelect = new DataServiceFB();
                        var dataFromFB = dataSelect.ReadDataFromTable(selectToFB, pFBConnection);

                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connectionSS))
                        {
                            bulkCopy.DestinationTableName = tableToTruncate;
                            bulkCopy.BatchSize = dataFromFB.Rows.Count;
                            bulkCopy.WriteToServer(dataFromFB);
                        }
                    }
                    Ejecutado = Datasqlsvr.ExecuteMergeStoredProcedure(pSSConnection);

                    return Ejecutado;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al ejecutar el ETL."+ Ejecutado, ex);
                }
            }
        }
    }
}
