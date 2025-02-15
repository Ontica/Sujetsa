﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TradeDataSchemaManager.Adapters;
using TradeDataSchemaManager.Data;

namespace TradeDataSchemaManager.Services {

  public class SchemaServices {

    private ConnectionModel conInfo = new ConnectionModel();
    private Helper helper = new Helper();

    public SchemaServices(bool isTest = false) {

      conInfo = helper.GetConnectionInfo<ConnectionModel>(isTest);

    }


    public List<ProductosAdapter> GetDataFromDb() {
      try {

        var productList = helper.GetProductsListByDB(conInfo);

        return productList;

      } catch (Exception ex) {

        throw new Exception($"TradeDataSchemaManager.Services.(GetDataFromDb())... {ex.Message}");
      }


    }


    public string GetDataCountFromDb() {

      try {

        var productList = GetDataFromDb();

        int nkbd = productList.FindAll(x => x.COMPANIA_ID == 1).Count();
        int microbd = productList.FindAll(x => x.COMPANIA_ID == 2).Count();
        int nkhpbd = productList.FindAll(x => x.COMPANIA_ID == 3).Count();

        return $"PRODUCTOS BD NK = {nkbd}. " +
               $"ARTICULOS BD Microsip = {microbd}. " +
               $"PRODUCTOS BD NKHidroplomex = {nkhpbd}. " +
               $"TOTAL = {nkbd + microbd + nkhpbd}";

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
