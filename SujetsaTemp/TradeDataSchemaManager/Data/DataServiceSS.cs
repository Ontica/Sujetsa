using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace TradeDataSchemaManager.Data
{
    internal class DataServiceSS
    {
        private static DataServiceSS Con = null;
        public SqlConnection CreateConnection(string pSSConnection)
        {
            if (string.IsNullOrEmpty(pSSConnection))
            {
                throw new ArgumentException("La cadena de conexión no puede ser nula o vacía.", nameof(pSSConnection));
            }
            return new SqlConnection(pSSConnection);
        }
        public static DataServiceSS getInstance()
        {
            if (Con == null)
            {
                Con = new DataServiceSS();
            }
            return Con;
        }

        public DataTable GetTablesList(string pSSConnection)
        {
            try
            {
                using (SqlConnection connectionSS = DataServiceSS.getInstance().CreateConnection(pSSConnection))
                using (SqlCommand cmd = new SqlCommand("SELECT SourceTable FROM sources.OMS_Intermediate_Tables_List WHERE Active = 'T'", connectionSS))
                {
                    connectionSS.Open();
                    using (var dataReader = cmd.ExecuteReader())
                    {
                        var dataTable = new DataTable();
                        dataTable.Load(dataReader);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de tablas de sources.OMS_Intermediate_Tables_List en Sql Server.", ex);
            }
        }
        public int ExecuteMergeStoredProcedure(string pSSConnection)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connectionSS = DataServiceSS.getInstance().CreateConnection(pSSConnection))
                using (SqlCommand cmd = new SqlCommand("sources.OMS_Merge_Intermediate_Tables", connectionSS))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    connectionSS.Open();
                    rowsAffected = cmd.ExecuteNonQuery();
                }

                return rowsAffected;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar procedimiento almacenado", ex);
            }
        }
        public string GetTableToTruncate(string ptableName, string pSSConnection)
        {
            try
            {
                using (SqlConnection connectionSS = DataServiceSS.getInstance().CreateConnection(pSSConnection))
                using (SqlCommand cmd = new SqlCommand("SELECT FullSourceTableName FROM sources.OMS_Intermediate_Tables_List WHERE SourceTable = @SourceTable AND Active = 'T'", connectionSS))
                {
                    cmd.Parameters.AddWithValue("@SourceTable", ptableName);

                    connectionSS.Open();

                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.Read())
                        {
                            return dataReader["FullSourceTableName"].ToString();
                        }
                    }

                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al truncar la tabla {ptableName}. Detalles del error: {ex.Message}", ex);
            }
        }

    }
}
