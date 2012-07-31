using System.Data.SqlClient;
using System.Data;

    public static class SqlDataAdapterX
    {
		#region Properties (1) 

        public static SqlDataAdapter Instance
        {
            get
            {
                return new SqlDataAdapter();
            }
        }

		#endregion Properties 

		#region Methods (1) 

		// Public Methods (1) 

        public static DataTable GetTable(
            this SqlDataAdapter adapter,
            string connectionString,
            string queryText,            
            params object[] paramPairs)
        {
            DataTable R = new DataTable("Table1");
            SqlConnection conn = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter(queryText, conn);
            adapter.SelectCommand.Parameters.Clear();
            for (int i = 0; i < paramPairs.Length; i += 2)
            {
                adapter.SelectCommand.Parameters.Add(new SqlParameter(paramPairs[i].ToString(), paramPairs[i + 1]));
            }
            adapter.Fill(R);
            return R;
        }

		#endregion Methods 
    }

