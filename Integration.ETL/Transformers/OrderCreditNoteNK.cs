/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Integration Layer                     *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Information holder                    *
*  Type     : OrderCreditNoteNK                            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : A row in Order(NOTACREDITO) NK table.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>A row in Order(NOTACREDITO) NK table.</summary>
  internal class OrderCreditNoteNK {

    [DataField("NOTACREDITO")]
    internal string NotaCredito {
      get; set;
    }

    [DataField("TIPO")]
    internal string  Tipo {
      get; set;
    }

    [DataField("TIPOPAGO")]
    internal string TipoPago {
      get; set;
    }

    [DataField("TIPO_NC")]
    internal string Tipo_NC {
      get; set;
    }

    [DataField("FECHA")]
    internal DateTime Fecha {
      get; set;
    }

    [DataField("CLIENTE")]
    internal string Cliente {
      get; set;
    }

    [DataField("USUARIO")]
    internal string Usuario {
      get; set;
    }


    [DataField("OBSERVACIONES")]
    internal string Observaciones {
      get; set;
    }
   
    [DataField("CANCELADA")]
    internal string Cancelada {
      get; set;
    }

    [DataField("FECHACAPTURA")]
    internal DateTime FechaCaptura {
          get; set;
    }   

    [DataField("TIPOVENTA")]
    internal string TipoVenta {
          get; set;
    }

    [DataField("FACTURA")]
    internal string Factura {
      get; set;
    }

    [DataField("CONCEPTODESC")]
    internal string Conceptodesc {
          get; set;
    }

    [DataField("POLIZA")]
    internal string Poliza {
          get; set;
    }

    [DataField("ICMOV")]
    internal string Icmov {
            get; set;
    }
   
    [DataField("CBPAGO")]
    internal string Cbpago {
            get; set;
    }

    [DataField("SINC_S")]
    internal string Sinc_s {
            get; set;
    }

    [DataField("SUBCLIENTE")]
    internal string SubCliente {
            get; set;
    }

    [DataField("MOTIVODEVOLUCION")]
    internal string MotivoDevolucion {
            get; set;
    }

    [DataField("ALMACEN")]
    internal string Almacen {
            get; set;
    }

    [DataField("DEVOLUCION")]
    internal string Devolucion {
            get; set;
    }

    [DataField("USUARIOCANCELO")]
    internal string UsuarioCancelo {
            get; set;
    }

    [DataField("FECHA_CANCELO")]
    internal DateTime Fecha_Cancelo {
            get; set;
    }

    [DataField("MD")]
    internal string MD {
            get; set;
    }    

    [DataField("MONEDA")]
    internal string Moneda {
            get; set;
    }

    [DataField("SERIE")]
    internal string Serie {
            get; set;
    }

    [DataField("CONTADOR")]
    internal int Contador {
            get; set;
    }

    [DataField("CBTIPO")]
    internal string CBtipo {
            get; set;
    }

    [DataField("MOTIVODESCUENTO")]
    internal string MotivoDescuento {
            get; set;
    }

    [DataField("TRANSACCION")]
    internal int Transaccion {
            get; set;
    }

    [DataField("VENDEDOR")]
    internal string vendedor {
            get; set;
    }

    [DataField("TELEMARKETER")]
    internal string Telemarketer {
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


  }  // class OrderCreditNoteNK

}  // namespace Empiria.Trade.Integration.ETL.Transformers
