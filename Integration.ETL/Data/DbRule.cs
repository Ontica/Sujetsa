/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Data Layer                            *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Value Object                          *
*  Type     : DbRule                                       License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Generates negative id values using rules for the ETL.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

namespace Empiria.Trade.Integration.ETL.Data {

  /// <summary>Generates negative id values using rules for the ETL.</summary>
  internal class DbRule {

    static private FixedList<DbRule> _allDBRules = null;

    static private readonly object _locker = new object();

    static DbRule() {
      LoadDbRules();
    }


    [DataField("SOURCENAME")]
    private string SOURCENAME {
      get; set;
    }


    [DataField("IDFIELDNAME")]
    private int IDFIELDNAME {
      get; set;
    }


    private static int CurrentValue {
      get; set;
    }


    static public int GetNextId(string tableName) {
      lock (_locker) {
        if (CurrentValue != 0) {
          CurrentValue = CurrentValue - 1;
        } else {
          CurrentValue = GetMinValue(tableName) - 1;
        }
        return CurrentValue;
      }
    }

    static private int GetMinValue(string tableName) {

      DbRule rule = _allDBRules.Find(x => x.SOURCENAME.ToLower() == tableName.ToLower());

      var sql = $"SELECT MIN({rule.IDFIELDNAME}) FROM {tableName}";

      DataOperation op = DataOperation.Parse(sql);

      int minValue = DataReader.GetScalar(op, -1);

      if (minValue == -1 || minValue >= 0) {
        return -100;
      } else {
        return minValue;
      }
    }


    static private void LoadDbRules() {
      var sql = $"SELECT * FROM DBRules WHERE ServerId = {ExecutionServer.ServerId}";

      DataOperation op = DataOperation.Parse(sql);

      _allDBRules = DataReader.GetPlainObjectFixedList<DbRule>(op);
    }

  }  // class DbRule

}  // namespace Empiria.Trade.Integration.ETL.Data
