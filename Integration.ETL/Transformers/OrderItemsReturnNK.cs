/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Integration Layer                     *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Information holder                    *
*  Type     : OrderItemsReturnNK                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : A row in OrderItems(DevolucionDet) NK table.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>A row in OrderItems(DevolucionDet) NK table.</summary>
  internal class OrderItemsReturnNK {

    [DataField("DEVOLUCION")]
    internal string Devolucion {
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

    [DataField("PRODUCTO")]
    internal string Producto {
      get; set;
    }

    [DataField("PRECIO")]
    internal decimal Precio {
      get; set;
    }
       
    [DataField("CLAVE")]
    internal string Clave {
      get; set;
    }
    
    [DataField("UNIDAD")]
    internal string Unidad {
      get; set;
    }
  
    [DataField("DESCUENTOS")]
    internal string Descuentos {
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
