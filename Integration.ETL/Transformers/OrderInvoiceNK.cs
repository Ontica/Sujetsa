/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Integration Layer                     *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Information holder                    *
*  Type     : OrderInvoiceNK                               License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : A row in Order(Factura) NK table.                                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>A row in Order(Factura) NK table.</summary>
  internal class OrderInvoiceNK {

    [DataField("FACTURA")]
    internal string Factura {
      get; set;
    }

    [DataField("CLIENTE")]
    internal string Cliente {
      get; set;
    }

    [DataField("DESCUENTO")]
    internal decimal Descuento {
      get; set;
    }

    [DataField("SUBTOTAL")]
    internal decimal SubTotal {
      get; set;
    }
   
    [DataField("TOTAL")]
    internal decimal Total {
      get; set;
    }

    [DataField("USUARIO")]
    internal string Usuario
    {
          get; set;
    }

    [DataField("FECHA")]
    internal DateTime Fecha
    {
          get; set;
    }

    [DataField("PEDIDO")]
    internal string Pedido
    {
          get; set;
    }

    [DataField("APLICADA")]
    internal string Aplicada
    {
          get; set;
    }

    [DataField("VENDEDOR")]
    internal string Vendedor
    {
          get; set;
    }

    [DataField("OV")]
    internal string Ov
    {
            get; set;
    }
   
    [DataField("ALMACEN")]
    internal string Almacen                    {
            get; set;
    }

    [DataField("ICMOV")]
    internal string Icmov
    {
            get; set;
    }

    [DataField("CONDICIONDEPAGO")]
    internal string Condiciondepago
    {
            get; set;
    }


    [DataField("FECHA_ENTREGA")]
    internal DateTime FechaEntrega
    {
            get; set;
    }

    [DataField("ENTREGA")]
    internal int Entrega
    {
            get; set;
    }

    [DataField("SUBCLIENTE")]
    internal string SubCliente
    {
            get; set;
    }

    [DataField("VENCIMIENTOS")]
    internal string Vencimientos
    {
            get; set;
    }

    [DataField("POLIZA")]
    internal string Poliza
    {
            get; set;
    }

    [DataField("SINC_S")]
    internal string Sinc_s
    {
            get; set;
    }    

    [DataField("OC")]
    internal string Oc
    {
            get; set;
    }

    [DataField("ESTATUS")]
    internal string Estatus
    {
            get; set;
    }

    [DataField("RUTA")]
    internal string Ruta
    {
            get; set;
    }

    [DataField("SUBRUTA")]
    internal int Subruta
    {
            get; set;
    }

    [DataField("DESGLOSA_IEPS")]
    internal string Desglosa_ieps
    {
            get; set;
    }

    [DataField("USUARIOCANCELO")]
    internal string UsuarioCancelo
    {
            get; set;
    }

    [DataField("FECHACANCELO")]
    internal DateTime FechaCancelo
    {
            get; set;
    }

    [DataField("CANCELADA")]
    internal string Cancelada {
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
   

  }  // class OrderInvoiceNK

}  // namespace Empiria.Trade.Integration.ETL.Transformers
