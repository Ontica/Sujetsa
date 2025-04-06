/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Integration Layer                     *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Information holder                    *
*  Type     : OrderReturnNK                               License   : Please read LICENSE.txt file           *
*                                                                                                            *
*  Summary  : A row in Order(Devolucion) NK table.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>A row in Order(Devolucion) NK table.</summary>
  internal class OrderReturnNK {

    [DataField("DEVOLUCION")]
    internal string Devolucion {
      get; set;
    }

    [DataField("APLICADA")]
    internal string Aplicada {
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

    [DataField("FACTURA")]
    internal string Factura {
          get; set;
    }

    [DataField("ICMOV")]
    internal string ICMOV {
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
   
    [DataField("REFERENCIA")]
    internal string Referencia {
            get; set;
    }

    [DataField("CONCEPTO")]
    internal string Concepto {
            get; set;
    }

    [DataField("ESTATUS")]
    internal string Estatus {
            get; set;
    }

    [DataField("NOTACREDITO")]
    internal string NotaCredito {
            get; set;
    }

    [DataField("FOLIO")]
    internal string Folio {
            get; set;
    }

    [DataField("SERIE")]
    internal string Serie {
            get; set;
    }

    [DataField("USUARIO_CANCELO")]
    internal string Usuario_Cancelo {
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

    [DataField("APLICAR_CARGOFACTURA")]
    internal string Aplicar_Cargofactura {
            get; set;
    }

    [DataField("PCS")]
    internal string PCS {
            get; set;
    }

    [DataField("VENDEDOR")]
    internal string Vendedor {
            get; set;
    }

    [DataField("TELEMARKETER")]
    internal string Telemarketer {
            get; set;
    }

    [DataField("SAT_FORMAPAGO")]
    internal string SAT_Formapago {
            get; set;
    }

    [DataField("TERMINADA")]
    internal string Terminada {
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


  }  // class OrderReturnNK

}  // namespace Empiria.Trade.Integration.ETL.Transformers
