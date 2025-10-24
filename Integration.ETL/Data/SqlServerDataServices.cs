/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Data Layer                            *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Service provider                      *
*  Type     : SqlServerDataServices                        License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides services to read and write data from SQL Server databases.                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Data;
using System.Data.SqlClient;

using System.Linq;

namespace Empiria.Trade.Integration.ETL.Data {

  /// <summary>Provides services to read and write data from SQL Server databases.</summary>
  internal class SqlServerDataServices {

    private readonly string _connectionString;

    internal SqlServerDataServices(string connectionString) {
      Assertion.Require(connectionString, nameof(connectionString));

      _connectionString = connectionString;
    }


    internal void ExecuteFillCommonStorageStoredProcedure() {

      using (SqlConnection dbConnection = OpenConnection()) {

        using (SqlCommand cmd = new SqlCommand("sources.OMS_Fill_Common_Storage", dbConnection)) {
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.CommandTimeout = 300;

          cmd.ExecuteNonQuery();
        }
      }
    }

    internal void ExecuteUpdateOrderItemsStatusStoredProcedure() {

      using (SqlConnection dbConnection = OpenConnection()) {

        using (SqlCommand cmd = new SqlCommand("sources.OMS_Update_Order_Items_Status", dbConnection)) {
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.CommandTimeout = 300;

          cmd.ExecuteNonQuery();
        }
      }
    }

    internal void ExecuteUpdatePartyStatusStoredProcedure() {

      using (SqlConnection dbConnection = OpenConnection()) {

        using (SqlCommand cmd = new SqlCommand("sources.Update_Party_Status", dbConnection)) {
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.CommandTimeout = 300;

          cmd.ExecuteNonQuery();
        }
      }
    }


    internal void ExecuteUpdateProductStatusProcedure() {

      using (SqlConnection dbConnection = OpenConnection()) {

        using (SqlCommand cmd = new SqlCommand("sources.OMS_Update_Product_Status", dbConnection)) {
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.CommandTimeout = 300;

          cmd.ExecuteNonQuery();
        }
      }
    }


    internal void ExecuteMergeStoredProcedure() {

      using (SqlConnection dbConnection = OpenConnection()) {

        using (SqlCommand cmd = new SqlCommand("sources.OMS_Merge_Intermediate_Tables", dbConnection)) {
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.CommandTimeout = 300;

          cmd.ExecuteNonQuery();
        }
      }
    }

    internal DataTable GetDataTable(string query) {
      Assertion.Require(query, nameof(query));

      using (SqlConnection dbConnection = OpenConnection()) 
      {
        using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, dbConnection)) {
          var dataTable = new DataTable();
          dataAdapter.Fill(dataTable);
          return dataTable;
        }
      }
    }


    internal int? GetChecksum(string fullTableName) {
      Assertion.Require(fullTableName, nameof(fullTableName));

      string query = $@"
            SELECT CHECKSUM_AGG(CHECKSUM(BinaryChecksum))
            FROM {fullTableName}";
      using (SqlConnection dbConnection = OpenConnection()) {

        using (SqlCommand cmd = new SqlCommand(query, dbConnection)) {

          return cmd.ExecuteScalar() as int?;
        }
      }
    }

    internal FixedList<ETLEmpiriaTable> GetEmpiriaTablesList() {
      var commandString = "SELECT Distinct(Tipo) " +
                          "FROM sources.OMS_Intermediate_Tables_List " +
                          "WHERE Active = 'T' " +
                          "AND Tipo NOT IN('', ' ') " +
                          "ORDER BY tipo DESC ";

      using (SqlConnection dbConnection = OpenConnection()) {

        using (SqlCommand cmd = new SqlCommand(commandString, dbConnection)) {

          var dataReader = cmd.ExecuteReader();

          var dataTable = new DataTable();

          dataTable.Load(dataReader);

          return dataTable.Select()
                          .Select(row => new ETLEmpiriaTable((string) row["Tipo"]))
                          .ToFixedList();
        }
      }
    }

    internal FixedList<ETLTable> GetTablesList() {
      var commandString = "SELECT SourceTable, FullSourceTableName, FullTargetTableName " +
                          "FROM sources.OMS_Intermediate_Tables_List " +
                          "WHERE Active = 'T'";

      using (SqlConnection dbConnection = OpenConnection()) {

        using (SqlCommand cmd = new SqlCommand(commandString, dbConnection)) {

          var dataReader = cmd.ExecuteReader();

          var dataTable = new DataTable();

          dataTable.Load(dataReader);

          return dataTable.Select()
                          .Select(row => new ETLTable((string) row["SourceTable"],
                                                      (string) row["FullSourceTableName"],
                                                      (string) row["FullTargetTableName"]))
                          .ToFixedList();
        }
      }
    }

 
    internal int RowCounter(string tableName) {
      Assertion.Require(tableName, nameof(tableName));

      using (SqlConnection dbConnection = OpenConnection()) {

        using (SqlCommand cmd = new SqlCommand($"SELECT COUNT(*) FROM {tableName}", dbConnection)) {

          return (int) cmd.ExecuteScalar();
        }
      }
    }


    internal void StoreDataTable(DataTable dataTable, string destinationTableName) {
      Assertion.Require(dataTable, nameof(dataTable));
      Assertion.Require(destinationTableName, nameof(destinationTableName));

      using (SqlConnection dbConnection = OpenConnection()) {

        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(dbConnection)) {
          bulkCopy.BulkCopyTimeout = 300; 
          bulkCopy.DestinationTableName = destinationTableName;
          EmpiriaLog.Info("(Sujetsa ETL) Starting data transfer to SQL Server table: "+destinationTableName);
          bulkCopy.BatchSize = dataTable.Rows.Count;
          bulkCopy.WriteToServer(dataTable);
          EmpiriaLog.Info("(Sujetsa ETL) Data transfer completed to SQL Server table: " + destinationTableName + " transferred records: "+ RowCounter(destinationTableName));
        }
      }
    }


    internal void TruncateTable(string tableName) {
      Assertion.Require(tableName, nameof(tableName));

      using (SqlConnection dbConnection = OpenConnection()) {

        using (SqlCommand cmdTruncate = new SqlCommand($"TRUNCATE TABLE {tableName}", dbConnection)) {
            cmdTruncate.ExecuteNonQuery();
          
        }
      }
    }

    #region Helpers

    private SqlConnection OpenConnection() {
      var connection = new SqlConnection(_connectionString);

      connection.Open();

      return connection;
    }

    #endregion Helpers

  }  // class SqlServerDataServices

}  // namespace Empiria.Trade.Integration.ETL.Data
