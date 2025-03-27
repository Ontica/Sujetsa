/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Services Layer                        *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Service provider                      *
*  Type     : ETLService                                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides Extract-Transform-Load services for Trade System's integration.                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
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
      var newDataToStore = (DataTable) null;

      FixedList<ETLTable> tablesToConvert = outputDataServices.GetTablesList();

      foreach (ETLTable table in tablesToConvert) {

        outputDataServices.TruncateTable(table.FullSourceTableName);

        if (table.SourceTableName == "CAPA" || table.SourceTableName == "OV" 
           || table.SourceTableName == "ICMOV" || table.SourceTableName == "FACTURA"
           || table.SourceTableName == "REML") {
            newDataToStore = inputDataServices.GetDataTable($"SELECT * FROM {table.SourceTableName} WHERE FECHA >= '2025-01-01'");

        } else if (table.SourceTableName == "PRODUCTOALMACENLOC") {
            newDataToStore = inputDataServices.GetDataTable($"SELECT (PRODUCTO || '' || ALMACEN || '' || COALESCE(RACK, '') || '' || FECHAMOV) AS DB_KEY, " +
              $"PRODUCTO, ALMACEN, RACK, NIVEL, INDICE, FECHA, RACKANT, NIVELANT, INDICEANT, FECHAANT, " +
              $" COMPRA, FECHAMOV, USUARIO FROM {table.SourceTableName} WHERE FECHAMOV >= '2025-01-01' order by FECHAMOV");
          
        } else if (table.SourceTableName == "OVUBICACIONCONSECUTIVO") {
          newDataToStore = inputDataServices.GetDataTable($"SELECT * FROM {table.SourceTableName} WHERE INI_SURTIO >= '2025-01-01' ");

        } else if (table.SourceTableName == "PRODUCTOALMACEN") {
            newDataToStore = inputDataServices.GetDataTable($"SELECT PA.* FROM {table.SourceTableName} PA JOIN ALMACEN A ON PA.ALMACEN = A.ALMACEN ");

        } else if (table.SourceTableName == "OVDET") {
            newDataToStore = inputDataServices.GetDataTable($"SELECT OT.* FROM {table.SourceTableName} OT JOIN OV O  ON O.OV = OT.OV AND O.FECHA >= '2025-01-01' ");

        } else if (table.SourceTableName == "ICMOVDET") {
            newDataToStore = inputDataServices.GetDataTable($"SELECT OT.* FROM {table.SourceTableName} OT JOIN ICMOV O ON O.ICMOV = OT.ICMOV AND O.FECHA >= '2025-01-01' ");

        } else if (table.SourceTableName == "FACTURADET") {
          newDataToStore = inputDataServices.GetDataTable($"SELECT OT.* FROM {table.SourceTableName} OT JOIN FACTURA O ON O.FACTURA = OT.FACTURA AND O.FECHA > '2025-01-01' ");

        } else if (table.SourceTableName == "REMLDET") {
          newDataToStore = inputDataServices.GetDataTable($"SELECT OT.* FROM {table.SourceTableName} OT JOIN REML O ON O.REML= OT.REML AND O.FECHA > '2025-01-01' ");

        } else {
            newDataToStore = inputDataServices.GetDataTable($"SELECT * FROM {table.SourceTableName}");
        }

        outputDataServices.StoreDataTable(newDataToStore, table.FullSourceTableName);

      }  // foreach

      outputDataServices.ExecuteMergeStoredProcedure();
    }

  }  // class ETLService

} // namespace Empiria.Trade.Integration.ETL
