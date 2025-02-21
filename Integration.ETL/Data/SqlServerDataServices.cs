/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Data Layer                            *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Service provider                      *
*  Type     : SqlServerDataServices                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides services to read and write data from SQL Server databases.                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using System.Data;
using System.Data.SqlClient;

namespace Empiria.Trade.Integration.ETL.Data {

  /// <summary>Provides services to read and write data from SQL Server databases.</summary>
  internal class SqlServerDataServices {

    static public SqlConnection CreateConnection(string pSSConnection) {
      if (string.IsNullOrEmpty(pSSConnection)) {
        throw new ArgumentException("La cadena de conexión no puede ser nula o vacía.", nameof(pSSConnection));
      }
      return new SqlConnection(pSSConnection);
    }


    public int ExecuteMergeStoredProcedure(string pSSConnection) {
      int rowsAffected = 0;
      try {
        using (SqlConnection connectionSS = CreateConnection(pSSConnection))
        using (SqlCommand cmd = new SqlCommand("sources.OMS_Merge_Intermediate_Tables", connectionSS)) {
          cmd.CommandType = CommandType.StoredProcedure;
          connectionSS.Open();
          rowsAffected = cmd.ExecuteNonQuery();
        }

        return rowsAffected;
      } catch (Exception ex) {
        throw new Exception("Error al ejecutar procedimiento almacenado", ex);
      }
    }


    public string GetTableToTruncate(string ptableName, string pSSConnection) {
      try {
        using (SqlConnection connectionSS = CreateConnection(pSSConnection))
        using (SqlCommand cmd = new SqlCommand("SELECT FullSourceTableName FROM sources.OMS_Intermediate_Tables_List WHERE SourceTable = @SourceTable AND Active = 'T'", connectionSS)) {
          cmd.Parameters.AddWithValue("@SourceTable", ptableName);

          connectionSS.Open();

          using (SqlDataReader dataReader = cmd.ExecuteReader()) {
            if (dataReader.Read()) {
              return dataReader["FullSourceTableName"].ToString();
            }
          }

          return string.Empty;
        }
      } catch (Exception ex) {
        throw new Exception($"Error al truncar la tabla {ptableName}. Detalles del error: {ex.Message}", ex);
      }
    }


    public DataTable GetTablesList(string pSSConnection) {
      try {
        using (SqlConnection connectionSS = CreateConnection(pSSConnection))
        using (SqlCommand cmd = new SqlCommand("SELECT SourceTable FROM sources.OMS_Intermediate_Tables_List WHERE Active = 'T'", connectionSS)) {
          connectionSS.Open();
          using (var dataReader = cmd.ExecuteReader()) {
            var dataTable = new DataTable();
            dataTable.Load(dataReader);
            return dataTable;
          }
        }
      } catch (Exception ex) {
        throw new Exception("Error al obtener la lista de tablas de sources.OMS_Intermediate_Tables_List en Sql Server.", ex);
      }
    }

  }  // class SqlServerDataServices

}  // namespace Empiria.Trade.Integration.ETL.Data
