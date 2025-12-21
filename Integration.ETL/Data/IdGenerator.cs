/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Data Layer                            *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Service provider                      *
*  Type     : IdGenerator, DbRule                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Generates negative id values using rules for the ETL.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Collections;
using Empiria.Data;

namespace Empiria.Trade.Integration.ETL.Data {

  /// <summary>Generates negative id values using rules for the ETL.</summary>
  static internal class IdGenerator {

    static public int GetNextId(string tableName) {
      Assertion.Require(tableName, nameof(tableName));

      return DbRule.GetNextId(tableName);
    }



    /// <summary>Inner class that generates negative IDs for a specific table.</summary>
    private class DbRule {

      static private EmpiriaHashTable<DbRule> _dbRules = null;

      static private readonly object _locker = new object();

      static DbRule() {
        LoadDbRules();
      }

      static internal int GetNextId(string tableName) {
        DbRule rule = _dbRules[tableName.ToLower()];

        return rule.GetNextId();
      }


      [DataField("SOURCENAME")]
      private string SOURCENAME {
        get; set;
      }


      [DataField("IDFIELDNAME")]
      private string IDFIELDNAME {
        get; set;
      }


      private int CurrentValue {
        get; set;
      }

      #region Helpers

      static private int GetMinValue(string tableName) {

        DbRule rule = _dbRules[tableName.ToLower()];

        var sql = $"SELECT MIN({rule.IDFIELDNAME}) FROM {tableName}";

        DataOperation op = DataOperation.Parse(sql);

        int minValue = DataReader.GetScalar(op, -1);

        if (minValue == -1 || minValue >= 0) {
          return -100;
        } else {
          return minValue;
        }
      }


      private int GetNextId() {
        lock (_locker) {

          if (CurrentValue != 0) {
            CurrentValue = CurrentValue - 1;
          } else {
            CurrentValue = GetMinValue(SOURCENAME) - 1;
          }

          return CurrentValue;
        }
      }


      static private void LoadDbRules() {
        var sql = $"SELECT * FROM DBRules WHERE ServerId = {ExecutionServer.ServerId}";

        DataOperation op = DataOperation.Parse(sql);

        _dbRules = DataReader.GetPlainObjectHashTable<DbRule>(op, (x) => x.SOURCENAME.ToLower());
      }

      #endregion Helpers

    }  // class DbRule

  }  // class IdGenerator

}  // namespace Empiria.Trade.Integration.ETL.Data
