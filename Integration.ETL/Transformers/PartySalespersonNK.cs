/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Integration Layer                     *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Information holder                    *
*  Type     : PartySalespersonNK                           License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : A row in Vendedor NK table.                                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>A row in Vendedor NK table.</summary>
  internal class PartySalespersonNK {

    [DataField("BinaryChecksum")]
    internal int BinaryChecksum {
      get; set;
    }

    [DataField("OldBinaryChecksum")]
    internal int OldBinaryChecksum {
      get; set;
    }

    [DataField("VENDEDOR")]
    internal string Vendedor {
      get; set;
    }
    
    [DataField("NOMBRE")]
    internal string Nombre {
      get; set;
    }
    
    [DataField("TELEFONO")]
    internal string Telefono {
      get; set;
    }

    [DataField("EMAIL")]
    internal string Email {
      get; set;
    }

    [DataField("ACTIVO")]
    internal string Activo {
      get; set;
    }

  }  // class PartySalespersonNK

}  // namespace Empiria.Trade.Integration.ETL.Transformers
