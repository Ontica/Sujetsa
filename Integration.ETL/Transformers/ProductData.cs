/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Integration Layer                     *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Information holder                    *
*  Type     : ProductData                                  License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Represents a Product in Empiria Trade OMS_Products database table.                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>Represents a Product in Empiria Trade OMS_Products database table.</summary>
  internal class ProductData {

    [DataField("Product_Id")]
    internal int Product_Id {
      get; set;
    }

    [DataField("Product_UID")]
    internal string Product_UID {
      get; set;
    }

    [DataField("Product_Type_Id")]
    internal int Product_Type_Id {
      get; set;
    }

    [DataField("Product_Category_Id")]
    internal int Product_Category_Id {
      get; set;
    }

    [DataField("Product_Name")]
    internal string Product_Name {
      get; set;
    }

    [DataField("Product_Description")]
    internal string Product_Description {
      get; set;
    }

    [DataField("Product_Internal_Code")]
    internal string Product_Internal_Code {
      get; set;
    }

    [DataField("Product_Identificators")]
    internal string Product_Identificators {
      get; set;
    }

    [DataField("Product_Roles")]
    internal string Product_Roles {
      get; set;
    }

    [DataField("Product_Tags")]
    internal string Product_Tags {
      get; set;
    }

    [DataField("Product_Attributes")]
    internal string Product_Attributes {
      get; set;
    }

    [DataField("Product_Base_Unit_Id")]
    internal int Product_Base_Unit_Id {
      get; set;
    }

    [DataField("Product_Manager_Id")]
    internal int Product_Manager_Id {
      get; set;
    }

    [DataField("Product_Ext_Data")]
    internal string Product_Ext_Data {
      get; set;
    }

    [DataField("Product_Keywords")]
    internal string Product_Keywords {
      get; set;
    }

    [DataField("Product_Start_Date")]
    internal DateTime Product_Start_Date {
      get; set;
    }

    [DataField("Product_End_Date")]
    internal DateTime Product_End_Date {
      get; set;
    }

    [DataField("Product_Historic_Id")]
    internal int Product_Historic_Id {
      get; set;
    }

    [DataField("Product_Posted_By_Id")]
    internal int Product_Posted_By_Id {
      get; set;
    }

    [DataField("Product_Posting_Time")]
    internal DateTime Product_Posting_Time {
      get; set;
    }

    [DataField("Product_Status")]
    internal char Product_Status {
      get; set;
    }

  }  // class ProductData

}  // namespace Empiria.Trade.Integration.ETL.Transformers
