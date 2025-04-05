/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Integration Layer                     *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Information holder                    *
*  Type     : OrderPurchaseNK                               License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : A row in Order(Factura) NK table.                                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>A row in Order(Compra) NK table.</summary>
  internal class OrderPurchaseNK {

    [DataField("COMPRA")]
    internal string Compra {
      get; set;
    }

    [DataField("FECHA")]
    internal DateTime Fecha {
      get; set;
    }

    [DataField("OC")]
    internal string OC {
      get; set;
    }

    [DataField("ALMACEN")]
    internal string Almacen {
      get; set;
    }

    [DataField("PROVEEDOR")]
    internal string Proveedor {
      get; set;
    }
   
    [DataField("FACTURA")]
    internal string Factura {
      get; set;
    }

    [DataField("FLETE")]
    internal decimal Flete {
          get; set;
    }   

    [DataField("PORCENTAJE_FLETE")]
    internal decimal Porcentaje_Flete {
          get; set;
    }

    [DataField("FECHA_RECEPCION")]
    internal DateTime Fecha_Recepcion {
          get; set;
    }

    [DataField("FECHA_FACTURAP")]
    internal DateTime Fecha_Facturap {
          get; set;
    }

    [DataField("FECHA_EMBARQUEP")]
    internal DateTime Fecha_Embarquep {
            get; set;
    }
   
    [DataField("REFERENCIA")]
    internal string Referencia {
            get; set;
    }

    [DataField("ICMOV")]
    internal string Icmov
    {
            get; set;
    }

    [DataField("VENCIMIENTOS")]
    internal string Vencimientos {
            get; set;
    }

    [DataField("APLICADO")]
    internal string Aplicado {
            get; set;
    }

    [DataField("CANCELADA")]
    internal string Cancelada {
            get; set;
    }

    [DataField("IMPRESO")]
    internal int Impreso {
            get; set;
    }

    [DataField("FECHA_PP")]
    internal DateTime Fecha_PP {
            get; set;
    }

    [DataField("PLAZO_EN")]
    internal string plazo_EN {
            get; set;
    }

    [DataField("CAPTURA")]
    internal DateTime Captura {
            get; set;
    }    

    [DataField("USUARIO")]
    internal string Usuario {
            get; set;
    }

    [DataField("F_RECEP_FACTORIGINAL")]
    internal DateTime F_Recep_Factoriginal {
            get; set;
    }

    [DataField("CLASE_DOCTO")]
    internal string Clase_Docto {
            get; set;
    }

    [DataField("APTIPO")]
    internal string Aptipo {
            get; set;
    }

    [DataField("MONEDA")]
    internal string Moneda {
            get; set;
    }

    [DataField("IMPORTACION")]
    internal string Importacion {
            get; set;
    }

    [DataField("CLAVEIMPUESTO_FLETE")]
    internal string ClaveImpuesto_Flete {
            get; set;
    }

    [DataField("SINC")]
    internal int Sinc {
      get; set;
    }

    [DataField("UTILIZAR_FV_FIJA")]
    internal string Utilizar_Fv_Fija {
      get; set;
    }

    [DataField("FV_FIJA")]
    internal DateTime Fv_Fija {
      get; set;
    }

    [DataField("CFDP")]
    internal string CFDP {
      get; set;
    }

    [DataField("TIPOCOMPRA")]
    internal string TipoCompra {
      get; set;
    }

    [DataField("SERIE")]
    internal string Serie {
      get; set;
    }

    [DataField("CONTADOR")]
    internal int Contador {
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


  }  // class OrderPurchaseNK

}  // namespace Empiria.Trade.Integration.ETL.Transformers
