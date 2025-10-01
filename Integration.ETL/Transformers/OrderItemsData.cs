/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Integration Layer                     *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Information holder                    *
*  Type     : OrderItemsData                                    License   : Please read LICENSE.txt file     *
*                                                                                                            *
*  Summary  : Represents a Sales Order in Empiria Trade OMS_OrderItems database table.                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>Represents a Sales Order Detail (Item) in Empiria Trade OMS_OrderItems database table.</summary>
  public class OrderItemsData {

    [DataField("Order_Item_Location_Id")]
    public int Order_Item_Location_Id {
      get;
      internal set;
    }

    [DataField("Order_Item_Id")]
    internal int Order_Item_Id {
      get; set;
    }

    [DataField("Order_Item_UID")]
    internal string Order_Item_UID {
      get; set;
    }

    [DataField("Order_Item_Type_Id")]
    internal int Order_Item_Type_Id {
      get; set;
    }

    [DataField("Order_Item_Order_Id")]
    internal int Order_Item_Order_Id {
      get; set;
    }

    [DataField("Order_Item_Product_Id")]
    internal int Order_Item_Product_Id {
      get; set;
    }

    [DataField("Order_Item_Description")]
    internal string Order_Item_Description {
      get; set;
    }

    [DataField("Order_Item_Product_Unit_Id")]
    internal int Order_Item_Product_Unit_Id {
      get; set;
    }

    [DataField("Order_Item_Product_Qty")]
    internal decimal Order_Item_Product_Qty {
      get; set;
    }
        
    [DataField("Order_Item_Unit_Price")]
    internal decimal Order_Item_Unit_Price {
      get; set;
    }

    [DataField("Order_Item_Discount")]
    internal decimal Order_Item_Discount {
      get; set;
    }

    [DataField("Order_Item_Currency_Id")]
    internal int Order_Item_Currency_Id {
      get; set;
    }

    [DataField("Order_Item_Related_Item_Id")]
    internal int Order_Item_Related_Item_Id {
      get; set;
    }

    [DataField("Order_Item_Requisition_Item_Id")]
    internal int Order_Item_Requisition_Item_Id {
      get; set;
    }

    [DataField("Order_Item_Requested_By_Id")]
    internal int Order_Item_Requested_By_Id {
      get; set;
    }

    [DataField("Order_Item_Budget_Account_Id")]
    internal int Order_Item_Budget_Account_Id {
      get; set;
    }

    [DataField("Order_Item_Project_Id")]
    internal int Order_Item_Project_Id {
      get; set;
    }

    [DataField("Order_Item_Provider_Id")]
    internal int Order_Item_Provider_Id {
      get; set;
    }

    [DataField("[Order_Item_Per_Each_Item_Id]")]
    internal int Order_Item_Per_Each_Item_Id {
      get; set;
    }
  
    [DataField("Order_Item_Ext_Data")]
    internal string Order_Item_Ext_Data {
      get; set;
    }

    [DataField("Order_Item_Keywords")]
    internal string Order_Item_Keywords {
      get; set;
    }

    [DataField("Order_Item_Position")]
    internal int Order_Item_Position {
      get; set;
    }

    [DataField("Order_Item_Posted_By_Id")]
    internal int Order_Item_Posted_By_Id {
      get; set;
    }

    [DataField("Order_Item_Posting_Time")]
    internal DateTime Order_Item_Posting_Time {
      get; set;
    }

    [DataField("Order_Item_Status")]
    internal char Order_Item_Status {
      get; set;
    }

  }  // class OrderData

}  // namespace Empiria.Trade.Integration.ETL.Transformers
