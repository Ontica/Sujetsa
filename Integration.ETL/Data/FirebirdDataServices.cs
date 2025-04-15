/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Data Layer                            *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Service provider                      *
*  Type     : FirebirdDataServices                         License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides services to read data from Firebird databases.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Data;
using System.Linq;
using FirebirdSql.Data.FirebirdClient;

namespace Empiria.Trade.Integration.ETL.Data {

  /// <summary>Provides services to read data from Firebird databases.</summary>
  internal class FirebirdDataServices {

    private readonly string _connectionString;

    internal FirebirdDataServices(string connectionString) {
      Assertion.Require(connectionString, nameof(connectionString));

      _connectionString = connectionString;
    }


    internal void BulkInsertToFirebird(DataTable dataTable,  string tableName) {

    using (FbConnection connection = OpenConnection()) {

          using (var transaction = connection.BeginTransaction()) {
        using (var command = new FbCommand()) {
          command.Connection = connection;
          command.Transaction = transaction;

          var columns = string.Join(", ", dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
          var parameters = string.Join(", ", dataTable.Columns.Cast<DataColumn>().Select(c => "@" + c.ColumnName));
          command.CommandText = $"INSERT INTO {tableName} ({columns}) VALUES ({parameters})";

          foreach (DataColumn column in dataTable.Columns) {
            command.Parameters.Add("@" + column.ColumnName, FbTypeFromColumnType(column.DataType));
          }

          foreach (DataRow row in dataTable.Rows) {
            for (int i = 0; i < dataTable.Columns.Count; i++) {
              command.Parameters[i].Value = row[i];
            }

            command.ExecuteNonQuery();
          }
        }

        transaction.Commit();
      }

      connection.Close();
    }
  }

  FbDbType FbTypeFromColumnType(Type type) {
    if (type == typeof(int))
      return FbDbType.Integer;
    if (type == typeof(string))
      return FbDbType.VarChar;
    if (type == typeof(DateTime))
      return FbDbType.TimeStamp;
    if (type == typeof(bool))
      return FbDbType.Boolean;
    if (type == typeof(decimal))
      return FbDbType.Decimal;
    if (type == typeof(double))
      return FbDbType.Double;
    // Agrega más según sea necesario
    throw new NotSupportedException($"Tipo no soportado: {type.Name}");
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

    internal void TruncateTable(string tableName) {
      Assertion.Require(tableName, nameof(tableName));

      using (FbConnection dbConnection = OpenConnection()) {
        using (FbTransaction transaction = dbConnection.BeginTransaction()) {
          using (FbCommand cmdDelete = new FbCommand($"DELETE FROM {tableName}", dbConnection, transaction)) {
            cmdDelete.ExecuteNonQuery();
          }

          transaction.Commit(); // Confirma la transacción
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
