/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Services Layer                        *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Service provider                      *
*  Type     : ETLService                                   License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides Extract-Transform-Load services for Trade System's integration.                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Collections.Generic;
using System.Threading.Tasks;
using Empiria.Json;

using Empiria.Trade.Integration.ETL.Data;
using Empiria.Trade.Integration.ETL.Transformers;

namespace Empiria.Trade.Integration.ETL {

  /// <summary>Provides Extract-Transform-Load services for Trade System's integration.</summary>
  public class ETLService {

    private readonly string _inputSourceConnectionString;
    private readonly string _outputSourceConnectionString;
    private readonly string _outputSourceEmpiriaConnectionString;

    public ETLService() {
      var config = ConfigurationData.Get<JsonObject>("Connection.Strings");

      _outputSourceConnectionString = config.Get<string>("sqlServerConnection");
      _inputSourceConnectionString = config.Get<string>("firebirdConnection");
      _outputSourceEmpiriaConnectionString = config.Get<string>("empiriaSqlServerConnection");
    }


    public void Execute() {
      var inputDataServices = new FirebirdDataServices(_inputSourceConnectionString);
      var outputDataServices = new SqlServerDataServices(_outputSourceConnectionString);
      var outputEmpiriaDataServices = new SqlServerDataServices(_outputSourceEmpiriaConnectionString);

      var tablesToConvert = outputDataServices.GetTablesList();

      foreach (var table in tablesToConvert) {
        outputDataServices.TruncateTable(table.FullSourceTableName);
        string query = GetQueryForTable(table.SourceTableName);
        EmpiriaLog.Info("(Sujetsa ETL) Starting data extraction from NK Firebird table: " + table.SourceTableName);
        var newDataToStore = inputDataServices.GetDataTable(query);
        EmpiriaLog.Info("(Sujetsa ETL) Data extraction completed from NK Firebird table: " + table.SourceTableName + " extracted records: " + newDataToStore.Rows.Count);
        outputDataServices.StoreDataTable(newDataToStore, table.FullSourceTableName);
      }
      EmpiriaLog.Info("(Sujetsa ETL) Starting Execute Merge Stored Procedure");
      outputDataServices.ExecuteMergeStoredProcedure();
      EmpiriaLog.Info("(Sujetsa ETL) Completed Execute Merge Stored Procedure");

      foreach (var table in tablesToConvert) {
        EmpiriaLog.Info("(Sujetsa ETL) Records Count after merge in table: " + table.FullTargetTableName + " total records: " + outputDataServices.RowCounter(table.FullTargetTableName));
      }
      
    }


    public async Task ExecuteAll() {

      await Task.Run(() => {
        EmpiriaLog.Info("(Sujetsa ETL) Starting ETL Extraction...");
        Execute();
        EmpiriaLog.Info("(Sujetsa ETL) ETL Extraction finished...");

        EmpiriaLog.Info("(Sujetsa ETL) Starting ETL Transformers execution...");

        EmpiriaLog.Info("(Sujetsa ETL) Starting Product Transformer execution...");
        var productTransformer = new ProductTransformer(_outputSourceEmpiriaConnectionString);
        productTransformer.Execute();
        EmpiriaLog.Info("(Sujetsa ETL) Product Transformer execution finished.");

               
        EmpiriaLog.Info("(Sujetsa ETL) Starting Party Transformer execution...");
        var partyTransformer = new PartyTransformer(_outputSourceEmpiriaConnectionString);
        partyTransformer.Execute();
        EmpiriaLog.Info("(Sujetsa ETL) Party Transformer execution finished.");


        EmpiriaLog.Info("(Sujetsa ETL) Starting Contact Transformer execution...");
        var contactTransformer = new ContactTransformer(_outputSourceEmpiriaConnectionString);
        contactTransformer.Execute();
        EmpiriaLog.Info("(Sujetsa ETL) Contact Transformer execution finished.");


        EmpiriaLog.Info("(Sujetsa ETL) Starting Order Invoice and Items Transformer execution...");
        var orderInvoice_and_ItemsTransformer = new OrderInvoiceTransformer(_outputSourceEmpiriaConnectionString);
        orderInvoice_and_ItemsTransformer.Execute();
        EmpiriaLog.Info("(Sujetsa ETL) Order Invoice and Items Transformer execution finished.");
        
        EmpiriaLog.Info("(Sujetsa ETL) Starting Order Credit Note and Items Transformer execution...");
        var orderCreditNote_and_ItemsTransformer = new OrderCreditNoteTransformer(_outputSourceEmpiriaConnectionString);
        orderCreditNote_and_ItemsTransformer.Execute();
        EmpiriaLog.Info("(Sujetsa ETL) Order Credit Note and Items Transformer execution finished.");
        
        EmpiriaLog.Info("(Sujetsa ETL) Starting Order Purchase and Items Transformer execution...");
        var orderPurchaseTransformer_and_ItemsTransformer = new OrderPurchaseTransformer(_outputSourceEmpiriaConnectionString);
        orderPurchaseTransformer_and_ItemsTransformer.Execute();
        EmpiriaLog.Info("(Sujetsa ETL) Order Purchase and Items Transformer execution finished.");
      
        EmpiriaLog.Info("(Sujetsa ETL) Starting Order Rem and Items Transformer execution...");
        var orderRemTransformer_and_ItemsTransformer = new OrderRemTransformer(_outputSourceEmpiriaConnectionString);
        orderRemTransformer_and_ItemsTransformer.Execute();
        EmpiriaLog.Info("(Sujetsa ETL) Order Rem and Items Transformer execution finished.");
          
        ////ov,ovdet devolucion, devoluciondet para el incio de inventarios
        /*
        EmpiriaLog.Info("(Sujetsa ETL) Starting Order OV and Items Transformer execution...");
        var orderTransformer_and_ItemsTransformer = new OrderTransformer(_outputSourceEmpiriaConnectionString);
        orderTransformer_and_ItemsTransformer.Execute();
        EmpiriaLog.Info("(Sujetsa ETL) Order OV and Items Transformer execution finished.");

        EmpiriaLog.Info("(Sujetsa ETL) Starting Devolucion Rem and Items Transformer execution...");
        var orderReturnTransformer_and_ItemsTransformer = new OrderReturnTransformer(_outputSourceEmpiriaConnectionString);
        orderReturnTransformer_and_ItemsTransformer.Execute();
        EmpiriaLog.Info("(Sujetsa ETL) Order Devolucion and Items Transformer execution finished.");
        */

        var connectionString = GetNKConnectionString();
        var outputDataServices = new SqlServerDataServices(connectionString);
        outputDataServices.ExecuteUpdateOrderItemsStatusStoredProcedure();
        outputDataServices.ExecuteUpdatePartyStatusStoredProcedure();

        var tablesToCount = outputDataServices.GetEmpiriaTablesList();

        var outputEmpiriaDataServices = new SqlServerDataServices(_outputSourceEmpiriaConnectionString);

        foreach (var table in tablesToCount) {  
          EmpiriaLog.Info("(Sujetsa ETL) Data transformation completed to Empiria table: " + table.Tipo + " Total records: " + outputEmpiriaDataServices.RowCounter(table.Tipo)); 
        }

        EmpiriaLog.Info("(Sujetsa ETL) ETL Transformers execution finished.");
      });
    }


    public void ExecuteReverseETL(string order_no) {
      var inputDataServices = new SqlServerDataServices(_outputSourceEmpiriaConnectionString);
      var outputDataServices = new FirebirdDataServices(_inputSourceConnectionString);
      EmpiriaLog.Info("(Sujetsa ETL) Starting Reverse ETL Extraction...");
      const string sourceTable = "[dbo].[VW_Inventory_Return] WHERE ESTATUS_PARTIDA = 'C' AND INV_NO =  ";
      const string destinationTable = "INV_SUJ";

      string query = $"SELECT * FROM {sourceTable} '" + order_no + "'";

      var dataToTransfer = inputDataServices.GetDataTable(query);

      outputDataServices.BulkInsertToFirebird(dataToTransfer, destinationTable);

      EmpiriaLog.Info("(Sujetsa ETL) Reverse ETL Extraction Finished.");
    }


    #region Helpers

    private string GetQueryForTable(string tableName) {
      var baseDateFilter = "FECHA >= '2025-12-01'";

      var queries = new Dictionary<string, string> {
        { "OV", $"SELECT * FROM {tableName} WHERE {baseDateFilter}" },
        { "ICMOV", $"SELECT * FROM {tableName} WHERE {baseDateFilter}" },
        { "FACTURA", $"SELECT * FROM {tableName} WHERE {baseDateFilter}" },
        { "REML", $"SELECT * FROM {tableName} WHERE {baseDateFilter}" },
        { "DEVOLUCION", $"SELECT * FROM {tableName} WHERE {baseDateFilter}" },
        { "COMPRA", $"SELECT * FROM {tableName} WHERE {baseDateFilter}" },
        { "NOTACREDITO", $"SELECT * FROM {tableName} WHERE {baseDateFilter} AND TIPO = 'D'" },
        { "PRODUCTOALMACENLOC", $@"SELECT 
            (PRODUCTO || '' || ALMACEN || '' || COALESCE(RACK, '') || '' || FECHAMOV) AS DB_KEY,
            PRODUCTO, ALMACEN, RACK, NIVEL, INDICE, FECHA, RACKANT, NIVELANT, INDICEANT, FECHAANT,
            COMPRA, FECHAMOV, USUARIO 
            FROM {tableName} " },
        { "OVUBICACIONCONSECUTIVO", $"SELECT * FROM {tableName} WHERE INI_SURTIO >= '2025-01-01'" },
        { "PRODUCTOALMACEN", $"SELECT PA.* FROM {tableName} PA JOIN ALMACEN A ON PA.ALMACEN = A.ALMACEN" },
        { "OVDET", $"SELECT OT.* FROM {tableName} OT JOIN OV O ON O.OV = OT.OV AND {baseDateFilter}" },
        { "ICMOVDET", $"SELECT OT.* FROM {tableName} OT JOIN ICMOV O ON O.ICMOV = OT.ICMOV AND {baseDateFilter}" },
        { "FACTURADET", $"SELECT OT.* FROM {tableName} OT JOIN FACTURA O ON O.FACTURA = OT.FACTURA AND {baseDateFilter}" },
        { "REMLDET", $"SELECT OT.* FROM {tableName} OT JOIN REML O ON O.REML = OT.REML AND {baseDateFilter}" },
        { "COMPRADET", $"SELECT OT.* FROM {tableName} OT JOIN COMPRA O ON O.COMPRA = OT.COMPRA AND {baseDateFilter}" },
        { "DEVOLUCIONDET", $"SELECT OT.* FROM {tableName} OT JOIN DEVOLUCION O ON O.DEVOLUCION = OT.DEVOLUCION AND {baseDateFilter}" },
        { "NOTACREDITODET", $"SELECT OT.* FROM {tableName} OT JOIN NOTACREDITO O ON O.NOTACREDITO = OT.NOTACREDITO AND {baseDateFilter} AND O.TIPO = 'D'" }
      };

      return queries.TryGetValue(tableName, out string query) ? query : $"SELECT * FROM {tableName}";
    }

    static private string GetNKConnectionString() {
      var config = ConfigurationData.Get<JsonObject>("Connection.Strings");

      return config.Get<string>("sqlServerConnection");
    }
    #endregion Helpers

  }  // class ETLService

} // namespace Empiria.Trade.Integration.ETL
