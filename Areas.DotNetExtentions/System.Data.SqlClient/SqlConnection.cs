using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;

    public static class SqlConnectionX
    {
		#region Properties (1) 

        public static SqlConnection Instance
        {
            get
            {                
                return new SqlConnection();
            }
        }

		#endregion Properties 

		#region Methods (4) 

		// Public Methods (4) 

        public static void CloseSafely(this SqlConnection conn)
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
            conn.Dispose();
        }

        public static void Destroy(this SqlConnection conn)
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public static string FromWebConfig(this SqlConnection shared, string name)
        {
            return WebConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public static void OpenSafely(this SqlConnection connection)
        {
            if (connection.State == ConnectionState.Closed)                
                connection.Open();
        }

		#endregion Methods 
    }

