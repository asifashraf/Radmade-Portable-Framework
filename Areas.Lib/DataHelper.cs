namespace WebAreas.Lib
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;

    public class DataHelper : IDisposable
    {
        public DataHelper(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        
        public string ConnectionString { get; set; }

        protected SqlConnection Connection { get; set; }

        protected SqlCommand Command { get; set; }

        protected void OpenConnection()
        {
            Connection = new SqlConnection(ConnectionString);
            Command = Connection.CreateCommand();
            Command.CommandType = System.Data.CommandType.Text;
            Command.Parameters.Clear();
            Command.CommandText = string.Empty;            
            this.Connection.OpenSafely();
        }

        protected void CloseConnection()
        {
            try
            {
                this.Connection.CloseSafely();
            }
            catch { }

            try
            {
                this.Command.Dispose();
            }
            catch { }

            try
            {
                this.Connection.Dispose();
            }
            catch { }
        }

        public void ExecuteQuery(string query, params object[] parametersArray)
        {
            OpenConnection();

            Command.CommandText = query;

            Command.SetCommandParametersByPairsArray(parametersArray);

            Command.ExecuteNonQuery();

            CloseConnection();
        }

        public void ExecuteStoredProcedure(string storedProcedure, params object[] parametersArray)
        {
            OpenConnection();

            Command.CommandType = System.Data.CommandType.StoredProcedure;

            Command.CommandText = storedProcedure;

            Command.SetCommandParametersByPairsArray(parametersArray);

            Command.ExecuteNonQuery();

            CloseConnection();
        }

        public object GetScalarByQuery(string query, params object[] parametersArray)
        {
            OpenConnection();

            Command.CommandText = query;

            var result = Command.ExecuteScalar();

            CloseConnection();

            return result;
        }

        public object GetScalarByStoredProcedure(string storedProcedure, params object[] parametersArray)
        {
            OpenConnection();

            Command.CommandType = System.Data.CommandType.StoredProcedure;

            Command.CommandText = storedProcedure;

            var result = Command.ExecuteScalar();

            CloseConnection();

            return result;
        }

        public DataTable GetDataTable(string query)
        {
            var adapter = new SqlDataAdapter(query, ConnectionString);

            var table = new DataTable();

            adapter.Fill(table);

            return table;
        }

        public List<T> GetTypedList<T>(string query, params object[] parameters)
        {
            this.OpenConnection();
            this.Command.CommandText = query;
            this.Command.SetCommandParametersByPairsArray(parameters);
            var reader = this.Command.ExecuteReader();            
            var result = reader.ParseToObjectList<T>(this.Connection, true);
            this.CloseConnection();
            return result;
        }

        public void Dispose()
        {
            this.CloseConnection();
        }
    }
}
