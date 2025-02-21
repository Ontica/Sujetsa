using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace TradeDataSchemaManager.Services
{
    public class SchemaServicesFBSS
    {
        HelperFBSS helper = new HelperFBSS();
        public SchemaServicesFBSS(bool isTest)
        {                  
            dynamic config = helper.GetConnectionsInfo(isTest);
            string SSConnection = config.ConnectionStrings.SSConnection;
            string FBConnection = config.ConnectionStrings.FBConnection;
            helper.ETL(SSConnection, FBConnection);
            //SchemaServicesFBSS ETL = new SchemaServicesFBSS(false);//in main
        }
       
        
    }
}