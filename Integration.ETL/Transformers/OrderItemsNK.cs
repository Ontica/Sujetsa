/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Integration Layer                     *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Information holder                    *
*  Type     : OrderItemsNK                                    License   : Please read LICENSE.txt file       *
*                                                                                                            *
*  Summary  : A row in OrderItems(OVDET) NK table.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>A row in OrderItems(OVDET) NK table.</summary>
  internal class OrderItemsNK {

    [DataField("OV")]
    internal string OV {
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

    [DataField("UNIDAD")]
    internal string Unidad {
      get; set;
    }

    [DataField("CANTIDAD")]
    internal decimal Cantidad {
      get; set;
    }
    
    [DataField("PRECIO")]
    internal decimal Precio {
      get; set;
    }
  
    [DataField("DESCUENTO")]
    internal decimal Descuento {
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
  
    [DataField("BinaryChecksum")]
    internal int BinaryChecksum {
      get; set;
    }

    [DataField("OldBinaryChecksum")]
    internal int OldBinaryChecksum {
      get; set;
    }

  }  // class OrderItemsNK

}  // namespace Empiria.Trade.Integration.ETL.Transformers
