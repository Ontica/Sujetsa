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
using System.Data;

namespace Empiria.Tests.Trade.Integration {

  /// <summary>Unit tests for SqlServerDataServices.</summary>
  public class SqlServerDataServicesTests {

    #region Facts


    [Fact]
    public void Should_Execute_Update_Order_Items_Status_Stored_Proceduree() {

      string connectionString = GetConnectionString();

      var sut = new SqlServerDataServices(connectionString);

      sut.ExecuteUpdateOrderItemsStatusStoredProcedure();

      Assert.NotNull(sut);

    }


    [Fact]
    public void Should_Execute_Update_Product_Status_Procedure() {
   
      string connectionString = GetConnectionString();

      var sut = new SqlServerDataServices(connectionString);

      sut.ExecuteUpdateProductStatusProcedure();

      Assert.NotNull(sut);

    }


    [Fact]
    public void Should_Execute_Fill_Common_Storage_Stored_Procedure() {

      string connectionString = GetConnectionString();

      var sut = new SqlServerDataServices(connectionString);

      sut.ExecuteFillCommonStorageStoredProcedure();

      Assert.NotNull(sut);

      string fullTableName = "[sujetsa].DBO.[Common_Storage]";

      int rowCount = sut.RowCounter(fullTableName);

      Assert.True(rowCount >= 0);

    }


    [Fact]
    public void Should_Get_Empiria_Tables_List() {
      string connectionString = GetConnectionString();

      var dataServices = new SqlServerDataServices(connectionString);

      FixedList<ETLEmpiriaTable> sut = dataServices.GetEmpiriaTablesList();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Get_Tables_List() {
      string connectionString = GetConnectionString();

      var dataServices = new SqlServerDataServices(connectionString);

      FixedList<ETLTable> sut = dataServices.GetTablesList();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Make_Truncate_Table() {
      string fullTableName = "NKsujetsaFB.sources.LINEA";
      string connectionString = GetConnectionString();

      var sut = new SqlServerDataServices(connectionString);

      sut.TruncateTable(fullTableName);

      int rowCount = sut.RowCounter(fullTableName);

      Assert.Equal(0, rowCount);
    }


    [Fact]
    public void Should_Return_Checksum() {
      string tableName = "NKsujetsaFB.sources.PRODUCTOALMACENLOC";

      string connectionString = GetConnectionString();

      var dataServices = new SqlServerDataServices(connectionString);

      var sut = dataServices.GetChecksum(tableName);

      Assert.NotNull(sut);
      Assert.True(sut > 0);
    }


    [Fact]
    public void Should_Return_Valid_Row_Count() {
      string tableName = "OMS_Order_Items where Order_Item_Type_Id != 4059";

      string connectionString = GetEmpiriaConnectionString();

      var sut = new SqlServerDataServices(connectionString);

      int rowCount = sut.RowCounter(tableName);

      Assert.True(rowCount >= 0);
    }


    [Fact]
    public void Should_Store_DataTable() {
      string destinationTableName = "NKsujetsaFB.sources.LINEA";
      DataTable dataTableTest = new DataTable();

      dataTableTest.Columns.Add("LINEA", typeof(string));
      dataTableTest.Columns.Add("NOMBRE", typeof(string));
      dataTableTest.Columns.Add("MARGEN", typeof(decimal));
      dataTableTest.Columns.Add("PERMISO", typeof(string));
      dataTableTest.Columns.Add("BinaryChecksum", typeof(byte[]));
      dataTableTest.Rows.Add("001", "ESTANDAR", null, null, null);
      dataTableTest.Rows.Add("002", "FINO", null, null, null);

      string connectionString = GetConnectionString();

      var sut = new SqlServerDataServices(connectionString);

      sut.StoreDataTable(dataTableTest, destinationTableName);

      int rowCount = sut.RowCounter(destinationTableName);

      Assert.Equal(2, rowCount);
      Assert.True(rowCount > 0);
    }


    [Fact]
    public void Should_Try_MergeStoredProcedure() {
      string tableName1 = "NKsujetsaFB.sources.OVUBICACIONCONSECUTIVO";
      string tableName2 = "NKsujetsaFB.sources.OVUBICACIONCONSECUTIVO_TARGET";

      string connectionString = GetConnectionString();

      var sut = new SqlServerDataServices(connectionString);

      sut.ExecuteMergeStoredProcedure();

      int checksumAfterExecuteMergeStoredProcedureTableName1 = (int)sut.GetChecksum(tableName1);
      int checksumAfterExecuteMergeStoredProcedureTableName2 = (int)sut.GetChecksum(tableName2);

      Assert.Equal(checksumAfterExecuteMergeStoredProcedureTableName1, checksumAfterExecuteMergeStoredProcedureTableName2);
    }


    #endregion Facts

    #region Helpers

    static private string GetConnectionString() {
      var config = ConfigurationData.Get<JsonObject>("Connection.Strings");

      return config.Get<string>("sqlServerConnection");
    }


    static private string GetEmpiriaConnectionString() {
      var config = ConfigurationData.Get<JsonObject>("Connection.Strings");

      return config.Get<string>("empiriaSqlServerConnection");
    }

    #endregion Helpers

  }  // class SqlServerDataServicesTests

}  // namespace Empiria.Tests.Trade.Integration
