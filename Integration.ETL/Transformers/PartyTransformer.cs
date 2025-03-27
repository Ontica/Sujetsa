/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Services Layer                        *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Service provider                      *
*  Type     : PartyTransformer                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Transforms a client from NK to Empiria Trade.                                                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Data;
using Empiria.Json;
using Empiria.Trade.Integration.ETL.Data;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>Transforms a client from NK to Empiria Trade.</summary>
  public class PartyTransformer {

    private readonly string _connectionString;

    internal PartyTransformer(string connectionString) {
      Assertion.Require(connectionString, nameof(connectionString));

      _connectionString = connectionString;
    }

    public void Execute() {

      FixedList<PartyNK> sourceData = ReadSourceData();

      FixedList<PartyData> transformedData = Transform(sourceData);

      WriteTargetData(transformedData);

      FixedList<PartyUsersNK> sourceUsersData = ReadSourceUsersData();

      //FixedList<PartyNK> toTransformData = ExtractDataToTransform(sourceData);

      FixedList<PartyData> transformedUsersData = TransformUsers(sourceUsersData);

      WriteTargetData(transformedUsersData);

      FixedList<PartySalespersonNK> sourceSalespersonData = ReadSourceSalespersonData();

      //FixedList<PartyNK> toTransformData = ExtractDataToTransform(sourceData);

      FixedList<PartyData> transformedSalespersonData = TransformSalesperson(sourceSalespersonData);

      WriteTargetData(transformedSalespersonData);

      FixedList<PartyData> transformedUsersDataForPartyContactIdUpdate = TransformUsersForPartyContactIdUpdate(sourceUsersData);

      WriteTargetData(transformedUsersDataForPartyContactIdUpdate);

      FixedList<PartyData> transformedSalespersonDataForPartyContactIdUpdate = TransformSalespersonForPartyContactIdUpdate(sourceSalespersonData);

      WriteTargetData(transformedSalespersonDataForPartyContactIdUpdate);
    }
    /*
    private FixedList<PartyNK> ExtractDataToTransform(FixedList<PartyNK> sourceData) {
      return sourceData.FindAll(x => x.OldBinaryChecksum != x.BinaryChecksum || x.OldBinaryChecksum == 0);
    }*/

    private FixedList<PartySalespersonNK> ReadSourceSalespersonData() {
      var sql = "SELECT VENDEDOR,NOMBRE,TELEFONO,EMAIL,ACTIVO,BinaryChecksum,OldBinaryChecksum FROM sources.VENDEDOR_TARGET PT " +
        "WHERE PT.OldBinaryChecksum != PT.BinaryChecksum " +
        "OR PT.OldBinaryChecksum = 0 " +
        "OR PT.OldBinaryChecksum IS NULL ";

      var connectionString = GetNKConnectionString();

      var inputDataService = new TransformerDataServices(connectionString);

      return inputDataService.ReadData<PartySalespersonNK>(sql);
    }

    private FixedList<PartyUsersNK> ReadSourceUsersData() {
      var sql = "SELECT USUARIO,NOMBRE,PERFIL,BinaryChecksum,OldBinaryChecksum " +
        "FROM sources.USUARIO_TARGET PT " +
        "WHERE PT.OldBinaryChecksum != PT.BinaryChecksum " +
        "OR PT.OldBinaryChecksum = 0 " +
        "OR PT.OldBinaryChecksum IS NULL ";

      var connectionString = GetNKConnectionString();

      var inputDataService = new TransformerDataServices(connectionString);

      return inputDataService.ReadData<PartyUsersNK>(sql);
    }

    private FixedList<PartyNK> ReadSourceData() {
      var sql = "SELECT * FROM sources.CLIENTE_TARGET PT " +
        "WHERE PT.OldBinaryChecksum != PT.BinaryChecksum " +
        "OR PT.OldBinaryChecksum = 0 " +
        "OR PT.OldBinaryChecksum IS NULL ";

      var connectionString = GetNKConnectionString();

      var inputDataService = new TransformerDataServices(connectionString);

      return inputDataService.ReadData<PartyNK>(sql);
    }


    private FixedList<PartyData> Transform(FixedList<PartyNK> toTransformData) {
      return toTransformData.Select(x => Transform(x))
                            .ToFixedList();
    }


    private FixedList<PartyData> TransformUsers(FixedList<PartyUsersNK> toTransformData) {
      return toTransformData.Select(x => TransformUsers(x))
                            .ToFixedList();
    }


    private FixedList<PartyData> TransformSalesperson(FixedList<PartySalespersonNK> toTransformData) {
      return toTransformData.Select(x => TransformSalesperson(x))
                            .ToFixedList();
    }


    private FixedList<PartyData> TransformUsersForPartyContactIdUpdate(FixedList<PartyUsersNK> toTransformData) {
      return toTransformData.Select(x => TransformUsersForPartyContactIdUpdate(x))
                            .ToFixedList();
    }


    private FixedList<PartyData> TransformSalespersonForPartyContactIdUpdate(FixedList<PartySalespersonNK> toTransformData) {
      return toTransformData.Select(x => TransformSalespersonForPartyContactIdUpdate(x))
                            .ToFixedList();
    }


    private PartyData Transform(PartyNK toTransformData) {
      string connectionString = GetEmpiriaConnectionString();
      var dataServices = new TransformerDataServices(connectionString);
      //toTransformData.OldBinaryChecksum = 0; insertar por que es nuevo
      if (toTransformData.OldBinaryChecksum == 0) {
        return new PartyData {
          Party_Id = dataServices.GetNextId("Parties"),
          Party_UID = System.Guid.NewGuid().ToString(),
          Party_Type_Id = (int) ReturnIdForPartyCode(toTransformData.RFC),//////153 o 154  de types puede ser con el RFC, si comienza con 4 letras es mpoesona, con 3 es moral
          Party_Code = toTransformData.Cliente,
          Party_Name =  toTransformData.Nombre,
          Party_Identificators = toTransformData.Cliente,
          Party_Roles = Empiria.EmpiriaString.BuildKeywords(toTransformData.Nombre, toTransformData.RFC, toTransformData.SAT_pais, toTransformData.SAT_FormaPago,
          toTransformData.SAT_MetodoPago, toTransformData.SAT_RegimenFiscal, toTransformData.SAT_UsoCFDI),
          Party_Tags = toTransformData.RFC,
          Party_Ext_Data = "",
          Party_Keywords = Empiria.EmpiriaString.BuildKeywords(toTransformData.Nombre, toTransformData.TSM, toTransformData.RFC, toTransformData.Nombre_Comercial,
          toTransformData.Calle, toTransformData.Numero, toTransformData.Numero_Interior, toTransformData.Colonia, toTransformData.CP, toTransformData.Localidad,  
          toTransformData.Ciudad1, toTransformData.Municipio, toTransformData.Estado1, toTransformData.Pais1, toTransformData.Telefono1, toTransformData.Telefono2),
          Party_Historic_Id = -1,
          Party_Start_Date = toTransformData.Fecha_Alta,
          Party_End_Date =  ExecutionServer.DateMaxValue,
          Party_Parent_Id = -1,
          Party_Posted_By_Id = -1,
          Party_Posting_Time = toTransformData.Fecha_Alta,
          Party_Status = "A", /////PENDIENTE REGLA 
          Party_Contact_Id = -1
        };
      } else {
        return new PartyData {
          Party_Id = dataServices.GetPartyIdFromParties(toTransformData.Cliente),
          Party_UID = dataServices.GetPartyUIDFromParties(toTransformData.Cliente),
          Party_Type_Id = (int) ReturnIdForPartyCode(toTransformData.RFC),//////153 o 154  de types puede ser con el RFC, si comienza con 4 letras es mpoesona, con 3 es moral
          Party_Code = toTransformData.Cliente,
          Party_Name = toTransformData.Nombre,
          Party_Identificators = toTransformData.Cliente,
          Party_Roles = Empiria.EmpiriaString.BuildKeywords(toTransformData.Nombre, toTransformData.RFC, toTransformData.SAT_pais, toTransformData.SAT_FormaPago,
          toTransformData.SAT_MetodoPago, toTransformData.SAT_RegimenFiscal, toTransformData.SAT_UsoCFDI),
          Party_Tags = toTransformData.RFC,
          Party_Ext_Data = "",
          Party_Keywords = Empiria.EmpiriaString.BuildKeywords(toTransformData.Nombre, toTransformData.TSM, toTransformData.RFC, toTransformData.Nombre_Comercial,
          toTransformData.Calle, toTransformData.Numero, toTransformData.Numero_Interior, toTransformData.Colonia, toTransformData.CP, toTransformData.Localidad,
          toTransformData.Ciudad1, toTransformData.Municipio, toTransformData.Estado1, toTransformData.Pais1, toTransformData.Telefono1, toTransformData.Telefono2),
          Party_Historic_Id = -1,
          Party_Start_Date = toTransformData.Fecha_Alta,
          Party_End_Date = ExecutionServer.DateMaxValue,
          Party_Parent_Id = -1,
          Party_Posted_By_Id = -1,
          Party_Posting_Time = toTransformData.Fecha_Alta,
          Party_Status = "A", /////PENDIENTE REGLA DE MEMO fechas baja
          Party_Contact_Id = -1
        };
      }
      //toTransformData.OldBinaryChecksum != null; actualizar por que ya existe

    }


    private PartyData TransformUsers(PartyUsersNK toTransformData) {
      string connectionString = GetEmpiriaConnectionString();
      var dataServices = new TransformerDataServices(connectionString);
      //toTransformData.OldBinaryChecksum = 0; insertar por que es nuevo
      if (toTransformData.OldBinaryChecksum == 0) {
        return new PartyData {
          Party_Id = dataServices.GetNextId("Parties"),
          Party_UID = System.Guid.NewGuid().ToString(),
          Party_Type_Id = 153,
          Party_Code = toTransformData.Usuario,
          Party_Name = toTransformData.Nombre,
          Party_Identificators = toTransformData.Usuario,
          Party_Roles = Empiria.EmpiriaString.BuildKeywords(toTransformData.Usuario, toTransformData.Nombre, toTransformData.Perfil),
          Party_Tags = toTransformData.Usuario,
          Party_Ext_Data = "",
          Party_Keywords = Empiria.EmpiriaString.BuildKeywords(toTransformData.Usuario, toTransformData.Perfil),
          Party_Historic_Id = -1,
          Party_Start_Date = ExecutionServer.DateMinValue,
          Party_End_Date = ExecutionServer.DateMaxValue,
          Party_Parent_Id = -1,
          Party_Posted_By_Id = -1,
          Party_Posting_Time = ExecutionServer.DateMinValue,
          Party_Status = "A", 
          Party_Contact_Id = -1// dataServices.GetNextId("Parties")-1
        };
      } else {
        return new PartyData {
          Party_Id = dataServices.GetPartyIdFromParties(toTransformData.Usuario),
          Party_UID = dataServices.GetPartyUIDFromParties(toTransformData.Usuario),
          Party_Type_Id = 153,
          Party_Code = toTransformData.Usuario,
          Party_Name = toTransformData.Nombre,
          Party_Identificators = toTransformData.Usuario,
          Party_Roles = Empiria.EmpiriaString.BuildKeywords(toTransformData.Usuario, toTransformData.Nombre, toTransformData.Perfil),
          Party_Tags = toTransformData.Usuario,
          Party_Ext_Data = "",
          Party_Keywords = Empiria.EmpiriaString.BuildKeywords(toTransformData.Usuario),
          Party_Historic_Id = -1,
          Party_Start_Date = ExecutionServer.DateMinValue,
          Party_End_Date = ExecutionServer.DateMaxValue,
          Party_Parent_Id = -1,
          Party_Posted_By_Id = -1,
          Party_Posting_Time = ExecutionServer.DateMinValue,
          Party_Status = "A",
          Party_Contact_Id = dataServices.GetPartyIdFromParties(toTransformData.Usuario)
        };
      }
    }


    private PartyData TransformSalesperson(PartySalespersonNK toTransformData) {
      string connectionString = GetEmpiriaConnectionString();
      var dataServices = new TransformerDataServices(connectionString);
      //toTransformData.OldBinaryChecksum = 0; insertar por que es nuevo
      if (toTransformData.OldBinaryChecksum == 0) {
        return new PartyData {
          Party_Id = dataServices.GetNextId("Parties"),
          Party_UID = System.Guid.NewGuid().ToString(),
          Party_Type_Id = 153,
          Party_Code = toTransformData.Vendedor,
          Party_Name = toTransformData.Nombre,
          Party_Identificators = toTransformData.Vendedor,
          Party_Roles = Empiria.EmpiriaString.BuildKeywords(toTransformData.Vendedor, toTransformData.Nombre, toTransformData.Email),
          Party_Tags = toTransformData.Vendedor,
          Party_Ext_Data = "",
          Party_Keywords = Empiria.EmpiriaString.BuildKeywords(toTransformData.Vendedor, toTransformData.Telefono),
          Party_Historic_Id = -1,
          Party_Start_Date = ExecutionServer.DateMinValue,
          Party_End_Date = ExecutionServer.DateMaxValue,
          Party_Parent_Id = -1,
          Party_Posted_By_Id = -1,
          Party_Posting_Time = ExecutionServer.DateMinValue,
          Party_Status = dataServices.ReturnStatusforPartyStatus(toTransformData.Activo).ToString(),
          Party_Contact_Id = -1// dataServices.GetNextId("Parties")-1
        };
      } else {
        return new PartyData {
          Party_Id = dataServices.GetPartyIdFromParties(toTransformData.Vendedor),
          Party_UID = dataServices.GetPartyUIDFromParties(toTransformData.Vendedor),
          Party_Type_Id = 153,
          Party_Code = toTransformData.Vendedor,
          Party_Name = toTransformData.Nombre,
          Party_Identificators = toTransformData.Vendedor,
          Party_Roles = Empiria.EmpiriaString.BuildKeywords(toTransformData.Vendedor, toTransformData.Nombre, toTransformData.Email),
          Party_Tags = toTransformData.Vendedor,
          Party_Ext_Data = "",
          Party_Keywords = Empiria.EmpiriaString.BuildKeywords(toTransformData.Vendedor, toTransformData.Telefono),
          Party_Historic_Id = -1,
          Party_Start_Date = ExecutionServer.DateMinValue,
          Party_End_Date = ExecutionServer.DateMaxValue,
          Party_Parent_Id = -1,
          Party_Posted_By_Id = -1,
          Party_Posting_Time = ExecutionServer.DateMinValue,
          Party_Status = dataServices.ReturnStatusforPartyStatus(toTransformData.Activo).ToString(),
          Party_Contact_Id = -1// dataServices.GetNextId("Parties")-1
        };
      }
      //toTransformData.OldBinaryChecksum != null; actualizar por que ya existe

    }

    
      private PartyData TransformSalespersonForPartyContactIdUpdate(PartySalespersonNK toTransformData) {
      string connectionString = GetEmpiriaConnectionString();
      var dataServices = new TransformerDataServices(connectionString);
      return new PartyData {
        Party_Id = dataServices.GetPartyIdFromParties(toTransformData.Vendedor),
        Party_UID = dataServices.GetPartyUIDFromParties(toTransformData.Vendedor),
        Party_Type_Id = 153,
        Party_Code = toTransformData.Vendedor,
        Party_Name = toTransformData.Nombre,
        Party_Identificators = toTransformData.Vendedor,
        Party_Roles = Empiria.EmpiriaString.BuildKeywords(toTransformData.Vendedor, toTransformData.Nombre, toTransformData.Email),
        Party_Tags = toTransformData.Vendedor,
        Party_Ext_Data = "",
        Party_Keywords = Empiria.EmpiriaString.BuildKeywords(toTransformData.Vendedor, toTransformData.Telefono),
        Party_Historic_Id = -1,
        Party_Start_Date = ExecutionServer.DateMinValue,
        Party_End_Date = ExecutionServer.DateMaxValue,
        Party_Parent_Id = -1,
        Party_Posted_By_Id = -1,
        Party_Posting_Time = ExecutionServer.DateMinValue,
        Party_Status = dataServices.ReturnStatusforPartyStatus(toTransformData.Activo).ToString(),
        Party_Contact_Id = dataServices.GetPartyIdFromParties(toTransformData.Vendedor)
      };
    }


    private PartyData TransformUsersForPartyContactIdUpdate(PartyUsersNK toTransformData) {
      string connectionString = GetEmpiriaConnectionString();
      var dataServices = new TransformerDataServices(connectionString);
        return new PartyData {
          Party_Id = dataServices.GetPartyIdFromParties(toTransformData.Usuario),
          Party_UID = dataServices.GetPartyUIDFromParties(toTransformData.Usuario),
          Party_Type_Id = 153,
          Party_Code = toTransformData.Usuario,
          Party_Name = toTransformData.Nombre,
          Party_Identificators = toTransformData.Usuario,
          Party_Roles = Empiria.EmpiriaString.BuildKeywords(toTransformData.Usuario, toTransformData.Nombre, toTransformData.Perfil),
          Party_Tags = toTransformData.Usuario,
          Party_Ext_Data = "",
          Party_Keywords = Empiria.EmpiriaString.BuildKeywords(toTransformData.Usuario),
          Party_Historic_Id = -1,
          Party_Start_Date = ExecutionServer.DateMinValue,
          Party_End_Date = ExecutionServer.DateMaxValue,
          Party_Parent_Id = -1,
          Party_Posted_By_Id = -1,
          Party_Posting_Time = ExecutionServer.DateMinValue,
          Party_Status = "A",
          Party_Contact_Id = dataServices.GetPartyIdFromParties(toTransformData.Usuario)
        };
    }


    private int ReturnIdForPartyCode(string RFC) {
      //Assertion.Require(RFC, nameof(RFC));
      //153 o 154  de types puede ser con el RFC, si comienza con 4 letras es person, con 3 es moral, 4 153 personas, 3 154 empresa
      bool first3AreLetters = RFC.Length >= 3 &&
                         Char.IsLetter(RFC[0]) &&
                         Char.IsLetter(RFC[1]) &&
                         Char.IsLetter(RFC[2]);

      // Verificar si los primeros 4 caracteres son letras
      bool first4AreLetters = RFC.Length >= 4 &&
                        Char.IsLetter(RFC[0]) &&
                        Char.IsLetter(RFC[1]) &&
                        Char.IsLetter(RFC[2]) &&
                        Char.IsLetter(RFC[3]);

      // Retornar 153 si los primeros 4 caracteres son letras, sino retornar 154 si los primeros 3 caracteres son letras
      if (first4AreLetters) {
        return 153;
      } else if (first3AreLetters) {
        return 154;
      } else {
        // Si ninguno de los casos es verdadero, podrías retornar un valor predeterminado o manejarlo de otra forma
        return -1; // O cualquier valor que consideres adecuado
      }
    }

    private void WriteTargetData(FixedList<PartyData> transformedData) {
      foreach (var item in transformedData) {
        WriteTargetData(item);
      }
    }
    private void WriteTargetData(PartyData o) {
          var op = DataOperation.Parse("write_Party", o.Party_Id, o.Party_UID, o.Party_Type_Id, o.Party_Code, o.Party_Name, o.Party_Identificators,
          o.Party_Roles, o.Party_Tags, o.Party_Ext_Data, o.Party_Keywords, o.Party_Historic_Id, o.Party_Start_Date, o.Party_End_Date, 
          o.Party_Parent_Id, o.Party_Posted_By_Id, o.Party_Posting_Time, o.Party_Status, o.Party_Contact_Id);

      DataWriter.Execute(op);
    }


    #region Helpers

    static private string GetEmpiriaConnectionString() {
      var config = ConfigurationData.Get<JsonObject>("Connection.Strings");

      return config.Get<string>("empiriaSqlServerConnection");
    }

    static private string GetNKConnectionString() {
      var config = ConfigurationData.Get<JsonObject>("Connection.Strings");

      return config.Get<string>("sqlServerConnection");
    }
 
    #endregion Helpers

  }  // class ProductTransformer

} // namespace Empiria.Trade.Integration.ETL.Transformers
