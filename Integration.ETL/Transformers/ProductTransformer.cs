/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Services Layer                        *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Service provider                      *
*  Type     : ProductTransformer                           License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Transforms a product from NK to Empiria Trade.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Data;

using Empiria.Trade.Integration.ETL.Data;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>Transforms a product from NK to Empiria Trade.</summary>
  public class ProductTransformer {

    public void Execute() {

      FixedList<ProductNK> sourceData = ReadSourceData();

      FixedList<ProductNK> toTransformData = ExtractDataToTransform(sourceData);

      FixedList<ProductData> transfomedData = Transform(toTransformData);

      WriteTargetData(transfomedData);
    }

    private FixedList<ProductNK> ExtractDataToTransform(FixedList<ProductNK> sourceData) {
      return sourceData.FindAll(x => x.OldChecksum != x.NewChecksum);
    }


    private FixedList<ProductNK> ReadSourceData() {
      var sql = "SELECT * FROM Productos";

      var inputDataService = new SqlServerDataServices("connectionString");

      return inputDataService.ReadData<ProductNK>(sql);
    }


    private FixedList<ProductData> Transform(FixedList<ProductNK> toTransformData) {
      return toTransformData.Select(x => Transform(x))
                            .ToFixedList();
    }


    private ProductData Transform(ProductNK toTransformData) {
      return new ProductData {
        ProductId = 0,
        ProductCode = toTransformData.Clave,
        Name = toTransformData.Nombre,
        StartDate = DateTime.Today,
        EndDate = ExecutionServer.DateMaxValue
      };
    }


    private void WriteTargetData(FixedList<ProductData> transformedData) {
      foreach (var item in transformedData) {
        WriteTargetData(item);
      }
    }


    private void WriteTargetData(ProductData o) {
      var op = DataOperation.Parse("write_OMS_Product", o.ProductId, o.ProductCode,
        o.Name, o.StartDate, o.EndDate);

      DataWriter.Execute(op);
    }


  }  // class ProductTransformer

} // namespace Empiria.Trade.Integration.ETL.Transformers
