/* Empiria Trade *********************************************************************************************
*                                                                                                            *
*  Module   : Trade Integration ETL Services               Component : Services Layer                        *
*  Assembly : Empiria.Trade.Integration.ETL                Pattern   : Service provider                      *
*  Type     : ContactTransformer                           License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Transforms a User and/or SalePerson from Parties Empiria Trade To Contact Empiria Trade.       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Data;
using Empiria.Json;
using Empiria.Trade.Integration.ETL.Data;

namespace Empiria.Trade.Integration.ETL.Transformers {

  /// <summary>Transforms a User and/or SalePerson from Parties Empiria Trade To Contact Empiria Trade.</summary>
  public class ContactTransformer {

    private readonly string _connectionString;

    internal ContactTransformer(string connectionString) {
      Assertion.Require(connectionString, nameof(connectionString));

      _connectionString = connectionString;
    }

    public void Execute() {

      FixedList<PartyData> sourceData = ReadSourceData();

      FixedList<ContactData> transformedData = Transform(sourceData);

      WriteTargetData(transformedData);
    }

      private FixedList<PartyData> ReadSourceData() {
      var sql = "SELECT * FROM dbo.Parties " +
        "WHERE Party_Id >= 1571 AND Party_Id <= 1733 ";

      var connectionString = GetEmpiriaConnectionString();

      var inputDataService = new TransformerDataServices(connectionString);

      return inputDataService.ReadData<PartyData>(sql);
    }


    private FixedList<ContactData> Transform(FixedList<PartyData> toTransformData) {
      return toTransformData.Select(x => Transform(x))
                            .ToFixedList();
    }


    private ContactData Transform(PartyData toTransformData) {
      string connectionString = GetEmpiriaConnectionString();
      var dataServices = new TransformerDataServices(connectionString);
        return new ContactData {
          ContactId= toTransformData.Party_Id,
          ContactUID = toTransformData.Party_UID,
          ContactTypeId = 102,
          ContactFullName = toTransformData.Party_Name,
          ShortName = toTransformData.Party_Code,
          Initials = toTransformData.Party_Code,
          OrganizationId =1,
	        ContactEmail="",
	        ContactTags= toTransformData.Party_Tags,
          ContactExtData = toTransformData.Party_Ext_Data,
          ContactKeywords = toTransformData.Party_Keywords,
          ContactStatus = Convert.ToChar(toTransformData.Party_Status)
        };
    }


    private void WriteTargetData(FixedList<ContactData> transformedData) {
      foreach (var item in transformedData) {
        WriteTargetData(item);
      }
    }
    private void WriteTargetData(ContactData o) {
          var op = DataOperation.Parse("writeContact", o.ContactId, o.ContactUID, o.ContactTypeId, o.ContactFullName, 
            o.ShortName, o.Initials, o.OrganizationId, o.ContactEmail, o.ContactTags, o.ContactExtData, 
            o.ContactKeywords, o.ContactStatus);

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

  }  // class ContactTransformer

} // namespace Empiria.Trade.Integration.ETL.Transformers
