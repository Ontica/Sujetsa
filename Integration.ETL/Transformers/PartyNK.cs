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
  internal class PartyNK {

    [DataField("CLIENTE")]
    internal string Cliente {
      get; set;
    }

    [DataField("NOMBRE")]
    internal string Nombre {
      get; set;
    }

    [DataField("TSM")]
    internal string TSM {
      get; set;
    }

    [DataField("RFC")]
    internal string RFC {
      get; set;
    }

    [DataField("CALLE")]
    internal string Calle {
      get; set;
    }

    [DataField("COLONIA")]
    internal string Colonia {
      get; set;
    }

    [DataField("CP")]
    internal string CP {
      get; set;
    }

    [DataField("TIPOCLIENTE")]
    internal string TipoCliente {
      get; set;
    }

    [DataField("FORMAENTREGA")]
    internal string FormaEntrega {
      get; set;
    }

    [DataField("MONEDA")]
    internal string Moneda {
      get; set;
    }

    [DataField("CONDICIONDEPAGO")]
    internal string CondicionDePago {
      get; set;
    }

    [DataField("TELEFONO1")]
    internal string Telefono1 {
      get; set;
    }

    [DataField("TELEFONO2")]
    internal string Telefono2 {
      get; set;
    }

    [DataField("FECHA_ALTA")]
    internal DateTime Fecha_Alta {
      get; set;
    }

    [DataField("LOCALIDAD")]
    internal string Localidad {
      get; set;
    }

    [DataField("CIUDAD1")]
    internal string Ciudad1 {
      get; set;
    }

    [DataField("ESTADO1")]
    internal string Estado1 {
      get; set;
    }

    [DataField("PAIS1")]
    internal string Pais1 {
      get; set;
    }

    [DataField("GERENCIA")]
    internal string Gerencia {
      get; set;
    }

    [DataField("NUMERO")]
    internal string Numero {
      get; set;
    }

    [DataField("NUMERO_INTERIOR")]
    internal string Numero_Interior {
      get; set;
    }

    [DataField("MUNICIPIO")]
    internal string Municipio {
      get; set;
    }

    [DataField("METODODEPAGO")]
    internal string MetodoDePago {
      get; set;
    }

    [DataField("NUMCTAPAGO")]
    internal string NumCtaPago {
      get; set;
    }

    [DataField("NOMBRE_COMERCIAL")]
    internal string Nombre_Comercial {
      get; set;
    }

    [DataField("SAT_PAIS")]
    internal string SAT_pais {
      get; set;
    }

    [DataField("SAT_USOCFDI")]
    internal string SAT_UsoCFDI {
      get; set;
    }

    [DataField("SAT_METODOPAGO")]
    internal string SAT_MetodoPago {
      get; set;
    }

    [DataField("SAT_FORMAPAGO")]
    internal string SAT_FormaPago {
      get; set;
    }

    [DataField("COMPLEMENTOPAGO_PORPAGO")]
    internal string ComplementoPago_PorPago {
      get; set;
    }

    [DataField("SAT_REGIMENFISCAL")]
    internal string SAT_RegimenFiscal {
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

  }  // class PartyNK

}  // namespace Empiria.Trade.Integration.ETL.Transformers
