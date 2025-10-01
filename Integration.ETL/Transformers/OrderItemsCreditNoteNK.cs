/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Integration Layer                     *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Information holder                    *
*  Type     : OrderItemsReturnNK                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : A row in OrderItems(NotaCreditoDet) NK table.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>A row in OrderItems(OVDET) NK table.</summary>
  public class OrderItemsCreditNoteNK {

    [DataField("NOTACREDITO")]
    internal string NotaCredito {
      get; set;
    }

    [DataField("DET")]
    internal int Det {
      get; set;
    }
    
    [DataField("CANTIDAD")]
    internal decimal Cantidad {
      get; set;
    }

    [DataField("UNIDAD_VENTA_MENUDEO_C")]
    internal decimal Unidad_Venta_Menudeo_c {
      get; set;
    }

    [DataField("UNIDAD")]
    internal string Unidad {
      get; set;
    }
       
    [DataField("PRODUCTO")]
    internal string Producto {
      get; set;
    }
    
    [DataField("PRECIO")]
    internal decimal Precio {
      get; set;
    }
  
    [DataField("IMPORTE")]
    internal decimal Importe {
      get; set;
    }

    [DataField("DESCRIPCION")]
    internal string Descripcion {
      get; set;
    }

    [DataField("CLAVEIMPUESTO")]
    internal string ClaveImpuesto {
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

  }  // class OrderItemsCreditNoteNK

}  // namespace Empiria.Trade.Integration.ETL.Transformers
