﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeDataSchemaManager.Adapters;
using TradeDataSchemaManager.Services;

namespace TradeDataSchemaManager.Mapper {
  public class ProductosMapper {


    public List<ProductosAdapter> Map(DataTable dt, int connectionId) {

      List<ProductosAdapter> list = new List<ProductosAdapter>();

      if (connectionId == 1) {
        list = GetNkProductsList(dt, connectionId);
      }
      if (connectionId == 2) {
        list = GetMicrosipProductsList(dt, connectionId);
      }
      if (connectionId == 3) {
        list = GetHpNkProductsList(dt, connectionId);
      }

      return list;
    }


    private List<ProductosAdapter> GetHpNkProductsList(DataTable dt, int connectionId) {
      List<ProductosAdapter> listaProductos = new List<ProductosAdapter>();

      foreach (DataRow row in dt.Rows) {
        string fechaUltimaCompra = row["FECHA_ULTIMA_COMPRA"].ToString();
        DateTime fechaUltimaCompraDate = new DateTime(2077, 01, 01);

        if (fechaUltimaCompra != "") {
          fechaUltimaCompraDate = Convert.ToDateTime(fechaUltimaCompra);
        }

        ProductosAdapter prod = new ProductosAdapter();
        prod.COMPANIA_ID = connectionId;
        prod.DET = "1";
        prod.PRODUCTO = row["PRODUCTO"].ToString() ?? "";
        prod.CLAVEPRODSERV = row["CLAVEPRODSERV"].ToString() ?? "";
        prod.DESCRIPCION = row["DESCRIPCION"].ToString() ?? "";
        prod.DESC_LARGA = row["DESC_LARGA"].ToString() ?? "";
        prod.ALTA = (DateTime) row["ALTA"] != null ? (DateTime) row["ALTA"] : prod.ALTA;
        prod.MARCA = row["MARCA"].ToString() ?? "";
        prod.LINEA = row["LINEA"].ToString() ?? "";
        prod.NLINEA = row["NLINEA"].ToString() ?? "";
        prod.GRUPO = row["GRUPO"].ToString() ?? "";
        prod.NGRUPO = row["NGRUPO"].ToString() ?? "";
        prod.SUBGRUPO = row["SUBGRUPO"].ToString() ?? "";
        prod.COSTO_BASE = row["COSTO_BASE"].ToString() == "" ? 0 : (decimal) row["COSTO_BASE"];
        prod.FECHA_ULTIMA_COMPRA = fechaUltimaCompraDate;
        prod.COSTO_ULTIMA_COMPRA = row["COSTO_ULTIMA_COMPRA"].ToString() == "" ? 0 : (decimal) row["COSTO_ULTIMA_COMPRA"];
        prod.EXISTENCIA = row["EXISTENCIA"].ToString() ?? "";
        prod.PRECIOLISTA1 = row["PRECIO1"].ToString() == "" ? 0 : (decimal) row["PRECIO1"];
        prod.PRECIOLISTA10 = row["PRECIO10"].ToString() == "" ? 0 : (decimal) row["PRECIO10"];
        prod.EMPAQUE = row["EMPAQUE"].ToString() ?? "";
        prod.MULTIPLO_RESURTIDO = row["MULTIPLO_RESURTIDO"].ToString() ?? "";
        prod.PROVEEDOR = row["PROVEEDOR"].ToString() ?? "";
        prod.NPROVEEDOR = row["NPROVEEDOR"].ToString() ?? "";
        prod.TIPO = row["TIPO"].ToString() ?? "";
        prod.BAJA = row["BAJA"].ToString() ?? "";
        prod.CATEGORIA = row["CATEGORIA"].ToString() ?? "";
        prod.UNIDAD_COMPRA = row["UNIDAD_COMPRA"].ToString() ?? "";
        prod.UNIDAD_VENTA = row["UNIDAD_VENTA"].ToString() ?? "";

        listaProductos.Add(prod);
      }
      return listaProductos;
    }



    private List<ProductosAdapter> GetMicrosipProductsList(DataTable dt, int connectionId) {
      List<ProductosAdapter> listaProductos = new List<ProductosAdapter>();

      int index = 0;
      try {
        foreach (DataRow row in dt.Rows) {

          ProductosAdapter prod = new ProductosAdapter();
          prod.COMPANIA_ID = connectionId;
          prod.PRODUCTO = row["PRODUCTO"].ToString() ?? "";
          prod.DESCRIPCION = row["DESCRIPCION"].ToString() ?? "";
          prod.UNIDAD_COMPRA = row["unidad_compra"].ToString() ?? "";
          prod.UNIDAD_VENTA = row["unidad_venta"].ToString() ?? "";
          prod.CONTENIDO_UNIDAD_COMPRA = row["contenido_unidad_compra"].ToString() ?? "";
          prod.ES_ALMACENABLE = row["es_almacenable"].ToString() ?? "";
          prod.ES_IMPORTADO = row["es_importado"].ToString() ?? "";
          prod.ES_SIEMPRE_IMPORTADO = row["es_siempre_importado"].ToString() ?? "";
          prod.PESO_UNITARIO = row["peso_unitario"].ToString() == "" ? 0 : (decimal) row["peso_unitario"];
          prod.ESTATUS = row["estatus"].ToString() ?? "";
          prod.LINEA_ARTICULO_ID = row["linea_articulo_id"].ToString() ?? "";
          prod.NGRUPO = row["GRUPO"].ToString() ?? "";
          prod.EXISTENCIA = row["EXISTENCIA"].ToString() ?? "";
          prod.PRECIOLISTA7 = row["PRECIO7"].ToString() == "" ? 0 : (decimal) row["PRECIO7"];
          //prod.PRECIOLISTA2 = row["PRECIO_ESP_SUJETSA"].ToString() == "" ? 0 : (decimal) row["PRECIO_ESP_SUJETSA"];
          //prod.PRECIOLISTA3 = row["PRECIO_ESP_HERRAMIENTAS"].ToString() == "" ? 0 : (decimal) row["PRECIO_ESP_HERRAMIENTAS"];
          //prod.PRECIOLISTA4 = row["PRECIO_ESP_TTC"].ToString() == "" ? 0 : (decimal) row["PRECIO_ESP_TTC"];

          listaProductos.Add(prod);
          index++;
        }
      } catch (Exception ex) {

        throw new Exception($"Error in method GetNkProductsList, index: {index}. {ex.Message}");
      }

      return listaProductos;
    }


    private List<ProductosAdapter> GetNkProductsList(DataTable dt, int connectionId) {
      List<ProductosAdapter> listaProductos = new List<ProductosAdapter>();

      int index = 0;
      try {
        foreach (DataRow row in dt.Rows) {

          ProductosAdapter prod = new ProductosAdapter();
          prod.COMPANIA_ID = connectionId;
          prod.DET = "1";
          prod.PRODUCTO = row["PRODUCTO"].ToString() ?? "";
          prod.CLAVEPRODSERV = row["CLAVEPRODSERV"].ToString() ?? "";
          prod.DESCRIPCION = row["DESCRIPCION"].ToString() ?? "";
          prod.ALTA = (DateTime) row["ALTA"];
          prod.LINEA = row["LINEA"].ToString() ?? "";
          prod.NLINEA = row["NLINEA"].ToString() ?? "";
          prod.GRUPO = row["GRUPO"].ToString() ?? "";
          prod.NGRUPO = row["NGRUPO"].ToString() ?? "";
          prod.SUBGRUPO = row["SUBGRUPO"].ToString() ?? "";
          prod.NSUBGRUPO = row["NSUBGRUPO"].ToString() ?? "";
          prod.LARGO = row["LARGO"].ToString();
          prod.CABEZAS = row["CABEZAS"].ToString();
          prod.NCABEZAS = row["NCABEZAS"].ToString();
          prod.ACABADOS = row["ACABADOS"].ToString();
          prod.NACABADOS = row["NACABADOS"].ToString();
          prod.EXISTENCIA = row["EXISTENCIA"].ToString() ?? "";
          prod.UNIDAD_VENTA = row["UNIDAD_VENTA"].ToString() ?? "";
          prod.COSTO_BASE = row["COSTO_BASE"].ToString() == "" ? 0 : (decimal) row["COSTO_BASE"];
          prod.PRECIOLISTA1 = row["PLISTA_1"].ToString() == "" ? 0 : (decimal) row["PLISTA_1"];
          prod.PRECIOLISTA2 = row["PLISTA_2"].ToString() == "" ? 0 : (decimal) row["PLISTA_2"];
          prod.PRECIOLISTA3 = row["PLISTA_3"].ToString() == "" ? 0 : (decimal) row["PLISTA_3"];
          prod.PRECIOLISTA5 = row["PLISTA_5"].ToString() == "" ? 0 : (decimal) row["PLISTA_5"];
          prod.EMPAQUE = row["EMPAQUE"].ToString() ?? "";
          prod.MULTIPLO_RESURTIDO = row["MULTIPLO_RESURTIDO"].ToString() ?? "";
          prod.COSTO_ULTIMA_COMPRA = row["COSTO_ULTIMA_COMPRA"].ToString() == "" ? 0 : (decimal) row["COSTO_ULTIMA_COMPRA"];
          prod.PROVEEDOR = row["PROVEEDOR"].ToString() ?? "";
          prod.NPROVEEDOR = row["NPROVEEDOR"].ToString() ?? "";
          prod.TIPO = row["TIPO"].ToString() ?? "";
          prod.BAJA = row["BAJA"].ToString() ?? "";
          prod.PESO = row["PESO"].ToString() == "" ? 0 : (decimal) row["PESO"];
          prod.CATEGORIA = row["CATEGORIA"].ToString() ?? "";
          prod.MINIMO_RESURTIDO = row["MINIMO_RESURTIDO"].ToString() ?? "";
          prod.DIAMETRO = row["DIAMETRO"].ToString() ?? "";
          prod.LARGO = row["LARGO"].ToString() ?? "";
          prod.GRADO = row["GRADO"].ToString() ?? "";
          prod.HILOS = row["HILOS"].ToString();
          prod.NHILOS = row["NHILOS"].ToString();

          listaProductos.Add(prod);
          index++;
        }
      } catch (Exception ex) {

        throw new Exception($"Error in method GetNkProductsList, index: {index}. {ex.Message}");
      }

      return listaProductos;
    }


  }


}
