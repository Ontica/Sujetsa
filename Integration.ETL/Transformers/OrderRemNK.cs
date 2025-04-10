/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Integration Layer                     *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Information holder                    *
*  Type     : OrderRemNK                               License   : Please read LICENSE.txt file              *
*                                                                                                            *
*  Summary  : A row in Order(Remision) NK table.                                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>A row in Order(Remision) NK table.</summary>
  internal class OrderRemNK {

    [DataField("REML")]
    internal string Reml {
      get; set;
    }

    [DataField("FECHA")]
    internal DateTime Fecha {
      get; set;
    }

    [DataField("ALMACEN")]
    internal string Almacen {
      get; set;
    }

    [DataField("USUARIO")]
    internal string Usuario {
      get; set;
    }

    [DataField("CLIENTE")]
    internal string Cliente {
      get; set;
    }
   
    [DataField("APLICADO")]
    internal string Aplicado {
      get; set;
    }

    [DataField("FECHA_APLICADO")]
    internal DateTime Fecha_Aplicado {
      get; set;
    }   

    [DataField("ICMOV")]
    internal string Icmov {
     get; set;
    }

    [DataField("CANCELADO")]
    internal string Cancelado {
      get; set;
    }

    [DataField("FECHA_CANCELACION")]
    internal DateTime Fecha_Cancelacion {
      get; set;
    }

    [DataField("ICMOV_CANCELACION")]
    internal string Icmov_Cancelacion {
      get; set;
    }

    [DataField("VENDEDOR")]
    internal string Vendedor {
      get; set;
    }

    [DataField("PAGADA")]
    internal string Pagada {
      get; set;
    }

    [DataField("USUARIO_PAGADA")]
    internal string Usuario_Pagada {
     get; set;
    }

    [DataField("FECHA_PAGADA")]
    internal DateTime Fecha_Pagada {
     get; set;
    }

    [DataField("REMLTIPO")]
    internal string RemlTipo {
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


  }  // class OrderRemNK

}  // namespace Empiria.Trade.Integration.ETL.Transformers
