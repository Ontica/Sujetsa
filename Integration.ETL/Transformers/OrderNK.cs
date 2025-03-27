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
 
    [DataField("CLIENTE")]
    internal string Cliente {
      get; set;
    }

    [DataField("SUBCLIENTE")]
    internal string SubCliente {
      get; set;
    }
 
    [DataField("ALMACEN")]
    internal string Almacen {
      get; set;
    }

    [DataField("VENDEDOR")]
    internal string Vendedor {
      get; set;
    }
  
    [DataField("MONEDA")]
    internal string Moneda {
      get; set;
    }
 
    [DataField("PRIORIDAD")]
    internal string Prioridad {
      get; set;
    }

    [DataField("ORDEN")]
    internal string Orden {
      get; set;
    }
 
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
  
    [DataField("USR_CAPTURA")]
    internal string Usr_Captura {
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
  
  }  // class OrderNK

}  // namespace Empiria.Trade.Integration.ETL.Transformers
