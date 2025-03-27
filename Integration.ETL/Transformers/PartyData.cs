/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Integration Layer                     *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Information holder                    *
*  Type     : PartyData                                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents a Cliente in Empiria Trade Parties database table.                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>Represents a Sales Order in Empiria Trade OMS_Parties database table.</summary>
  internal class PartyData {

    [DataField("Party_Id")]
    internal int Party_Id {
      get; set;
    }

    [DataField("Party_UID")]
    internal string Party_UID {
      get; set;
    }
    
    [DataField("Party_Type_Id")]
    internal int Party_Type_Id {
      get; set;
    }

    [DataField("Party_Code")]
    internal string Party_Code {
      get; set;
    }

    [DataField("Party_Name")]
    internal string Party_Name {
      get; set;
    }

    [DataField("Party_Identificators")]
    internal string Party_Identificators {
      get; set;
    }

    [DataField("Party_Roles")]
    internal string Party_Roles {
      get; set;
    }

    [DataField("Party_Tags")]
    internal string Party_Tags {
      get; set;
    }

    [DataField("Party_Ext_Data")]
    internal string Party_Ext_Data {
      get; set;
    }

    [DataField("Party_Keywords")]
    internal string Party_Keywords {
      get; set;
    }

    [DataField("Party_Historic_Id")]
    internal int Party_Historic_Id {
      get; set;
    }

    [DataField("Party_Start_Date")]
    internal DateTime Party_Start_Date {
      get; set;
    }

    [DataField("Party_End_Date")]
    internal DateTime Party_End_Date {
      get; set;
    }

    [DataField("Party_Parent_Id")]
    internal int Party_Parent_Id {
      get; set;
    }

    [DataField("Party_Posted_By_Id")]
    internal int Party_Posted_By_Id {
      get; set;
    }

    [DataField("Party_Posting_Time")]
    internal DateTime Party_Posting_Time {
      get; set;
    }

    [DataField("Party_Status")]
    internal string Party_Status {
      get; set;
    }

    [DataField("Party_Contact_Id")]
    internal int Party_Contact_Id {
      get; set;
    }

  }  // class PartyData

}  // namespace Empiria.Trade.Integration.ETL.Transformers
