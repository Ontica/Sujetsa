/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Integration Layer                     *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Information holder                    *
*  Type     : ProductNK                                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : A row in Product NK table.                                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>A row in Product NK table.</summary>
  internal class ProductNK {

    [DataField("PRODUCTO")]
    internal string Producto {
      get; set;
    }

    [DataField("DESCRIPCION")]
    internal string Descripcion {
      get; set;
    }

    [DataField("GRUPO")]
    internal string Grupo {
      get; set;
    }

    [DataField("SUBGRUPO")]
    internal string SubGrupo {
      get; set;
    }

    [DataField("UNIDAD")]
    internal string UnidadMedida {
      get; set;
    }

    [DataField("ALTA")]
    internal DateTime FechaAlta {
      get; set;
    }

    [DataField("BAJA")]
    internal string StatusProducto {
      get; set;
    }

    [DataField("EMPAQUE")]
    internal int Empaque {
      get; set;
    }

    [DataField("COSTO_BASE")]
    internal decimal Costo_Base {
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
    
  }  // class ProductNK

}  // namespace Empiria.Trade.Integration.ETL.Transformers
