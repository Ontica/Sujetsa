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
    internal string Tipo {
      get; set;
    }

    [DataField("TIPOPAGO")]
    internal string TipoPago {
      get; set;
    }

    [DataField("FR")]
    internal string Fr {
      get; set;
    }

    [DataField("CLIENTE")]
    internal string Cliente {
      get; set;
    }

    [DataField("FORMAPAGO")]
    internal string FormaPago {
      get; set;
    }

    [DataField("MONEDA")]
    internal string Moneda {
      get; set;
    }

  
    [DataField("TIPOCAMBIO")]
    internal decimal TipoCambio {
      get; set;
    }

    [DataField("IMPORTE")]
    internal decimal Importe {
      get; set;
    }

    [DataField("CARGOS")]
    internal decimal Cargos {
      get; set;
    }

    [DataField("DESCUENTO")]
    internal decimal Descuento {
      get; set;
    }

    [DataField("SUBTOTAL")]
    internal decimal SubTotal {
      get; set;
    }

    [DataField("IVA")]
    internal decimal IVA {
      get; set;
    }

    [DataField("R_IVA")]
    internal decimal R_IVA {
      get; set;
    }

    [DataField("R_ISR")]
    internal decimal R_ISR {
      get; set;
    }

    [DataField("TOTAL")]
    internal decimal Total {
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
