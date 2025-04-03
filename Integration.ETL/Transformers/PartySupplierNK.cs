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
  internal class PartySupplierNK {

   [DataField("PROVEEDOR")]
    internal string Proveedor {
      get; set;
    }

    [DataField("NOMBRE_COMERCIAL")]
    internal string Nombre_Comercial {
      get; set;
    }

    [DataField("GRUPOPROVEEDOR")]
    internal string GrupoProveedor {
      get; set;
    }

    [DataField("RFC")]
    internal string RFC {
      get; set;
    }

    [DataField("NUMERO1")]
    internal string Numero1 {
      get; set;
    }

    [DataField("ESTADO1")]
    internal string Estado1 {
      get; set;
    }

    [DataField("COLONIA1")]
    internal string Colonia1 {
      get; set;
    }

    [DataField("TELEFONO1")]
    internal string Telefono1 {
      get; set;
    }

    [DataField("EMAIL1")]
    internal string Email1 {
      get; set;
    }

    [DataField("CIUDAD1")]
    internal string Ciudad1 {
      get; set;
    }

    [DataField("NOMBRE")]
    internal string Nombre {
      get; set;
    }

    [DataField("CP1")]
    internal string CP1 {
      get; set;
    }

    [DataField("CALLE1")]
    internal string Calle1 {
      get; set;
    }

    [DataField("FECHA_ALTA")]
    internal DateTime Fecha_Alta {
      get; set;
    }

    [DataField("MONEDA")]
    internal string Moneda {
      get; set;
    }

    [DataField("ESTATUS")]
    internal string Estatus {
      get; set;
    }

    [DataField("PAIS1")]
    internal string Pais1 {
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

  }  // class PartySalespersonNK

}  // namespace Empiria.Trade.Integration.ETL.Transformers
