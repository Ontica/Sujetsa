/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Integration Layer                     *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Information holder                    *
*  Type     : OrderData                                    License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents a Sales Order in Empiria Trade OMS_Orders database table.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>Represents a Sales Order in Empiria Trade OMS_Orders database table.</summary>
  internal class OrderData {

    [DataField("Order_Location_Id")]
    internal int Order_Location_Id {
      get; set;
    }

    [DataField("Order_Id")]
    internal int Order_Id {
      get; set;
    }

    [DataField("Order_UID")]
    internal string Order_UID {
      get; set;
    }

    [DataField("Order_Type_Id")]
    internal int Order_Type_Id {
      get; set;
    }

    [DataField("Order_Category_Id")]
    internal int Order_Category_Id {
      get; set;
    }

    [DataField("Order_No")]
    internal string Order_No {
      get; set;
    }

    [DataField("Order_Description")]
    internal string Order_Description {
      get; set;
    }

    [DataField("Order_Identificators")]
    internal string Order_Identificators {
      get; set;
    }

    [DataField("Order_Tags")]
    internal string Order_Tags {
      get; set;
    }
        
    [DataField("Order_Requested_By_Id")]
    internal int Order_Requested_By_Id {
      get; set;
    }

    [DataField("Order_Responsible_Id")]
    internal int Order_Responsible_Id {
      get; set;
    }

    [DataField("Order_Beneficary_Id")]
    internal int Order_Beneficary_Id {
      get; set;
    }

    [DataField("Order_Provider_Id")]
    internal int Order_Provider_Id {
      get; set;
    }
    [DataField("Order_Budget_Id")]
    internal int Order_Budget_Id {
      get; set;
    }

    [DataField("Order_Requisition_Id")]
    internal int Order_Requisition_Id {
      get; set;
    }
    [DataField("Order_Contract_Id")]
    internal int Order_Contract_Id {
      get; set;
    }

    [DataField("Order_Project_Id")]
    internal int Order_Project_Id {
      get; set;
    }
    [DataField("Order_Currency_Id")]
    internal int Order_Currency_Id {
      get; set;
    }

    [DataField("Order_Source_Id")]
    internal int Order_Source_Id {
      get; set;
    }

    [DataField("Order_Priority")]
    internal char Order_Priority {
      get; set;
    }

    [DataField("Order_Authorization_Time")]
    internal DateTime Order_Authorization_Time {
      get; set;
    }

    [DataField("Order_Authorized_By_Id")]
    internal int Order_Authorized_By_Id {
      get; set;
    }

    [DataField("Order_Closing_Time")]
    internal DateTime Order_Closing_Time {
      get; set;
    }
    
    [DataField("Order_Closed_By_Id")]
    internal int Order_Closed_By_Id {
      get; set;
    }

    [DataField("Order_Ext_Data")]
    internal string Order_Ext_Data {
      get; set;
    }

    [DataField("Order_Keywords")]
    internal string Order_Keywords {
      get; set;
    }

    [DataField("Order_Posted_By_Id")]
    internal int Order_Posted_By_Id {
      get; set;
    }

    [DataField("Order_Posting_Time")]
    internal DateTime Order_Posting_Time {
      get; set;
    }

    [DataField("Order_Status")]
    internal char Order_Status {
      get; set;
    }

  }  // class OrderData

}  // namespace Empiria.Trade.Integration.ETL.Transformers
