/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Integration Layer                     *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Information holder                    *
*  Type     : ProductData                                  License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents a Product in Empiria Trade OMS_Products database table.                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>Represents a Product in Empiria Trade OMS_Products database table.</summary>
  internal class ProductData {

    internal int ProductId {
      get; set;
    }

    internal string ProductCode {
      get; set;
    }

    internal string Name {
      get; set;
    }


    internal DateTime StartDate {
      get; set;
    }


    internal DateTime EndDate {
      get; set;
    }

  }  // class ProductData

}  // namespace Empiria.Trade.Integration.ETL.Transformers
