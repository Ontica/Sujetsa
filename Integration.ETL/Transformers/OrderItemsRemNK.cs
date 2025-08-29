/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Integration Layer                     *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Information holder                    *
*  Type     : OrderItemsRemNK                         License   : Please read LICENSE.txt file               *
*                                                                                                            *
*  Summary  : A row in OrderItems(Remision Reml) NK table.                                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>A row in OrderItems(Remision) NK table.</summary>
  internal class OrderItemsRemNK {

    [DataField("REML")]
    internal string Reml {
      get; set;
    }

    [DataField("DET")]
    internal int Det {
      get; set;
    }
    
    [DataField("PRODUCTO")]
    internal string Producto {
      get; set;
    }

    [DataField("CANTIDAD")]
    internal decimal Cantidad {
      get; set;
    }

    [DataField("UNIDAD")]
    internal string Unidad {
      get; set;
    }
       
    [DataField("PRECIO_LISTA")]
    internal decimal Precio_Lista {
      get; set;
    }
  
    [DataField("DESCUENTO")]
    internal string Descuento {
      get; set;
    }

    [DataField("PRECIO")]
    internal decimal Precio {
      get; set;
    }

    [DataField("TOTAL")]
    internal decimal Total {
      get; set;
    }

    [DataField("DISPONIBLE")]
    internal decimal Disponible {
      get; set;
    }

    [DataField("ESTADO")]
    internal string Estado {
      get; set;
    }

    [DataField("REFERENCIA")]
    internal string Referencia {
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

  }  // class OrderItemsRemNK

}  // namespace Empiria.Trade.Integration.ETL.Transformers
