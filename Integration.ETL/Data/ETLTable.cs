/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Data Layer                            *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Value Object                          *
*  Type     : ETLTable                                     License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Describes an ETL source table.                                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Trade.Integration.ETL.Data {

  /// <summary>Describes an ETL source table.</summary>
  internal class ETLTable {

    public ETLTable(string sourceTable, string fullSourceTableName) {
      Assertion.Require(sourceTable, nameof(sourceTable));
      Assertion.Require(fullSourceTableName, nameof(fullSourceTableName));

      SourceTableName = sourceTable;
      FullSourceTableName = fullSourceTableName;
    }

    public string SourceTableName {
      get;
    }

    public string FullSourceTableName {
      get;
    }

  }  // class ETLTable

}  // namespace Empiria.Trade.Integration.ETL.Data
