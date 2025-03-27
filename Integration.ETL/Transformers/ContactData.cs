/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Integration Layer                     *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Information holder                    *
*  Type     : ContactData                                    License   : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Represents a Contact in Empiria Trade Contacts database table.                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>Represents a Contact in Empiria Trade Contacts database table.</summary>
  internal class ContactData {

    [DataField("ContactId")]
    internal int ContactId {
      get; set;
    }

    [DataField("ContactUID")]
    internal string ContactUID {
      get; set;
    }
    
    [DataField("ContactTypeId")]
    internal int ContactTypeId {
      get; set;
    }

    [DataField("ContactFullName")]
    internal string ContactFullName {
      get; set;
    }

    [DataField("ShortName")]
    internal string ShortName {
      get; set;
    }

    [DataField("Initials")]
    internal string Initials {
      get; set;
    }

    [DataField("OrganizationId")]
    internal int OrganizationId {
      get; set;
    }

    [DataField("ContactEmail")]
    internal string ContactEmail {
      get; set;
    }

    [DataField("ContactTags")]
    internal string ContactTags {
      get; set;
    }

    [DataField("ContactExtData")]
    internal string ContactExtData {
      get; set;
    }

    [DataField("ContactKeywords")]
    internal string ContactKeywords {
      get; set;
    }


    [DataField("ContactStatus")]
    internal char ContactStatus {
      get; set;
    }

   

  }  // class Contacts

}  // namespace Empiria.Trade.Integration.ETL.Transformers
