/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Data Layer                            *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Service provider                      *
*  Type     : TransformerDataServices                      License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides services to read, transform and write data from SQL Server databases.                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Data.SqlClient;

using Empiria.Data;

namespace Empiria.Trade.Integration.ETL.Data {

  /// <summary>Provides services to read and write data from SQL Server databases.</summary>
  internal class TransformerDataServices {

    private readonly string _connectionString;

    internal TransformerDataServices(string connectionString) {
      Assertion.Require(connectionString, nameof(connectionString));

      _connectionString = connectionString;
    }


    internal int? GetCategoryIdFromCommonStorage(string grupo, string subGrupo) {
      Assertion.Require(grupo, nameof(grupo));
      Assertion.Require(subGrupo, nameof(subGrupo));
      int grupoInt = int.Parse(grupo);
      int subGrupoInt = int.Parse(subGrupo);
      using (SqlConnection dbConnection = OpenConnection()) {

        using (SqlCommand cmd = new SqlCommand($"SELECT Object_ID FROM DBO.Common_Storage WHERE Object_Category_Id = {grupoInt} AND Object_Classification_Id = {subGrupoInt}", dbConnection)) {
          var result = cmd.ExecuteScalar();

          if (result != null && (int) result != 0) {

            return (int) result;
          } else {
            return -1;
          }
        }
      }
    }

    
    internal DateTime GetPostingDateFromOMSOrders(string ov) {
      Assertion.Require(ov, nameof(ov));
      using (SqlConnection dbConnection = OpenConnection()) {

        using (SqlCommand cmd = new SqlCommand($"SELECT Order_Posting_Time FROM dbo.OMS_Orders  " +
          $" WHERE Order_No = '{ov}'", dbConnection)) {
          var result = cmd.ExecuteScalar();

          return (DateTime) result;
        }
      }
    }


    internal DateTime GetClosedDateFromOvUbicacionConsecutivo(string ov) {
      Assertion.Require(ov, nameof(ov));
      using (SqlConnection dbConnection = OpenConnection()) {

        using (SqlCommand cmd = new SqlCommand($"SELECT U.FIN_FACTURO " +
          $" FROM sources.OVUBICACIONCONSECUTIVO_TARGET U " +
          $" WHERE U.OV = '{ov}'", dbConnection)) {
          var result = cmd.ExecuteScalar();

          if (result == DBNull.Value || result == null) {
            return ExecutionServer.DateMinValue;
          }

          return Convert.ToDateTime(result);
        }
      }
    }


    internal int GetClosedIdFromOvUbicacionConsecutivo(string ov) {
      Assertion.Require(ov, nameof(ov));
      using (SqlConnection dbConnection = OpenConnection()) {

        using (SqlCommand cmd = new SqlCommand($"SELECT U.USR_SURTIO " +
          $" FROM sources.OVUBICACIONCONSECUTIVO_TARGET U " +
          $" WHERE U.OV = '{ov}'", dbConnection)) {
          var result = cmd.ExecuteScalar();

          if (result == DBNull.Value || result == null) {
            return -1;
          }

          return Convert.ToInt32(result);
        }
      }
    }


    public int GetNextId(string tableName) {
      return DataWriter.CreateId(tableName);
    }


    internal string GetObjectTagsFromCommonStorage(string grupo, string subGrupo) {
      Assertion.Require(grupo, nameof(grupo));
      Assertion.Require(subGrupo, nameof(subGrupo));
      int grupoInt = int.Parse(grupo);
      int subGrupoInt = int.Parse(subGrupo);
      using (SqlConnection dbConnection = OpenConnection()) {

        using (SqlCommand cmd = new SqlCommand($"SELECT Object_Tags FROM DBO.Common_Storage WHERE Object_Category_Id = {grupoInt}" +
          $" AND Object_Classification_Id = {subGrupoInt}", dbConnection)) {
          var result = cmd.ExecuteScalar();

          if (result != null) {

            return result.ToString();
          } else {
            return "";
          }
        }
      }
    }

    internal int GetOrderIdFromOMSOrders(string orden) {
      Assertion.Require(orden, nameof(orden));
      using (SqlConnection dbConnection = OpenConnection()) {

        using (SqlCommand cmd = new SqlCommand($"SELECT Order_Id FROM dbo.OMS_Orders  WHERE Order_No = '{orden}'", dbConnection)) {
          var result = cmd.ExecuteScalar();

          return (int) result;
        }
      }
    }

    internal int GetOrderIdFromOMSOrdersItems(int id, int det) {
      Assertion.Require(id, "Necesito el Order Item Order Id");
      Assertion.Require(det, "Necesito el DET");
      using (SqlConnection dbConnection = OpenConnection()) {

        using (SqlCommand cmd = new SqlCommand($"SELECT Order_Item_Id FROM dbo.OMS_Order_Items  WHERE Order_Item_Order_Id = '{id}' " +
          $"and Order_Item_Position = {det}", dbConnection)) {
          var result = cmd.ExecuteScalar();

          return (int) result;
        }
      }
    }


    internal int GetOrderItemProviderIdFromOMSOrders(string orden) {
      Assertion.Require(orden, nameof(orden));
      using (SqlConnection dbConnection = OpenConnection()) {
        using (SqlCommand cmd = new SqlCommand($"SELECT Order_Provider_Id FROM dbo.OMS_Orders WHERE Order_No = '{orden}'", dbConnection)) {
          var result = cmd.ExecuteScalar();
          return (int) result;
        }
      }
    }


    internal string GetOrderItemStatusFromOMSOrders(string orden) {
      Assertion.Require(orden, nameof(orden));
      using (SqlConnection dbConnection = OpenConnection()) {

        using (SqlCommand cmd = new SqlCommand($"SELECT Order_Status FROM dbo.OMS_Orders WHERE Order_No = '{orden}'", dbConnection)) {
          var result = cmd.ExecuteScalar();

          return result.ToString();
        }
      }
    }


    internal string GetOrderUIDFromOMSOrders(string orden) {
      Assertion.Require(orden, nameof(orden));
      using (SqlConnection dbConnection = OpenConnection()) {

        using (SqlCommand cmd = new SqlCommand($"SELECT Order_UID FROM dbo.OMS_Orders  WHERE Order_No = '{orden}'", dbConnection)) {
          var result = cmd.ExecuteScalar();

          return result.ToString();
        }
      }
    }


    internal string GetOrderUIDFromOMSOrdersItems(int id, int det) {
      Assertion.Require(id, "Necesito el Order Item Order Id");
      Assertion.Require(det, "Necesito el DET");
      using (SqlConnection dbConnection = OpenConnection()) {

        using (SqlCommand cmd = new SqlCommand($"SELECT Order_UID FROM dbo.OMS_Order_Items  WHERE Order_Item_Order_Id = '{id}' " +
          $" and Order_Item_Position = {det}", dbConnection)) {
          var result = cmd.ExecuteScalar();

          return result.ToString();
        }
      }
    }

    internal int GetPartyIdFromParties(string cliente) {
      if (string.IsNullOrEmpty(cliente)) {
         return -1;
      }
      Assertion.Require(cliente, nameof(cliente));
      using (SqlConnection dbConnection = OpenConnection()) {

        using (SqlCommand cmd = new SqlCommand($"SELECT Party_Id FROM DBO.Parties WHERE Party_Identificators = '{cliente}'", dbConnection)) {
          var result = cmd.ExecuteScalar();

          if (result == DBNull.Value || result == null) {
            return -1;
          }

          return (int) result;
        }
      }
    }


    internal string GetPartyUIDFromParties(string cliente) {
      Assertion.Require(cliente, nameof(cliente));
      using (SqlConnection dbConnection = OpenConnection()) {

        using (SqlCommand cmd = new SqlCommand($"SELECT Party_UID FROM DBO.Parties WHERE Party_Identificators = '{cliente}'", dbConnection)) {
          var result = cmd.ExecuteScalar();

          return result.ToString();
        }
      }
    }

    

    internal DateTime GetPostedDateFromOMSOrders(string orden) {
      Assertion.Require(orden, nameof(orden));
      using (SqlConnection dbConnection = OpenConnection()) {

        using (SqlCommand cmd = new SqlCommand($"SELECT Order_Posting_Time FROM dbo.OMS_Orders WHERE Order_No = '{orden}'", dbConnection)) {
          var result = cmd.ExecuteScalar();

          return Convert.ToDateTime(result);
        }
      }
    }

    internal int GetPostedUserIdFromOMSOrders(string orden) {
      Assertion.Require(orden, nameof(orden));
      using (SqlConnection dbConnection = OpenConnection()) {

        using (SqlCommand cmd = new SqlCommand($"SELECT Order_Posted_By_Id FROM dbo.OMS_Orders WHERE Order_No = '{orden}'", dbConnection)) {
          var result = cmd.ExecuteScalar();

          return (int) result;
        }
      }
    }


    internal int GetProductIdFromOMSProducts(string producto) {
      Assertion.Require(producto, nameof(producto));
      using (SqlConnection dbConnection = OpenConnection()) {

        using (SqlCommand cmd = new SqlCommand($"SELECT Product_Id FROM DBO.OMS_Products WHERE Product_Name = '{producto}'", dbConnection)) {
          var result = cmd.ExecuteScalar(); 

            return (int) result;
        }
      }
    }


    internal string GetProductUIDFromOMSProducts(string producto) {
      Assertion.Require(producto, nameof(producto));
      using (SqlConnection dbConnection = OpenConnection()) {

        using (SqlCommand cmd = new SqlCommand($"SELECT Product_UID FROM DBO.OMS_Products WHERE Product_Name = '{producto}'", dbConnection)) {
          var result = cmd.ExecuteScalar();

          return result.ToString();
        }
      }
    }


    internal int GetRequestedUserIdFromOMSOrders(string orden) {
      Assertion.Require(orden, nameof(orden));
      using (SqlConnection dbConnection = OpenConnection()) {

        using (SqlCommand cmd = new SqlCommand($"SELECT Order_Requested_By_Id FROM dbo.OMS_Orders WHERE Order_No = '{orden}'", dbConnection)) {
          var result = cmd.ExecuteScalar();

          return (int) result;
        }
      }
    }

    internal FixedList<T> ReadData<T>(string sql) {
      Assertion.Require(sql, nameof(sql));
      
      var dataSource = new DataSource("TransformerDataServices", _connectionString, DataTechnology.SqlServer);
      var op = DataOperation.Parse(dataSource, sql);

      return DataReader.GetPlainObjectFixedList<T>(op);
    }


    internal char ReturnIdForPriority(string prioridad) {
      switch (prioridad) {
        case "1":
          return 'U';
        case "2":
          return 'U';
        case "3":
          return 'U';
        case "4":
          return 'N';
        default:
          return ' ';
      }
    }


    internal int ReturnIdForProductBaseUnitId(string unidadMedida) {
      switch (unidadMedida) {
        case "H87":
          return 1110;
        case "KGM":
          return 1111;
        case "X44":
          return 1112;
        default:
          return -1;
      }
    }


    internal string ReturnOldDescriptionForPriority(string prioridad) {
      switch (prioridad) {
        case "1":
          return "OCURRE";
        case "2":
          return "URGENTE OV";
        case "3":
          return "URGENTE OC";
        case "4":
          return "NORMAL";
        default:
          return " ";
      }
    }

    
    internal char ReturnStatusforPartyStatus(string activo) {
      switch (activo) {
        case "N":
          return 'S';
        case "S":
          return 'A';
        default:
          return 'S';
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

    internal int GetWareHouseIdFromCommonStorage(string almacen) {
      if (string.IsNullOrEmpty(almacen)) {
        return -1;
      }
      Assertion.Require(almacen, nameof(almacen));

      using (SqlConnection dbConnection = OpenConnection()) {

        using (SqlCommand cmd = new SqlCommand($"SELECT Object_Id FROM DBO.Common_Storage WHERE [Object_Named_Key] = '{almacen}' ", dbConnection)) {
          var result = cmd.ExecuteScalar();

          if (result != null) {

            return Convert.ToInt32(result);

          } else {

            return -1;
          }
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

  }  // class TransformerDataServices

}  // namespace Empiria.Trade.Integration.ETL.Data
