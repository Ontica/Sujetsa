﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ConnectionsToFirebirdSujetsa.Adapters;
using ConnectionsToFirebirdSujetsa.Data;

namespace ConnectionsToFirebirdSujetsa.Services {
  public class Services {

    private ConnectionModel conInfo = new ConnectionModel();
    private Helper helper = new Helper();

    public Services(bool isTest = false) {

      conInfo = helper.GetConnectionInfo<ConnectionModel>(isTest);

    }


    public List<ProductosAdapter> GetDataFromDb() {
      try {

        var productList = helper.GetProductsListByDB(conInfo);

        return productList;

      } catch (Exception ex) {

        throw new Exception($"ERROR: {ex.Message}");
      }
      

    }


    public string GetDataCountFromDb() {

      try {

        var productList = GetDataFromDb();

        int nkbd = productList.FindAll(x => x.ALMACEN_ID == 1).Count();
        int nkhpbd = productList.FindAll(x => x.ALMACEN_ID == 2).Count();

        return $"PRODUCTOS BD NK = {nkbd}. PRODUCTOS BD NKHidroplomex = {nkhpbd}. TOTAL = {nkbd + nkhpbd}";

      } catch (Exception ex) {

        throw new Exception($"ERROR: {ex.Message}");
      }
      
    }


    public string InsertProductToSql(List<ProductosAdapter> productsToUpdate) {

      var data = new DataService();

      try {

        return data.InsertProductToSql(productsToUpdate, conInfo.SqlConnectionString);

      } catch (Exception ex) {

        throw new Exception($"ERROR: {ex.Message}");
      }
    }


    public async Task<String> InsertProductToSqlAsync(List<ProductosAdapter> productsToUpdate) {

      var data = new DataService();

      try {

        return await Task.Run(() => data.InsertProductToSql(productsToUpdate, conInfo.SqlConnectionString)).ConfigureAwait(false);

      } catch (Exception ex) {

        throw new Exception($"ERROR: {ex.Message}");
      }
    }


    public List<ProductosAdapter> GetListFromSql() {

      var data = new DataService();
      return data.GetListFromSql(conInfo.SqlConnectionString);
    }
  }
}
