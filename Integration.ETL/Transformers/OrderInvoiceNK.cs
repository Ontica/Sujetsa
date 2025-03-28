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

    [DataField("TIPO")]
    internal string Tipo {
      get; set;
    }

    [DataField("TIPOPAGO")]
    internal string TipoPago {
      get; set;
    }

    [DataField("FR")]
    internal string Fr {
      get; set;
    }

    [DataField("CLIENTE")]
    internal string Cliente {
      get; set;
    }

    [DataField("FORMAPAGO")]
    internal string FormaPago {
      get; set;
    }

    [DataField("MONEDA")]
    internal string Moneda {
      get; set;
    }

  
    [DataField("TIPOCAMBIO")]
    internal decimal TipoCambio {
      get; set;
    }

    [DataField("IMPORTE")]
    internal decimal Importe {
      get; set;
    }

    [DataField("CARGOS")]
    internal decimal Cargos {
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

    [DataField("IVA")]
    internal decimal IVA {
      get; set;
    }

    [DataField("R_IVA")]
    internal decimal R_IVA {
      get; set;
    }

    [DataField("R_ISR")]
    internal decimal R_ISR {
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

    [DataField("CANCELADA")]
    internal string Cancelada
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

    [DataField("IEPS")]
    internal decimal Ieps
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

    [DataField("CONSIGNACION")]
    internal string Consignacion
    {
            get; set;
    }

    [DataField("MD")]
    internal string Md
    {
            get; set;
    }
       
    [DataField("TCAMBIO")]
    internal Decimal Tcambio
    {
            get; set;
    }

    [DataField("IMPORTE_A")]
    internal Decimal Importe_a
    {
            get; set;
    }

    [DataField("DESCUENTO_A")]
    internal Decimal Descuento_a
    {
            get; set;
    }

    [DataField("SUBTOTAL_A")]
    internal Decimal Subtotal_a
    {
            get; set;
    }

    [DataField("IEPS_A")]
    internal Decimal Ieps_a
    {
            get; set;
    }

    [DataField("IVA_A")]
    internal Decimal Iva_a
    {
            get; set;
    }

    [DataField("R_IVA_A")]
    internal Decimal R_iva_a
    {
            get; set;
    }

    [DataField("R_ISR_A")]
    internal Decimal R_isr_a
    {
            get; set;
    }

    [DataField("TOTAL_A")]
    internal Decimal Total_a
    {
            get; set;
    }

    [DataField("REVERSE_DE")]
    internal string Reverse_de
    {
            get; set;
    }

    [DataField("SERIE")]
    internal string Serie
    {
            get; set;
    }

    [DataField("CONTADOR")]
    internal int Contador
    {
            get; set;
    }
     

    [DataField("HORA")]
    internal DateTime Hora
    {
            get; set;
    }    

    [DataField("CFD")]
    internal string Cfd
    {
            get; set;
    }
    [DataField("UBICACION")]
    internal string Ubicacion
    {
            get; set;
    }

    [DataField("CONSECUTIVO")]
    internal int Consecutivo
    {
            get; set;
    }

    [DataField("SALIDA")]
    internal int Salida
    {
            get; set;
    }

    [DataField("VER")]
    internal string Ver
    {
            get; set;
    }

    [DataField("TRANSACCION")]
    internal int Transaccion
    {
            get; set;
    }

    [DataField("TELEMARKETER")]
    internal string Telemarketer
    {
            get; set;
    }

    [DataField("USOCFDI")]
    internal string Usocfdi
    {
            get; set;
    }

    [DataField("SAT_METODOPAGO")]
    internal string Sat_metodopago
    {
            get; set;
    }

    [DataField("SAT_FORMAPAGO")]
    internal string Sat_formapago
    {
            get; set;
    }

    [DataField("SAT_REGIMENFISCAL")]
    internal string Sat_regimenfiscal
    {
            get; set;
    }

    [DataField("TIPOCAMBIOUSD")]
    internal decimal Tipocambiousd
    {
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
