/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Integration Layer                     *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Information holder                    *
*  Type     : OrderInvoiceNK                               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : A row in Order(Factura) NK table.                                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>A row in Order(Factura) NK table.</summary>
  internal class OrderInvoiceNK {

    [DataField("FACTURA")]
    internal string Factura {
      get; set;
    }

    [DataField("TIPO")]
    internal DateTime Tipo {
      get; set;
    }
  



    [DataField("BinaryChecksum")]
    internal int BinaryChecksum {
      get; set;
    }

    [DataField("OldBinaryChecksum")]
    internal int OldBinaryChecksum {
      get; set;
    }
   

  }  // class OrderInvoiceNK

}  // namespace Empiria.Trade.Integration.ETL.Transformers
