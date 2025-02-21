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

using FirebirdSql.Data.FirebirdClient;

namespace Empiria.Trade.Integration.ETL.Data {

  /// <summary>Provides services to read data from Firebird databases.</summary>
  internal class FirebirdDataServices {

    public DataTable ReadDataFromTable(string pSelectToFB, string pFBConnection) {
      try {
        using (FbConnection SqlConFB = CreateConnection(pFBConnection))
        using (FbDataAdapter dataAdapter = new FbDataAdapter(pSelectToFB, SqlConFB)) {
          SqlConFB.Open();
          var dataTable = new DataTable();
          dataAdapter.Fill(dataTable);
          return dataTable;
        }
      } catch (Exception ex) {
        throw new Exception($"Error en ReadDataFromTable FB con la consulta: {pSelectToFB}", ex);
      }
    }

    #region Helpers

    private FbConnection CreateConnection(string pFBConnection) {
      if (string.IsNullOrEmpty(pFBConnection)) {
        throw new ArgumentException("La cadena de conexión no puede ser nula o vacía.", nameof(pFBConnection));
      }
      return new FbConnection(pFBConnection);
    }

    #endregion Helpers

  }  // class FirebirdDataServices

}  // namespace Empiria.Trade.Integration.ETL.Data
