/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Integration Layer                     *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Information holder                    *
*  Type     : OrderNK                                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : A row in Order(OV) NK table.                                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>A row in Order(OV) NK table.</summary>
  internal class OrderNK {

    [DataField("OV")]
    internal string OV {
      get; set;
    }

    [DataField("FECHA")]
    internal DateTime Fecha {
      get; set;
    }
    /*
    [DataField("FECHA_ENTREGA")]
    internal DateTime Fecha_Entrega {
      get; set;
    }

    [DataField("FECHA_PROMESA")]
    internal DateTime Fecha_Promesa {
      get; set;
    }*/

    [DataField("CLIENTE")]
    internal string Cliente {
      get; set;
    }

    [DataField("SUBCLIENTE")]
    internal string SubCliente {
      get; set;
    }
    /*
    [DataField("ENTREGA")]
    internal int Entrega {
      get; set;
    }*/

    [DataField("ALMACEN")]
    internal string Almacen {
      get; set;
    }

    [DataField("VENDEDOR")]
    internal string Vendedor {
      get; set;
    }
    /*
    [DataField("LISTAPRECIOS")]
    internal int ListaPrecios {
      get; set;
    }*/

    [DataField("MONEDA")]
    internal string Moneda {
      get; set;
    }
    /*
    [DataField("CONDICIONDEPAGO")]
    internal string CondicionDePago {
      get; set;
    }

    [DataField("VIAEMBARQUE")]
    internal string ViaEmbarque {
      get; set;
    }

    [DataField("BACKORDER")]
    internal string BackOrder {
      get; set;
    }*/

    [DataField("PRIORIDAD")]
    internal string Prioridad {
      get; set;
    }

    [DataField("ORDEN")]
    internal string Orden {
      get; set;
    }
    /*
    [DataField("SUBTOTAL")]
    internal decimal SubTotal {
      get; set;
    }

    [DataField("IVA")]
    internal decimal IVA {
      get; set;
    }

    [DataField("TOTAL")]
    internal decimal Total {
      get; set;
    }*/

    [DataField("APLICADO")]
    internal string Aplicado {
      get; set;
    }

    [DataField("CANCELADO")]
    internal string Cancelado {
      get; set;
    }

    [DataField("ESTATUS")]
    internal string Estatus {
      get; set;
    }
    /*
    [DataField("VENCIMIENTOS")]
    internal string Vencimientos {
      get; set;
    }*/
    /*
    [DataField("IEPS")]
    internal decimal Ieps {
      get; set;
    }*/
    /*
    [DataField("ESTATUS_CREDITO")]
    internal string Estatus_Credito {
      get; set;
    }
    *//*
    [DataField("USUARIO_APLICO")]
    internal string Usuario_Aplico {
      get; set;
    }

    [DataField("FECHA_APLICO")]
    internal DateTime Fecha_Aplico {
      get; set;
    }

    [DataField("INI_CAPTURA")]
    internal DateTime Ini_Captura {
      get; set;
    }

    [DataField("USR_AUTORIZA")]
    internal string Usr_Autoriza {
      get; set;
    }

    [DataField("FIN_AUTORIZA")]
    internal DateTime Fin_Autoriza {
      get; set;
    }*/

    [DataField("USR_CAPTURA")]
    internal string Usr_Captura {
      get; set;
    }
    /*
    [DataField("TELEMARKETER")]
    internal string Telemarketer {
      get; set;
    }

    [DataField("FV_FIJA")]
    internal DateTime Fv_Fija {
      get; set;
    }
    
    [DataField("USOCFDI")]
    internal string UsoCFDI {
      get; set;
    }

    [DataField("SAT_METODOPAGO")]
    internal string SAT_Metodopago {
      get; set;
    }

    [DataField("SAT_FORMAPAGO")]
    internal string SAT_Formapago {
      get; set;
    }
    
    [DataField("PROMOVENTA")]
    internal string Promoventa {
      get; set;
    }
    
    [DataField("SAT_REGIMENFISCAL")]
    internal string SAT_Regimenfiscal {
      get; set;
    }
    */
    [DataField("BinaryChecksum")]
    internal int BinaryChecksum {
      get; set;
    }

    [DataField("OldBinaryChecksum")]
    internal int OldBinaryChecksum {
      get; set;
    }
    /*
    [DataField("FECHA_CIERRE")]
    internal DateTime Fecha_Cierre {
      get; set;
    }

    [DataField("USUARIO_CIERRE")]
    internal string Usr_Cierre {
      get; set;
    }*/

  }  // class OrderNK

}  // namespace Empiria.Trade.Integration.ETL.Transformers
