/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Integration Layer                     *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Information holder                    *
*  Type     : OrderItemsInvoiceNK                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : A row in OrderItems(OVDET) NK table.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>A row in OrderItems(OVDET) NK table.</summary>
  public class OrderItemsInvoiceNK {

    [DataField("FACTURA")]
    internal string Factura {
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
  
    [DataField("DESCUENTO")]
    internal decimal Descuento {
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
