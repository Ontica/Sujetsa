/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Data Layer                            *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Value Object                          *
*  Type     : ETLEmpiriaTable                              License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Describes an ETL Empiria target table.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Trade.Integration.ETL.Data {

  /// <summary>Describes an ETL source table.</summary>
  internal class ETLEmpiriaTable {
    public ETLEmpiriaTable(string tipo) {
      Assertion.Require(tipo, nameof(tipo));

      Tipo = tipo;

    }

    public string Tipo {
      get;
    }


  }  // class ETLEmpiriaTable

}  // namespace Empiria.Trade.Integration.ETL.Data
