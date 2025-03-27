/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Integration Layer                     *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Information holder                    *
*  Type     : PartyNK                                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : A row in Cliente NK table.                                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>A row in Cliente NK table.</summary>
  internal class PartyUsersNK {

    [DataField("BinaryChecksum")]
    internal int BinaryChecksum {
      get; set;
    }

    [DataField("OldBinaryChecksum")]
    internal int OldBinaryChecksum {
      get; set;
    }

    [DataField("USUARIO")]
    internal string Usuario {
      get; set;
    }
    
    [DataField("NOMBRE")]
    internal string Nombre {
      get; set;
    }
    
    [DataField("PERFIL")]
    internal string Perfil {
      get; set;
    }

  }  // class PartyUserNK

}  // namespace Empiria.Trade.Integration.ETL.Transformers
