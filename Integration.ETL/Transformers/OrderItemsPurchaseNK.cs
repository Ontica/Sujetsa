/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Integration Layer                     *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Information holder                    *
*  Type     : OrderItemsPurchaseNK                         License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : A row in OrderItems(OVDET) NK table.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>A row in OrderItems(OVDET) NK table.</summary>
  internal class OrderItemsPurchaseNK {

    [DataField("COMPRA")]
    internal string Compra {
      get; set;
    }

    [DataField("DET")]
    internal int Det {
      get; set;
    }
    
    [DataField("CLAVE")]
    internal string Clave {
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
  
    [DataField("COSTO")]
    internal decimal Costo {
      get; set;
    }

    [DataField("SUBTOTAL")]
    internal decimal SubTotal {
      get; set;
    }

    [DataField("DESCUENTOS")]
    internal string Descuentos {
      get; set;
    }

    [DataField("IVA")]
    internal decimal IVA {
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

  }  // class OrderItemsPurchaseNK

}  // namespace Empiria.Trade.Integration.ETL.Transformers
