using System;
using System.Collections.Generic;
using System.Linq;
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
      //"SELECT PRODUCTO.PRODUCTO,PRODUCTO.CLAVEPRODSERV,PRODUCTO.DESCRIPCION, PRODUCTO.ALTA, PRODUCTO.LINEA, " +
      //"LINEA.NOMBRE NLINEA, PRODUCTO.GRUPO, GRUPO.DESCRIPCION NGRUPO, PRODUCTO.SUBGRUPO, SUBGRUPO.DESCRIPCION NSUBGRUPO, LARGO, PRODUCTO.SECCION, " +
      //"SECCION.NOMBRE, PRODUCTO.MODELO PASOS, MODELO.DESCRIPCION NPASOS, PRODUCTO.TALLA CABEZAS, TALLA.DESCRIPCION NCABEZAS, PRODUCTO.COLOR ACABADOS, " +
      //"COLOR.DESCRIPCION NACABADOS, PRODUCTO.COSTO_BASE, LISTA.PRECIO AS PMINIMO,INVENTARIO.EXISTENCIA, PRODUCTO.UNIDAD UNIDAD_VENTA, PRODUCTO.MONEDA, " + 
      //"LISTA1.PRECIO PLISTA_1, LISTA2.PRECIO PLISTA_2, LISTA.PRECIO PLISTA_3, LISTA4.PRECIO PLISTA_4,PRODUCTO.EMPAQUE, PRODUCTO.MULTIPLO_RESURTIDO, " +
      //"COMPRAS.PRECIO COSTO_ULTIMA_COMPRA, PRODUCTO.PROV1 PROVEEDOR,PROVEEDOR.NOMBRE_COMERCIAL NPROVEEDOR, PRODUCTO.TIPO,PRODUCTO.BAJA, PRODUCTO.PESO, " +
      //"PRODUCTO.CATEGORIA,ALMACEN1.MINIMO MINIMO_RESURTIDO, PRODUCTO.USR1 DIAMETRO, PRODUCTO.USR2 LARGO, PRODUCTO.USR3 GRADO, PRODUCTO.SECCION HILOS, " +
      //"SECCION.NOMBRE NHILOS " +
      
      "SELECT PRODUCTO.PRODUCTO,PRODUCTO.CLAVEPRODSERV,PRODUCTO.DESCRIPCION, PRODUCTO.ALTA, PRODUCTO.LINEA, " +
      "LINEA.NOMBRE NLINEA, PRODUCTO.GRUPO, GRUPO.DESCRIPCION NGRUPO, PRODUCTO.SUBGRUPO, SUBGRUPO.DESCRIPCION NSUBGRUPO, LARGO, PRODUCTO.SECCION, " +
      "SECCION.NOMBRE, PRODUCTO.MODELO PASOS, MODELO.DESCRIPCION NPASOS, PRODUCTO.TALLA CABEZAS, TALLA.DESCRIPCION NCABEZAS, PRODUCTO.COLOR ACABADOS, " +
      "COLOR.DESCRIPCION NACABADOS, PRODUCTO.COSTO_BASE, LISTA.PRECIO AS PMINIMO,INVENTARIO.EXISTENCIA, PRODUCTO.UNIDAD UNIDAD_VENTA, PRODUCTO.MONEDA, " +
      "LISTA1.PRECIO PLISTA_1, LISTA2.PRECIO PLISTA_2, LISTA.PRECIO PLISTA_3, LISTA4.PRECIO PLISTA_4,PRODUCTO.EMPAQUE, PRODUCTO.MULTIPLO_RESURTIDO, " +
      "COMPRAS.FECHA_ULTIMA_COMP FECHA_ULTIMA_COMPRA, " +
      "COMPRAS.PRECIO COSTO_ULTIMA_COMPRA, PRODUCTO.PROV1 PROVEEDOR,PROVEEDOR.NOMBRE_COMERCIAL NPROVEEDOR, PRODUCTO.TIPO, PRODUCTO.BAJA, PRODUCTO.PESO, " +
      "PRODUCTO.CATEGORIA, ALMACEN1.MINIMO MINIMO_RESURTIDO, PRODUCTO.USR1 DIAMETRO, PRODUCTO.USR2 LARGO, PRODUCTO.USR3 GRADO, PRODUCTO.SECCION HILOS, " +
      "SECCION.NOMBRE NHILOS " +

      "FROM PRODUCTO " +
      "LEFT JOIN LINEA ON PRODUCTO.LINEA = LINEA.LINEA " +
      "LEFT JOIN GRUPO ON PRODUCTO.GRUPO = GRUPO.GRUPO " +
      "LEFT JOIN SUBGRUPO ON PRODUCTO.GRUPO = SUBGRUPO.GRUPO AND PRODUCTO.SUBGRUPO = SUBGRUPO.SUBGRUPO " +
      "LEFT JOIN SECCION ON PRODUCTO.SECCION = SECCION.SECCION " +
      "LEFT JOIN MODELO ON PRODUCTO.MODELO = MODELO.MODELO " +
      "LEFT JOIN TALLA ON PRODUCTO.TALLA = TALLA.TALLA " +
      "LEFT JOIN COLOR  ON PRODUCTO.COLOR = COLOR.COLOR " +
      "LEFT JOIN " +
       "(SELECT  PRODUCTO, LISTA, PRECIO FROM LISTAPRECIOS WHERE LISTA = 3) LISTA ON LISTA.PRODUCTO = PRODUCTO.PRODUCTO " +
         "LEFT JOIN " +
       "(SELECT PRODUCTO, SUM(CANTIDAD) EXISTENCIA FROM CAPA GROUP BY CAPA.PRODUCTO) INVENTARIO ON PRODUCTO.PRODUCTO = INVENTARIO.PRODUCTO " +
      "LEFT JOIN " +
       "(SELECT  PRODUCTO, LISTA, PRECIO FROM LISTAPRECIOS WHERE LISTA = 1) LISTA1 ON LISTA1.PRODUCTO = PRODUCTO.PRODUCTO " +
      "LEFT JOIN " +
       "(SELECT  PRODUCTO, LISTA, PRECIO FROM LISTAPRECIOS WHERE LISTA = 2) LISTA2 ON LISTA2.PRODUCTO = PRODUCTO.PRODUCTO " +
      "LEFT JOIN " +
       "(SELECT  PRODUCTO, LISTA, PRECIO FROM LISTAPRECIOS WHERE LISTA = 5) LISTA4 ON LISTA4.PRODUCTO = PRODUCTO.PRODUCTO " +
      "LEFT JOIN " +
      "(" +
      " SELECT  MAX(B.PRECIO) PRECIO, A.PRODUCTO,   A.FECHA_ULTIMA_COMP " +
      " FROM (" +
      "   SELECT  MAX(COMPRA.FECHA) FECHA_ULTIMA_COMP, COMPRADET.PRODUCTO " +
      "   FROM COMPRADET " +
      "   INNER JOIN COMPRA ON COMPRA.COMPRA = COMPRADET.COMPRA " +
      "   GROUP  BY COMPRADET.PRODUCTO" +
      " ) A " +
      " LEFT JOIN (" +
      "   SELECT CD.COMPRA, CD.PRODUCTO, CD.PRECIO, C.FECHA " +
      "   FROM COMPRADET CD " +
      "   INNER JOIN COMPRA C ON C.COMPRA = CD.COMPRA" +
      " ) B ON B.FECHA = A.FECHA_ULTIMA_COMP AND A.PRODUCTO = B.PRODUCTO " +
      " GROUP BY A.PRODUCTO, A.FECHA_ULTIMA_COMP " +
      ") COMPRAS ON COMPRAS.PRODUCTO = PRODUCTO.PRODUCTO " +
      
      "LEFT JOIN PROVEEDOR ON PROVEEDOR.PROVEEDOR = PRODUCTO.PROV1 " +
      "LEFT JOIN (SELECT PRODUCTO, MINIMO, ALMACEN.ALMACEN,ALMACEN.PRINCIPAL FROM productoalmacen " +
      "LEFT JOIN ALMACEN ON ALMACEN.ALMACEN = PRODUCTOALMACEN.ALMACEN where almacen.principal = 'S' ) ALMACEN1 " +
      "ON ALMACEN1.PRODUCTO = PRODUCTO.PRODUCTO";

    public string productosNKHidroplomexConn =
      "SELECT PRODUCTO.PRODUCTO, CLAVEPRODSERV, PRODUCTO.DESCRIPCION, DESC_LARGA, ALTA, PRODUCTO.LINEA, PRODUCTO.GRUPO, " +
      "PRODUCTO.SUBGRUPO, LARGO, COSTO_BASE, PRODUCTO.MONEDA, PRECIO1, PRECIO2, PRECIO3, PRECIO4, EMPAQUE, MULTIPLO_RESURTIDO, " +
      "PRODUCTO.PROVEEDOR, PRODUCTO.TIPO, PRODUCTO.BAJA, " +
      "CATEGORIA, UNIDAD_COMPRA, UNIDAD_VENTA_MENUDEO UNIDAD_VENTA, GRUPO.DESCRIPCION NGRUPO, INVENTARIO.EXISTENCIA, " +
      "LINEA.NOMBRE NLINEA, SUBGRUPO.DESCRIPCION NSUBGRUPO, " +
      "GIRO.NOMBRE GIRO, MARCA.NOMBRE MARCA, SECCION.NOMBRE SECCION, MODELO.DESCRIPCION MODELO, " +
      "COMPRAS.FECHA_ULTIMA_COMP FECHA_ULTIMA_COMPRA, COMPRAS.PRECIO COSTO_ULTIMA_COMPRA, " +
      "PROVEEDOR.NOMBRE_COMERCIAL NPROVEEDOR " +
      "FROM PRODUCTO " +
      "LEFT JOIN LINEA ON PRODUCTO.LINEA = LINEA.LINEA " +
      "LEFT JOIN MARCA ON PRODUCTO.MARCA = MARCA.MARCA " +
      "LEFT JOIN GRUPO ON PRODUCTO.GRUPO = GRUPO.GRUPO " +
      "LEFT JOIN SECCION ON PRODUCTO.SECCION = SECCION.SECCION " +
      "LEFT JOIN MODELO ON PRODUCTO.MODELO = MODELO.MODELO  " +
      "LEFT JOIN GIRO ON PRODUCTO.GIRO = GIRO.GIRO " +
      "LEFT JOIN PROVEEDOR ON PRODUCTO.PROVEEDOR = PROVEEDOR.PROVEEDOR " +
      "LEFT JOIN SUBGRUPO ON PRODUCTO.SUBGRUPO = SUBGRUPO.SUBGRUPO AND SUBGRUPO.GRUPO = GRUPO.GRUPO " +
      "LEFT JOIN (SELECT PRODUCTO, SUM(CANTIDAD) EXISTENCIA FROM CAPA GROUP BY CAPA.PRODUCTO) INVENTARIO " +
      "ON PRODUCTO.PRODUCTO = INVENTARIO.PRODUCTO " +
      "LEFT JOIN " +
      "(" +
      " SELECT  MAX(B.PRECIO) PRECIO, A.PRODUCTO,   A.FECHA_ULTIMA_COMP " +
      " FROM (" +
      "   SELECT  MAX(COMPRA.FECHA) FECHA_ULTIMA_COMP, COMPRADET.PRODUCTO " +
      "   FROM COMPRADET " +
      "   INNER JOIN COMPRA ON COMPRA.COMPRA = COMPRADET.COMPRA " +
      "   GROUP  BY COMPRADET.PRODUCTO" +
      " ) A " +
      " LEFT JOIN (" +
      "   SELECT CD.COMPRA, CD.PRODUCTO, CD.PRECIO, C.FECHA " +
      "   FROM COMPRADET CD " +
      "   INNER JOIN COMPRA C ON C.COMPRA = CD.COMPRA" +
      " ) B ON B.FECHA = A.FECHA_ULTIMA_COMP AND A.PRODUCTO = B.PRODUCTO " +
      " GROUP BY A.PRODUCTO, A.FECHA_ULTIMA_COMP " +
      ") COMPRAS ON COMPRAS.PRODUCTO = PRODUCTO.PRODUCTO ";
      //"(" +
      //" SELECT A.PRODUCTO, B.PRECIO,  A.FECHA_ULTIMA_COMP " +
      //" FROM (" +
      //"   SELECT MAX(COMPRA.FECHA) FECHA_ULTIMA_COMP, COMPRADET.PRODUCTO " +
      //"   FROM COMPRADET " +
      //"   INNER JOIN COMPRA ON  COMPRA.COMPRA = COMPRADET.COMPRA " +
      //"   GROUP  BY COMPRADET.PRODUCTO " +
      //" ) A " +
      //" LEFT JOIN ( " +
      //"   SELECT CD.PRODUCTO, CD.PRECIO,  C.FECHA " +
      //"   FROM COMPRADET CD " +
      //"   INNER JOIN COMPRA C ON C.COMPRA = CD.COMPRA" +
      //" ) B ON B.FECHA = A.FECHA_ULTIMA_COMP AND A.PRODUCTO = B.PRODUCTO " +
      //") COMPRAS ON COMPRAS.PRODUCTO = PRODUCTO.PRODUCTO ";
      //"WHERE PRODUCTO.BAJA = 'N'";


    public string articulosMicrosipConn =
      "SELECT  " +
      "claves_articulos.clave_articulo PRODUCTO, articulos.articulo_id, articulos.nombre DESCRIPCION, articulos.unidad_compra, " +
      "articulos.unidad_venta, articulos.contenido_unidad_compra, articulos.es_almacenable, articulos.es_importado, " +
      "articulos.es_siempre_importado, articulos.peso_unitario, articulos.estatus,lineas_articulos.linea_articulo_id, " +
      "lineas_articulos.nombre NOMBRE_LINEA, SALDOS_IN.SUMA EXISTENCIA, PRECIOS.precio as Precio_lista, " +
      "PRECIOS_MIN.precio as Precio_Minimo, PRECIO3.PRECIO AS PRECIO_ESP_SUJETSA, PRECIO4.PRECIO AS PRECIO_ESP_HERRAMIENTAS, " +
      "PRECIO5.PRECIO AS PRECIO_ESP_TTC " +

      "FROM articulos " +

      "INNER JOIN claves_articulos ON claves_articulos.articulo_id = articulos.articulo_id " +

      "LEFT JOIN lineas_articulos ON lineas_articulos.linea_articulo_id = articulos.linea_articulo_id    " +

      "LEFT JOIN (" +
      " SELECT ARTICULO_ID, Sum(SALDOS_IN.ENTRADAS_UNIDADES-SALDOS_IN.SALIDAS_UNIDADES) AS SUMA " +
      " FROM SALDOS_IN " +
      " GROUP BY ARTICULO_ID " +
      ") SALDOS_IN ON  SALDOS_IN.ARTICULO_ID = ARTICULOS.ARTICULO_ID " +

      "LEFT JOIN (" +
      " SELECT precios_articulos.precio_articulo_id,  precios_articulos.articulo_id, precios_articulos.precio_empresa_id, " +
      " precios_articulos.precio, precios_articulos.moneda_id,  monedas.nombre, precios_articulos.margen, precios_articulos.markup, " +
      " precios_articulos.fecha_hora_ult_modif, precios_empresa.nombre lista " +
      " FROM precios_articulos " +
      " LEFT JOIN MONEDAS ON monedas.moneda_id = precios_articulos.moneda_id " +
      " LEFT JOIN precios_empresa   ON precios_empresa.precio_empresa_id = precios_articulos.precio_empresa_id " +
      " where  precios_articulos.precio_empresa_id = 42 " +
      ") AS PRECIOS ON articulos.articulo_id = PRECIOS.articulo_id "+

      "LEFT JOIN (" +
      " SELECT precios_articulos.precio_articulo_id,  precios_articulos.articulo_id, precios_articulos.precio_empresa_id, " +
      " precios_articulos.precio, precios_articulos.moneda_id,  monedas.nombre, precios_articulos.margen, precios_articulos.markup, " +
      " precios_articulos.fecha_hora_ult_modif, precios_empresa.nombre lista " +
      " FROM precios_articulos " +
      " LEFT JOIN MONEDAS ON monedas.moneda_id = precios_articulos.moneda_id " +
      " LEFT JOIN precios_empresa   ON precios_empresa.precio_empresa_id = precios_articulos.precio_empresa_id " +
      " WHERE  precios_articulos.precio_empresa_id = 43 " +
      ") AS PRECIOS_MIN ON articulos.articulo_id = PRECIOS_MIN.articulo_id " +

      "LEFT JOIN (" +
      " SELECT precios_articulos.precio_articulo_id,  precios_articulos.articulo_id, precios_articulos.precio_empresa_id, " +
      " precios_articulos.precio, precios_articulos.moneda_id,  monedas.nombre, precios_articulos.margen, precios_articulos.markup, " +
      " precios_articulos.fecha_hora_ult_modif, precios_empresa.nombre lista " +
      " FROM precios_articulos " +
      " LEFT JOIN MONEDAS ON monedas.moneda_id = precios_articulos.moneda_id " +
      " LEFT JOIN precios_empresa   ON precios_empresa.precio_empresa_id = precios_articulos.precio_empresa_id " +
      " WHERE  precios_articulos.precio_empresa_id = 751978 " +
      ") AS PRECIO3 ON articulos.articulo_id = PRECIO3.articulo_id " +

      "LEFT JOIN (" +
      " SELECT precios_articulos.precio_articulo_id,  precios_articulos.articulo_id, precios_articulos.precio_empresa_id, " +
      " precios_articulos.precio, precios_articulos.moneda_id,  monedas.nombre, precios_articulos.margen, precios_articulos.markup, " +
      " precios_articulos.fecha_hora_ult_modif, precios_empresa.nombre lista " +
      " FROM precios_articulos " +
      " LEFT JOIN MONEDAS ON monedas.moneda_id = precios_articulos.moneda_id " +
      " LEFT JOIN precios_empresa   ON precios_empresa.precio_empresa_id = precios_articulos.precio_empresa_id " +
      " WHERE  precios_articulos.precio_empresa_id = 751992 " +
      ") AS PRECIO4 ON articulos.articulo_id = PRECIO4.articulo_id " + 

      "LEFT JOIN (" +
      " SELECT precios_articulos.precio_articulo_id,  precios_articulos.articulo_id, precios_articulos.precio_empresa_id, " +
      " precios_articulos.precio, precios_articulos.moneda_id,  monedas.nombre, precios_articulos.margen, precios_articulos.markup, " +
      " precios_articulos.fecha_hora_ult_modif, precios_empresa.nombre lista " +
      " FROM precios_articulos " +
      " LEFT JOIN MONEDAS ON monedas.moneda_id = precios_articulos.moneda_id " +
      " LEFT JOIN precios_empresa   ON precios_empresa.precio_empresa_id = precios_articulos.precio_empresa_id " +
      " WHERE  precios_articulos.precio_empresa_id = 765683 " +
      ") AS PRECIO5 ON articulos.articulo_id = PRECIO5.articulo_id " +

      "ORDER BY claves_articulos.clave_articulo ";

  }

}
