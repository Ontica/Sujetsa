using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TradeDataSchemaManager.Services {


  internal enum ConnectionName {
    ProductosNkConn,
    ProductosNKHidroplomex,
    ArtículosMicrosip
  }


  internal class ConnectionModel {

    public List<FbConnectionSettings> ConnectionSettings = new List<FbConnectionSettings>();

    public string SqlConnectionString {
      get; set;
    } = string.Empty;
  }


  internal class FbConnectionSettings {

    public int ConnectionId {
      get; set;
    }

    public string ConnectionName {
      get; set;
    } = string.Empty;

    public string ConnectionString {
      get; set;
    } = string.Empty;

  }


  public class FbQueryStrings {

    public string productosNkConn =

      "SELECT PRODUCTO.PRODUCTO,PRODUCTO.CLAVEPRODSERV,PRODUCTO.DESCRIPCION, PRODUCTO.ALTA, PRODUCTO.LINEA, LINEA.NOMBRE NLINEA, " +
      "PRODUCTO.GRUPO, GRUPO.DESCRIPCION NGRUPO, PRODUCTO.SUBGRUPO, SUBGRUPO.DESCRIPCION NSUBGRUPO, LARGO, " +
      "PRODUCTO.TALLA CABEZAS, TALLA.DESCRIPCION NCABEZAS, PRODUCTO.COLOR ACABADOS, COLOR.DESCRIPCION NACABADOS, " +
      "PRODUCTO.COSTO_BASE, INVENTARIO.EXISTENCIA, PRODUCTO.UNIDAD UNIDAD_VENTA, LISTA1.PRECIO PLISTA_1, LISTA2.PRECIO PLISTA_2, " +
      "LISTA.PRECIO PLISTA_3, LISTA5.PRECIO PLISTA_5,PRODUCTO.EMPAQUE, PRODUCTO.MULTIPLO_RESURTIDO, " +
      "COMPRAS.FECHA_ULTIMA_COMP FECHA_ULTIMA_COMPRA, COMPRAS.PRECIO COSTO_ULTIMA_COMPRA, PRODUCTO.PROV1 PROVEEDOR, " +
      "PROVEEDOR.NOMBRE_COMERCIAL NPROVEEDOR, PRODUCTO.TIPO, PRODUCTO.BAJA, PRODUCTO.PESO, PRODUCTO.CATEGORIA, " +
      "ALMACEN1.MINIMO MINIMO_RESURTIDO, PRODUCTO.USR1 DIAMETRO, PRODUCTO.USR2 LARGO, PRODUCTO.USR3 GRADO, " +
      "PRODUCTO.SECCION HILOS, SECCION.NOMBRE NHILOS " +

      "FROM PRODUCTO " +
      "LEFT JOIN LINEA ON PRODUCTO.LINEA = LINEA.LINEA " +
      "LEFT JOIN GRUPO ON PRODUCTO.GRUPO = GRUPO.GRUPO " +
      "LEFT JOIN SUBGRUPO ON PRODUCTO.GRUPO = SUBGRUPO.GRUPO AND PRODUCTO.SUBGRUPO = SUBGRUPO.SUBGRUPO " +
      "LEFT JOIN SECCION ON PRODUCTO.SECCION = SECCION.SECCION " +
      "LEFT JOIN TALLA ON PRODUCTO.TALLA = TALLA.TALLA " +
      "LEFT JOIN COLOR  ON PRODUCTO.COLOR = COLOR.COLOR " +

      "LEFT JOIN ( " +
      "      SELECT PRODUCTO, SUM(CANTIDAD) EXISTENCIA FROM CAPA GROUP BY CAPA.PRODUCTO " +
      ") INVENTARIO ON PRODUCTO.PRODUCTO = INVENTARIO.PRODUCTO " +

      "LEFT JOIN ( " +
      "	SELECT  PRODUCTO, LISTA, PRECIO FROM LISTAPRECIOS WHERE LISTA = 1 " +
      ") LISTA1 ON LISTA1.PRODUCTO = PRODUCTO.PRODUCTO " +

      "LEFT JOIN ( " +
      "	SELECT  PRODUCTO, LISTA, PRECIO FROM LISTAPRECIOS WHERE LISTA = 2 " +
      ") LISTA2 ON LISTA2.PRODUCTO = PRODUCTO.PRODUCTO " +

      "LEFT JOIN ( " +
      "	SELECT  PRODUCTO, LISTA, PRECIO FROM LISTAPRECIOS WHERE LISTA = 3 " +
      ") LISTA ON LISTA.PRODUCTO = PRODUCTO.PRODUCTO " +

      "LEFT JOIN ( " +
      "	SELECT  PRODUCTO, LISTA, PRECIO FROM LISTAPRECIOS WHERE LISTA = 5 " +
      ") LISTA5 ON LISTA5.PRODUCTO = PRODUCTO.PRODUCTO " +

      "LEFT JOIN ( " +
      "	SELECT  MAX(B.PRECIO) PRECIO, A.PRODUCTO,   A.FECHA_ULTIMA_COMP " +
      "    FROM ( " +
      "    	SELECT  MAX(COMPRA.FECHA) FECHA_ULTIMA_COMP, COMPRADET.PRODUCTO " +
      "        FROM COMPRADET " +
      "        INNER JOIN COMPRA ON COMPRA.COMPRA = COMPRADET.COMPRA " +
      "        GROUP  BY COMPRADET.PRODUCTO " +
      "        ) A " +
      "    LEFT JOIN ( " +
      "    	SELECT CD.COMPRA, CD.PRODUCTO, CD.PRECIO, C.FECHA " +
      "        FROM COMPRADET CD " +
      "        INNER JOIN COMPRA C ON C.COMPRA = CD.COMPRA " +
      "        ) B ON B.FECHA = A.FECHA_ULTIMA_COMP AND A.PRODUCTO = B.PRODUCTO " +
      "    GROUP BY A.PRODUCTO, A.FECHA_ULTIMA_COMP " +
      ") COMPRAS ON COMPRAS.PRODUCTO = PRODUCTO.PRODUCTO " +

      "LEFT JOIN PROVEEDOR ON PROVEEDOR.PROVEEDOR = PRODUCTO.PROV1 " +

      "LEFT JOIN ( " +
      "	SELECT PRODUCTO, MINIMO, ALMACEN.ALMACEN,ALMACEN.PRINCIPAL " +
      "    FROM productoalmacen " +
      "    LEFT JOIN ALMACEN ON ALMACEN.ALMACEN = PRODUCTOALMACEN.ALMACEN " +
      "    WHERE almacen.principal = 'S' " +
      ") ALMACEN1 ON ALMACEN1.PRODUCTO = PRODUCTO.PRODUCTO ";


    public string articulosMicrosipConn =
      "SELECT claves_articulos.clave_articulo PRODUCTO, articulos.articulo_id, articulos.nombre DESCRIPCION, " +
      "articulos.unidad_compra, articulos.unidad_venta, articulos.contenido_unidad_compra, articulos.es_almacenable, " +
      "articulos.es_importado, articulos.es_siempre_importado, articulos.peso_unitario, articulos.estatus, " +
      "lineas_articulos.linea_articulo_id, lineas_articulos.nombre GRUPO, SALDOS_IN.SUMA EXISTENCIA, PRECIOS.precio as PRECIO7 " +

      "FROM articulos " +
      "INNER JOIN claves_articulos ON claves_articulos.articulo_id = articulos.articulo_id " +
      "LEFT JOIN lineas_articulos ON lineas_articulos.linea_articulo_id = articulos.linea_articulo_id " +

      "LEFT JOIN ( " +
      "  SELECT ARTICULO_ID, Sum(SALDOS_IN.ENTRADAS_UNIDADES-SALDOS_IN.SALIDAS_UNIDADES) AS SUMA " +
      "    FROM SALDOS_IN " +
      "    GROUP BY ARTICULO_ID " +
      ") SALDOS_IN ON  SALDOS_IN.ARTICULO_ID = ARTICULOS.ARTICULO_ID " +

      "LEFT JOIN( " +
      "  SELECT precios_articulos.precio_articulo_id, precios_articulos.articulo_id, precios_articulos.precio_empresa_id, " +
      "    precios_articulos.precio, " +
      "    precios_articulos.fecha_hora_ult_modif " +
      "    FROM precios_articulos " +
      "    where  precios_articulos.precio_empresa_id = 42 " +
      ") AS PRECIOS ON articulos.articulo_id = PRECIOS.articulo_id " +
      
      "WHERE articulos.estatus = 'A'" +
      "ORDER BY claves_articulos.clave_articulo ";


    public string productosNKHidroplomexConn =
      "SELECT PRODUCTO.PRODUCTO, CLAVEPRODSERV, PRODUCTO.DESCRIPCION, DESC_LARGA, ALTA, PRODUCTO.LINEA, PRODUCTO.GRUPO, PRODUCTO.SUBGRUPO, " +
      "LARGO, COSTO_BASE, LISTA1.PRECIO PRECIO1, LISTA10.PRECIO PRECIO10, EMPAQUE, MULTIPLO_RESURTIDO, PRODUCTO.PROVEEDOR, " +
      "PRODUCTO.TIPO, PRODUCTO.BAJA, CATEGORIA, UNIDAD_COMPRA, UNIDAD_VENTA_MENUDEO UNIDAD_VENTA, GRUPO.DESCRIPCION NGRUPO, " +
      "INVENTARIO.EXISTENCIA, LINEA.NOMBRE NLINEA, SUBGRUPO.DESCRIPCION NSUBGRUPO, MARCA.NOMBRE MARCA, " +
      "COMPRAS.FECHA_ULTIMA_COMP FECHA_ULTIMA_COMPRA, " +
      "COMPRAS.PRECIO COSTO_ULTIMA_COMPRA, PROVEEDOR.NOMBRE_COMERCIAL NPROVEEDOR " +

      "FROM PRODUCTO " +
      "LEFT JOIN LINEA ON PRODUCTO.LINEA = LINEA.LINEA " +
      "LEFT JOIN MARCA ON PRODUCTO.MARCA = MARCA.MARCA " +
      "LEFT JOIN GRUPO ON PRODUCTO.GRUPO = GRUPO.GRUPO " +
      "LEFT JOIN PROVEEDOR ON PRODUCTO.PROVEEDOR = PROVEEDOR.PROVEEDOR " +
      "LEFT JOIN SUBGRUPO ON PRODUCTO.SUBGRUPO = SUBGRUPO.SUBGRUPO AND SUBGRUPO.GRUPO = GRUPO.GRUPO " +

      "LEFT JOIN ( " +
      "  SELECT PRODUCTO, LISTA, PRECIO FROM LISTAPRECIOS WHERE LISTA = 1 " +
      ") LISTA1 ON LISTA1.PRODUCTO = PRODUCTO.PRODUCTO " +

      "LEFT JOIN ( " +
      "  SELECT PRODUCTO, LISTA, PRECIO FROM LISTAPRECIOS WHERE LISTA = 10 " +
      ") LISTA10 ON LISTA10.PRODUCTO = PRODUCTO.PRODUCTO " +

      "LEFT JOIN( " +
      "  SELECT PRODUCTO, SUM(CANTIDAD) EXISTENCIA FROM CAPA GROUP BY CAPA.PRODUCTO " +
      ") INVENTARIO ON PRODUCTO.PRODUCTO = INVENTARIO.PRODUCTO " +

      "LEFT JOIN( " +
      "  SELECT MAX(B.PRECIO) PRECIO, A.PRODUCTO, A.FECHA_ULTIMA_COMP " +
      "    FROM ( " +
      "      SELECT MAX(COMPRA.FECHA) FECHA_ULTIMA_COMP, COMPRADET.PRODUCTO " +
      "        FROM COMPRADET " +
      "        INNER JOIN COMPRA ON COMPRA.COMPRA = COMPRADET.COMPRA " +
      "        GROUP BY COMPRADET.PRODUCTO " +
      "    ) A " +

      "    LEFT JOIN( " +
      "      SELECT CD.COMPRA, CD.PRODUCTO, CD.PRECIO, C.FECHA " +
      "        FROM COMPRADET CD " +
      "        INNER JOIN COMPRA C ON C.COMPRA = CD.COMPRA " +
      "    ) B ON B.FECHA = A.FECHA_ULTIMA_COMP AND A.PRODUCTO = B.PRODUCTO " +
      "    GROUP BY A.PRODUCTO, A.FECHA_ULTIMA_COMP " +

      ") COMPRAS ON COMPRAS.PRODUCTO = PRODUCTO.PRODUCTO ";


  }

}
