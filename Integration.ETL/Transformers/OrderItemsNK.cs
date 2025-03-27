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
    /*
    [DataField("CLAVE")]
    internal string Clave {
      get; set;
    }*/

    [DataField("PRODUCTO")]
    internal string Producto {
      get; set;
    }

    [DataField("UNIDAD")]
    internal string Unidad {
      get; set;
    }
    /*
    [DataField("EMPAQUE")]
    internal int Empaque {
      get; set;
    }*/
    
    [DataField("CANTIDAD")]
    internal decimal Cantidad {
      get; set;
    }
    
    [DataField("PRECIO")]
    internal decimal Precio {
      get; set;
    }
    /*
    [DataField("PRECIOV")]
    internal decimal Preciov {
      get; set;
    }*/
    /*
    [DataField("IMPORTE")]
    internal decimal Importe {
      get; set;
    }
    */
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
    /*
    [DataField("SUBTOTAL")]
    internal decimal SubTotal {
      get; set;
    }

    [DataField("IEPS")]
    internal decimal IEPS {
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

    [DataField("MARGEN")]
    internal decimal Margen {
      get; set;
    }*/
    /*
    [DataField("COMPROMETIDO")]
    internal decimal Comprometido {
      get; set;
    }*/
    /*
    [DataField("FACTURADO")]
    internal decimal Facturado {
      get; set;
    }*/
    /*
   [DataField("CORRECCION")]
    internal decimal Correccion {
      get; set;
    }
    */
    /*
    [DataField("FACTURAC")]
    internal int Facturac {
      get; set;
    }

    [DataField("NEGADOS")]
    internal decimal Negados {
      get; set;
    }*/
    /*
     [DataField("DESCUENTOSM")]
     internal string Descuentosm {
       get; set;
     }
    */
    /*
     [DataField("DESCUENTOS")]
     internal string Descuentos {
       get; set;
     }*/
    /*
     [DataField("EXISTENCIA")]
     internal decimal Existencia {
       get; set;
     }

     [DataField("DISPONIBLE")]
     internal decimal Disponible {
       get; set;
     }

     [DataField("PIVA")]
     internal decimal PIVA {
       get; set;
     }

     [DataField("PIEPS")]
     internal string PIEPS {
       get; set;
     }
    *//*
     [DataField("UBICACION")]
     internal string Ubicacion {
       get; set;
     }

     [DataField("PRECIO_NETO")]
     internal decimal Precio_Neto {
       get; set;
     }



     [DataField("PRECIO_MINIMO")]
     internal decimal Precio_Minimo {
       get; set;
     }

     [DataField("PRECIO_NETO_IVA")]
     internal decimal Precio_Neto_IVA {
       get; set;
     }*/
    /*
    [DataField("CLAVEIMPUESTO")]
    internal string ClaveImpuesto {
      get; set;
    }

    [DataField("MARGENV")]
    internal decimal Margenv {
      get; set;
    }*/

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
