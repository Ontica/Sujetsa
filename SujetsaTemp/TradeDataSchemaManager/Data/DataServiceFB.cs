using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;



namespace TradeDataSchemaManager.Data
{
    internal class DataServiceFB
    {
        private static DataServiceFB Con = null;
        public FbConnection CreateConnection(string pFBConnection)
        {
            if (string.IsNullOrEmpty(pFBConnection))
            {
                throw new ArgumentException("La cadena de conexión no puede ser nula o vacía.", nameof(pFBConnection));
            }
            return new FbConnection(pFBConnection);
        }

        public static DataServiceFB getInstance()
        {
            if (Con == null)
            {
                Con = new DataServiceFB();
            }
            return Con;
        }

        public DataTable ReadDataFromTable(string pSelectToFB, string pFBConnection)
        {
            try
            {
                using (FbConnection SqlConFB = DataServiceFB.getInstance().CreateConnection(pFBConnection))
                using (FbDataAdapter dataAdapter = new FbDataAdapter(pSelectToFB, SqlConFB))
                {
                    SqlConFB.Open();
                    var dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en ReadDataFromTable FB con la consulta: {pSelectToFB}", ex);
            }
        }

    }
}
