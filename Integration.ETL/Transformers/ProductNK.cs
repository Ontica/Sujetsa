/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Integration Layer                     *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Information holder                    *
*  Type     : ProductNK                                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : A row in Product NK table.                                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>A row in Product NK table.</summary>
  internal class ProductNK {

    [DataField("Clave")]
    internal string Clave {
      get; set;
    }

    [DataField("NombreProducto")]
    internal string Nombre {
      get; set;
    }

    [DataField("Marca")]
    internal string Marca {
      get; set;
    }

    [DataField("Mdl")]
    internal string Modelo {
      get; set;
    }

    [DataField("OldChecksum")]
    public int OldChecksum {
      get; set;
    }

    [DataField("NewChecksum")]
    public int NewChecksum {
      get; set;
    }

  }  // class ProductNK

}  // namespace Empiria.Trade.Integration.ETL.Transformers
