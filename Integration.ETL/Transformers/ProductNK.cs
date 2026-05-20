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
    internal decimal CostoBase {
      get; set;
    } = 0.00000m;

    [DataField("BinaryChecksum")]
    internal int BinaryChecksum {
      get; set;
    }

    [DataField("OldBinaryChecksum")]
    internal int OldBinaryChecksum {
      get; set;
    }

    [DataField("LINEA")]
    internal string Linea {
      get; set;
    }

    [DataField("LINEA_NOMBRE")]
    internal string LineaNombre {
      get; set;
    }

    [DataField("MARCA")]
    internal string Marca {
      get; set;
    }

    [DataField("MARCA_NOMBRE")]
    internal string MarcaNombre {
      get; set;
    }

    [DataField("CATEGORIA")]
    internal string Categoria {
      get; set;
    }

    [DataField("COLOR")]
    internal string Color {
      get; set;
    }

    [DataField("COLOR_NOMBRE")]
    internal string ColorNombre {
      get; set;
    }

    [DataField("TALLA")]
    internal string Talla {
      get; set;
    }

    [DataField("TALLA_NOMBRE")]
    internal string TallaNombre {
      get; set;
    }

    [DataField("DIAMETRO")]
    internal string Usr1 {
      get; set;
    }

    [DataField("LARGO")]
    internal string Usr2 {
      get; set;
    }

    [DataField("PESO")]
    internal string Peso {
      get; set;
    }

    [DataField("MODELO")]
    internal string Modelo {
      get; set;
    }

    [DataField("MODELO_NOMBRE")]
    internal string ModeloNombre {
      get; set;
    }

    [DataField("SECCION")]
    internal string Seccion {
      get; set;
    }

    [DataField("SECCION_NOMBRE")]
    internal string SeccionNombre {
      get; set;
    }

  }  // class ProductNK

}  // namespace Empiria.Trade.Integration.ETL.Transformers
