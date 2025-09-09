/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Services Layer                        *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Messsage queue processor              *
*  Type     : ETLServiceInvoker                            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Invokes a ETL services async using a timer.                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using System.Threading;

namespace Empiria.Trade.Integration.ETL {

  /// <summary>Invokes a ETL services async using a timer.</summary>
  static public class ETLServiceInvoker {

    #region Fields

    private static volatile bool isRunning = false;
    private static volatile Timer timer = null;

    #endregion Fields


    #region Engine methods

    static public bool IsRunning {
      get {
        return isRunning;
      }
    }


    /// <summary>Starts the execution engine for ETLService.</summary>
    static public void Start() {
      try {
        if (isRunning) {
          return;
        }

        int MESSAGE_ENGINE_EXECUTION_MINUTES = ConfigurationData.Get("ETL.Execution.Minutes", 5);

        timer = new Timer(ExecuteETL, null, 10 * 1000,
                          MESSAGE_ENGINE_EXECUTION_MINUTES * 60 * 1000);

        isRunning = true;
        EmpiriaLog.Info("ETLServiceInvoker was started.");

      } catch (Exception e) {
        EmpiriaLog.Info("ETLServiceInvoker was stopped due to an ocurred exception.");

        EmpiriaLog.Error(e);

        isRunning = false;
      }
    }


    /// <summary>Stops the execution engine.</summary>
    static public void Stop() {
      if (!isRunning) {
        return;
      }
      timer.Dispose();
      timer = null;
      isRunning = false;

      EmpiriaLog.Info("ETLServiceInvoker was stopped.");
    }

    #endregion Engine methods


    #region Execution methods

    /// <summary>Executes ETL Service.</summary>
    static private async void ExecuteETL(object stateInfo) {

      var service = new ETLService();

      await service.ExecuteAll();

      EmpiriaLog.Info($"ETLServiceInvoker was executed.");
    }

    # endregion Execution methods

  }  // class ETLServiceInvoker

}  // namespace Empiria.Land.Messaging
