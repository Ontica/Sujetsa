﻿/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Services Layer                        *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Service provider                      *
*  Type     : ETLService                                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides Extract-Transform-Load services for Trade System's integration.                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Collections.Generic;
using System.Data;
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

      var tablesToConvert = outputDataServices.GetTablesList();

      foreach (var table in tablesToConvert) {
        outputDataServices.TruncateTable(table.FullSourceTableName);
        string query = GetQueryForTable(table.SourceTableName);
        var newDataToStore = inputDataServices.GetDataTable(query);
        outputDataServices.StoreDataTable(newDataToStore, table.FullSourceTableName);
      }

      outputDataServices.ExecuteMergeStoredProcedure();
      outputDataServices.ExecuteFillCommonStorageStoredProcedure();
    }

    private string GetQueryForTable(string tableName) {
      var baseDateFilter = "FECHA >= '2025-01-01'";

      var queries = new Dictionary<string, string>
      {
        { "OV", $"SELECT * FROM {tableName} WHERE {baseDateFilter}" },
        { "ICMOV", $"SELECT * FROM {tableName} WHERE {baseDateFilter}" },
        { "FACTURA", $"SELECT * FROM {tableName} WHERE {baseDateFilter}" },
        { "REML", $"SELECT * FROM {tableName} WHERE {baseDateFilter}" },
        { "COMPRA", $"SELECT * FROM {tableName} WHERE {baseDateFilter}" },
        { "PRODUCTOALMACENLOC", $@"SELECT 
            (PRODUCTO || '' || ALMACEN || '' || COALESCE(RACK, '') || '' || FECHAMOV) AS DB_KEY,
            PRODUCTO, ALMACEN, RACK, NIVEL, INDICE, FECHA, RACKANT, NIVELANT, INDICEANT, FECHAANT,
            COMPRA, FECHAMOV, USUARIO 
            FROM {tableName} WHERE FECHAMOV >= '2025-01-01' ORDER BY FECHAMOV" },
        { "OVUBICACIONCONSECUTIVO", $"SELECT * FROM {tableName} WHERE INI_SURTIO >= '2025-01-01'" },
        { "PRODUCTOALMACEN", $"SELECT PA.* FROM {tableName} PA JOIN ALMACEN A ON PA.ALMACEN = A.ALMACEN" },
        { "OVDET", $"SELECT OT.* FROM {tableName} OT JOIN OV O ON O.OV = OT.OV AND {baseDateFilter}" },
        { "ICMOVDET", $"SELECT OT.* FROM {tableName} OT JOIN ICMOV O ON O.ICMOV = OT.ICMOV AND {baseDateFilter}" },
        { "FACTURADET", $"SELECT OT.* FROM {tableName} OT JOIN FACTURA O ON O.FACTURA = OT.FACTURA AND {baseDateFilter}" },
        { "REMLDET", $"SELECT OT.* FROM {tableName} OT JOIN REML O ON O.REML = OT.REML AND {baseDateFilter}" },
        { "COMPRADET", $"SELECT OT.* FROM {tableName} OT JOIN COMPRA O ON O.COMPRA = OT.COMPRA AND {baseDateFilter}" },
        { "DEVOLUCIONDET", $"SELECT OT.* FROM {tableName} OT JOIN DEVOLUCION O ON O.DEVOLUCION = OT.DEVOLUCION AND {baseDateFilter}" },
        { "NOTACREDITODET", $"SELECT OT.* FROM {tableName} OT JOIN NOTACREDITO O ON O.NOTACREDITO = OT.NOTACREDITO AND {baseDateFilter}" }
    };

      return queries.TryGetValue(tableName, out string query) ? query : $"SELECT * FROM {tableName}";
    }


  }  // class ETLService

} // namespace Empiria.Trade.Integration.ETL
