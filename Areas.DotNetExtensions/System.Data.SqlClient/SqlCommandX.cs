using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

public static class SqlCommandX
    {
        #region Properties (1)

        public static SqlCommand Instance
        {
            get
            {
                return new SqlCommand();
            }
        }

        #endregion Properties

        #region Methods (3)

        // Public Methods (3) 

        public static void SetCommandParametersByPairsArray(this SqlCommand cmd, params object[] pairs)
        {
            for (int i = 0; i < pairs.Length; i += 2)
            {
                cmd.Parameters.Add(new SqlParameter(pairs[i].ToString(),
                    pairs[i + 1]));
            }
        }

        public static int UpateBySwappedColumns(this SqlCommand com, IEnumerable<ColumnSwaper> swapList, string columnName, string otherColumnName, string tableName, string connectionString)
        {
            if (swapList.Count<ColumnSwaper>() > 0)
            {
                StringBuilder query = new StringBuilder();
                SqlConnection conn = new SqlConnection(connectionString);
                com = conn.CreateCommand();
                int count = 0;
                foreach (ColumnSwaper value in swapList)
                {
                    query.AppendLine(string.Format(" UPDATE {0} SET {1}=@NewVal{2} WHERE {3}=@WhereCol{4}; ",
                        tableName, otherColumnName, count.ToString(), columnName, count.ToString()));
                    com.Parameters.AddWithValue(string.Format("@NewVal{0}", count.ToString()), value.SwappedValue);
                    com.Parameters.AddWithValue(string.Format("@WhereCol{0}", count.ToString()), value.ColumnValue);
                    count++;
                }
                count = 0;
                com.CommandType = CommandType.Text;
                com.CommandText = query.ToString();
                conn.OpenSafely();
                int result = com.ExecuteNonQuery();
                return result;
            }
            return 0;
        }

        public static int UpateBySwappedColumns<IdType, ColType, SwaperType>(this SqlCommand com,
            IEnumerable<ColumnSwaper<IdType, ColType, SwaperType>> swapList, string columnName, string otherColumnName, string tableName, string connectionString)
        {
            if (swapList.Count<ColumnSwaper<IdType, ColType, SwaperType>>() > 0)
            {
                StringBuilder query = new StringBuilder();
                SqlConnection conn = new SqlConnection(connectionString);
                com = conn.CreateCommand();
                int count = 0;
                foreach (ColumnSwaper<IdType, ColType, SwaperType> value in swapList)
                {
                    query.AppendLine(string.Format(" UPDATE {0} SET {1}=@NewVal{2} WHERE {3}=@WhereCol{4}; ",
                        tableName, otherColumnName, count.ToString(), columnName, count.ToString()));
                    com.Parameters.AddWithValue(string.Format("@NewVal{1}", count.ToString()), value.SwappedValue);
                    com.Parameters.AddWithValue(string.Format("@WhereCol{1}", count.ToString()), value.ColumnValue);
                    count++;
                }
                count = 0;
                com.CommandType = CommandType.Text;
                com.CommandText = query.ToString();
                conn.OpenSafely();
                int result = com.ExecuteNonQuery();
                return result;
            }
            return 0;
        }

        #endregion Methods
        
        #region Get methods
        /// <summary>
        /// execute a reader
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="connectionString"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="conn"></param>
        /// <param name="paramPairs"></param>
        /// <returns></returns>
        public static SqlDataReader GetReader(
            this SqlCommand instance,
            string connectionString,
            string commandText,
            CommandType commandType,
            out SqlConnection conn,
            params object[] paramPairs)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            for (int i = 0; i < paramPairs.Length; i += 2)
            {
                parameters.Add(new SqlParameter(paramPairs[i].ToString(),
                    paramPairs[i + 1]));
            }
            return instance.GetReader(connectionString, commandText,
                commandType, out conn, parameters.ToArray());
        }

        /// <summary>
        /// execute a reader
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="providingParams"></param>
        /// <param name="connectionString"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="conn"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static SqlDataReader GetReader(
            this SqlCommand instance,

            string connectionString,
            string commandText,
            CommandType commandType,
            out SqlConnection conn,
            SqlParameter[] parameters)
        {
            conn = new SqlConnection(connectionString);
            instance.Connection = conn;
            instance.CommandType = commandType;
            instance.CommandText = commandText;
            instance.Parameters.AddRange(parameters);
            conn.OpenSafely();
            SqlDataReader reader = instance.ExecuteReader();
            return reader;			
        }

        /// <summary>
        /// Get a scalar value
        /// </summary>
        /// <param name="com"></param>
        /// <param name="connectionString"></param>
        /// <param name="ScalarQuery"></param>
        /// <param name="commandType"></param>
        /// <param name="paramPairs"></param>
        /// <returns></returns>
        public static object GetScalarValue(
            this SqlCommand com,
            string connectionString,
            string ScalarQuery,
            CommandType commandType,
            params object[] paramPairs)
        {
            List<SqlParameter> ps = new List<SqlParameter>();
            for (int i = 0; i < paramPairs.Length; i += 2)
            {
                ps.Add(new SqlParameter(paramPairs[i].ToString(), paramPairs[i + 1]));
            }
            return com.GetScalarValue(true, connectionString, ScalarQuery,
                commandType, ps.ToArray());
        }

        /// <summary>
        /// get a scalar value
        /// </summary>
        /// <param name="com"></param>
        /// <param name="providingParams"></param>
        /// <param name="connectionString"></param>
        /// <param name="ScalarQuery"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object GetScalarValue(
            this SqlCommand com,
            bool providingParams,
            string connectionString,
            string ScalarQuery,
            CommandType commandType,
            SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            com.Connection = conn;
            com.CommandType = commandType;
            com.Parameters.Clear();
            com.Parameters.AddRange(parameters);
            com.CommandText = ScalarQuery;
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            //
            object R = com.ExecuteScalar();
            //
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            return R;
        }

        /// <summary>
        /// rad binary large object
        /// </summary>
        /// <param name="com"></param>
        /// <param name="scalarQuery"></param>
        /// <param name="connectionString"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static Byte[] ReadBlob(this SqlCommand com, string scalarQuery, string connectionString,
            params object[] parameters)
        {
            //initialize blob
            Byte[] blob = null;

            //connection
            SqlConnection conn;

            //create reader
            SqlDataReader sdr = SqlCommandX.Instance.GetReader(connectionString,
                scalarQuery, CommandType.Text, out conn, parameters);

            //read
            sdr.Read();

            blob = new Byte[sdr.GetBytes(0, 0, null, 0, int.MaxValue)];
            sdr.GetBytes(0, 0, blob, 0, blob.Length);
            sdr.Close();
            try
            {
                conn.Close();
            }
            catch { }
            return blob;
        }

        /// <summary>
        /// swap one unique column with the other
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="com"></param>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="swapperColName"></param>
        /// <param name="columnValues"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static List<ColumnSwaper<IdType, ColType, SwaperType>> GetSwapperColumn<IdType, ColType, SwaperType>(
            this SqlCommand com, string tableName, string columnName,
            string swapperColName, List<ColumnSwaper<IdType, ColType, SwaperType>> columnValues, string connectionString, string additionalWhereExpression, params SqlParameter[] additionalWhereParams)
        {
            List<ColumnSwaper<IdType, ColType, SwaperType>> list = new List<ColumnSwaper<IdType, ColType, SwaperType>>();
            if (columnValues.Count<ColumnSwaper<IdType, ColType, SwaperType>>() > 0)
            {
                StringBuilder query = new StringBuilder(string.Format("Select {0},{1} from {2} Where (",
                    columnName, swapperColName, tableName));
                SqlConnection conn = new SqlConnection(connectionString);
                com = conn.CreateCommand();
                int count = 0;
                foreach (ColumnSwaper<IdType, ColType, SwaperType> value in columnValues)
                {
                    query.Append(string.Format("({0}=@Param{1}) OR", columnName, count.ToString()));
                    com.Parameters.AddWithValue(string.Format("@Param{0}", count.ToString()), value.ColumnValue);

                    count++;
                }
                count = 0;
                string q = query.ToString();
                q = "".TrimEnd(q, "OR", false);
                q += ")";
                //add additional where query
                if (!string.IsNullOrEmpty(additionalWhereExpression))
                {
                    q += string.Format(" AND {0}", additionalWhereExpression);
                    foreach (SqlParameter p in additionalWhereParams)
                    {
                        com.Parameters.Add(p);
                    }
                }
                com.CommandType = CommandType.Text;
                com.CommandText = q;
                conn.OpenSafely();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    ColType t1 = default(ColType); SwaperType t2 = default(SwaperType);
                    if (reader[columnName] != null) t1 = (ColType)reader[columnName];
                    if (reader[swapperColName] != null) t2 = (SwaperType)reader[swapperColName];
                    list.Add(new ColumnSwaper<IdType, ColType, SwaperType> { ColumnValue = t1, SwappedValue = t2 });
                }
                reader.Close();
                reader.Dispose();
                conn.CloseSafely();

                foreach (ColumnSwaper<IdType, ColType, SwaperType> swapped in list)
                {
                    IEnumerable<ColumnSwaper<IdType, ColType, SwaperType>> listSwapFromMainList = from s in columnValues
                                                                                                  where "".MatchByString(s.ColumnValue, swapped.ColumnValue)
                                                                                                  select s;
                    foreach (ColumnSwaper<IdType, ColType, SwaperType> swapFromMainList in listSwapFromMainList)
                    {
                        swapFromMainList.SwappedValue = swapped.SwappedValue;
                    }
                }
                return columnValues;
            }
            return list;
        }

        public static List<ColumnSwaper> GetSwapperColumn(this SqlCommand com, string tableName,
            string columnName, string swapperColName, List<ColumnSwaper> columnValues,
            string connectionString, string additionalWhereExpression, params SqlParameter[] additionalWhereParams)
        {
            List<ColumnSwaper> list = new List<ColumnSwaper>();
            if (columnValues.Count<ColumnSwaper>() > 0)
            {
                StringBuilder query = new StringBuilder(string.Format("Select {0},{1} from {2} Where (",
                    columnName, swapperColName, tableName));
                SqlConnection conn = new SqlConnection(connectionString);
                com = conn.CreateCommand();
                int count = 0;
                foreach (ColumnSwaper value in columnValues)
                {
                    query.Append(string.Format("({0}=@Param{1}) OR", columnName, count.ToString()));
                    com.Parameters.AddWithValue(string.Format("@Param{0}", count.ToString()), value.ColumnValue);

                    count++;
                }
                count = 0;
                string q = query.ToString();
                q = "".TrimEnd(q, "OR", false);
                q += ")";
                //add additional where query
                if (!string.IsNullOrEmpty(additionalWhereExpression))
                {
                    q += string.Format(" AND {0}", additionalWhereExpression);
                    foreach (SqlParameter p in additionalWhereParams)
                    {
                        com.Parameters.Add(p);
                    }
                }

                com.CommandType = CommandType.Text;
                com.CommandText = q;
                conn.OpenSafely();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    object t1 = null;
                    object t2 = null;
                    if (reader[columnName] != null) t1 = reader[columnName];
                    if (reader[swapperColName] != null) t2 = reader[swapperColName];
                    list.Add(new ColumnSwaper { ColumnValue = t1, SwappedValue = t2 });
                }
                reader.Close();
                reader.Dispose();
                conn.CloseSafely();

                foreach (ColumnSwaper swapped in list)
                {
                    IEnumerable<ColumnSwaper> listSwapFromMainList = from s in columnValues
                                                                     where "".MatchByString(s.ColumnValue, swapped.ColumnValue)
                                                                     select s;
                    foreach (ColumnSwaper swapFromMainList in listSwapFromMainList)
                    {
                        swapFromMainList.SwappedValue = swapped.SwappedValue;
                    }
                }
                return columnValues;
            }
            return list;
        }
        static void Append(StringBuilder sb, string text, params object[] parameters)
        {
            //    string[] arrStrings = new string[parameters.Length];
            //    for (int i = 0; i < parameters.Length; i++)
            //    {
            //        arrStrings[i] = parameters[i].ToString();
            //    }
            sb.Append(string.Format(text, parameters));
            //arrStrings = null;
            //text = null;
            //parameters = null;
            parameters = null;
        }
        public static object GetSwappedValue(
            this SqlCommand command,
            string tableName,
            string columnName,
            object columnValue,
            string swappedColumnName,
            string connectionString,
            string whereClause,
            params object[] whereParameters)
        {
            StringBuilder sb = new StringBuilder();
            Append(sb,"SELECT {0} FROM {1} WHERE {2}=@ColumnValue {3}",
                swappedColumnName,
                tableName,
                columnName,
                !String.IsNullOrEmpty(whereClause) ?
                String.Format(" AND {0}", whereClause)
                : String.Empty);
            SqlConnection conn = new SqlConnection(connectionString);
            command = conn.CreateCommand();
            command.CommandText = sb.ts();
            command.SetCommandParametersByPairsArray(whereParameters);
            command.Parameters.AddWithValue("@ColumnValue", columnValue);
            //start operation
            conn.OpenSafely();
            object result = command.ExecuteScalar();
            conn.CloseSafely();
            return result;
        }

        /// <summary>
        /// check which values exist in db and which are not
        /// </summary>
        /// <param name="com"></param>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="values"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static List<FoundColumn<int>> GetFoundColumnsM(this SqlCommand com, string tableName, string columnName,
            IEnumerable<int> values, string connectionString)
        {
            List<FoundColumn<int>> list = new List<FoundColumn<int>>();
            if (values.Count<int>() > 0)
            {
                SqlConnection conn = new SqlConnection(connectionString);

                com = conn.CreateCommand();

                StringBuilder sb = new StringBuilder(string.Format("Select {0} from {1} Where {2} IN(", columnName, tableName, columnName));
                foreach (int i in values)
                {
                    sb.Append(i.ToString());
                    sb.Append(",");
                }
                string q = sb.ToString();
                q = q.TrimEnd(',');
                q += ")";

                com.CommandType = CommandType.Text;
                com.CommandText = q;
                conn.OpenSafely();
                SqlDataReader reader = com.ExecuteReader();
                List<int> found = new List<int>();
                while (reader.Read())
                {
                    found.Add(Convert.ToInt32(reader[columnName].ToString()));
                }

                foreach (int i in values)
                {
                    IEnumerable<int> fq = from f in found
                                          where f == i
                                          select f;
                    if (fq.Count<int>() > 0)
                    {
                        list.Add(new FoundColumn<int> { ColumnValue = i, IsFound = true });
                    }
                    else
                    {
                        list.Add(new FoundColumn<int> { ColumnValue = i, IsFound = false });
                    }
                }

                reader.Close();
                reader.Dispose();
                conn.CloseSafely();
            }
            return list;

        }

        /// <summary>
        /// check which values exist in db and which are not
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="com"></param>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="values"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static List<FoundColumn<T>> GetFoundColumnsM<T>(this SqlCommand com, string tableName, string columnName,
            IEnumerable<T> values, string connectionString)
        {
            List<FoundColumn<T>> list = new List<FoundColumn<T>>();
            if (values.Count<T>() > 0)
            {
                SqlConnection conn = new SqlConnection(connectionString);

                com = conn.CreateCommand();

                StringBuilder sb = new StringBuilder(string.Format("Select {0} from {1} Where ", columnName, tableName));
                int c = 0;
                foreach (object v in values)
                {
                    sb.Append(string.Format("({0}=@Param{1}) OR", columnName, c.ToString()));

                    com.Parameters.AddWithValue(string.Format("@Param{0}", c.ToString()), v);
                    c++;
                }
                string q = sb.ToString();
                q = "".TrimEnd(q, "OR", false);

                com.CommandType = CommandType.Text;
                com.CommandText = q;
                conn.OpenSafely();
                SqlDataReader reader = com.ExecuteReader();
                List<T> found = new List<T>();
                while (reader.Read())
                {
                    found.Add((T)reader[columnName]);
                }

                foreach (T i in values)
                {
                    IEnumerable<T> fq = from f in found
                                        where f.ToString().ToLower().Trim() == i.ToString().ToLower().Trim()
                                        select f;
                    if (fq.Count<T>() > 0)
                    {
                        list.Add(new FoundColumn<T> { ColumnValue = i, IsFound = true });
                    }
                    else
                    {
                        list.Add(new FoundColumn<T> { ColumnValue = i, IsFound = false });
                    }
                }

                reader.Close();
                reader.Dispose();
                conn.CloseSafely();
            }
            return list;

        }

        public static SqlDataReader GetPagedReader(this SqlCommand com,
            string orderbyClause,
            string tableName,
            string whereClause,
            int skip,
            int take,
            string connectionString,
            out SqlConnection connectionBeingUsed,
            out int virtualItemsCount,
            params object[] whereClauseParameters_Pairs)
        {
            if (!String.IsNullOrEmpty(whereClause)) 
            {
                whereClause = whereClause.ToLower().Contains("where") ?
                whereClause : string.Format(" WHERE {0}", whereClause);
            }

            if (!String.IsNullOrEmpty(orderbyClause))
            {
                orderbyClause = orderbyClause.ToLower().Contains("order") ?
                string.Format("({0})", orderbyClause)
                : string.Format("(ORDER BY {0})", orderbyClause);
            }
            #region count virtual items
            SqlConnection vCon = new SqlConnection(connectionString);
            SqlCommand vCom = vCon.CreateCommand();
            vCom.CommandText = string.Format("SELECT Count(*) FROM {0} {1}",
                tableName,
                whereClause);
            vCom.SetCommandParametersByPairsArray(whereClauseParameters_Pairs);
            vCon.OpenSafely();
            virtualItemsCount = int.Parse(vCom.ExecuteScalar().ts());
            vCon.CloseSafely();
            #endregion
            string query = string.Format("SELECT * FROM (SELECT ROW_NUMBER() OVER {0} AS [ROW_NUMBER], * FROM {1} {2}) AS [t1] WHERE [t1].[ROW_NUMBER] BETWEEN @p3 + 1 AND @p3 + @p4 ORDER BY [t1].[ROW_NUMBER]",
                orderbyClause, tableName, whereClause);
            connectionBeingUsed = new SqlConnection(connectionString);
            com = connectionBeingUsed.CreateCommand();
            com.CommandText = query;
            com.SetCommandParametersByPairsArray(whereClauseParameters_Pairs);
            com.Parameters.AddWithValue("@p3", skip);
            com.Parameters.AddWithValue("@p4", take);
            connectionBeingUsed.OpenSafely();
            return com.ExecuteReader();
        }



        public static SqlDataReader GetPagedReader(this SqlCommand com,
        int skip,
        int take,
        string tableName,
        string commaDelimitedColumnNames,
        string whereClause,
        string orderbyClause,
       string connectionString,
       out SqlConnection connectionBeingUsed,
       out int virtualItemsCount,
       params object[] whereClauseParameters_Pairs)
        {
            if (!String.IsNullOrEmpty(whereClause))
            {
                whereClause = whereClause.Contains("where") ?
                whereClause : string.Format(" WHERE {0}", whereClause);
            }

            if (!String.IsNullOrEmpty(orderbyClause))
            {
                orderbyClause = orderbyClause.Contains("order") ?
                string.Format("({0})", orderbyClause)
                : string.Format("(ORDER BY {0})", orderbyClause);
            }
            #region count virtual items
            SqlConnection vCon = new SqlConnection(connectionString);
            SqlCommand vCom = vCon.CreateCommand();
            vCom.CommandText = string.Format("SELECT Count(*) FROM {0} {1}",
                tableName,
                whereClause);
            vCom.SetCommandParametersByPairsArray(whereClauseParameters_Pairs);
            vCon.OpenSafely();
            virtualItemsCount = int.Parse(vCom.ExecuteScalar().ts());
            vCon.CloseSafely();
            #endregion
            string query = string.Format("SELECT {3} FROM (SELECT ROW_NUMBER() OVER {0} AS [ROW_NUMBER], {3} FROM {1} {2}) AS [t1] WHERE [t1].[ROW_NUMBER] BETWEEN @p3 + 1 AND @p3 + @p4 ORDER BY [t1].[ROW_NUMBER]",
                orderbyClause, tableName, whereClause, commaDelimitedColumnNames);
            connectionBeingUsed = new SqlConnection(connectionString);
            com = connectionBeingUsed.CreateCommand();
            com.CommandText = query;
            com.SetCommandParametersByPairsArray(whereClauseParameters_Pairs);
            com.Parameters.AddWithValue("@p3", skip);
            com.Parameters.AddWithValue("@p4", take);
            connectionBeingUsed.OpenSafely();
            return com.ExecuteReader();
        }

        public static List<T> GetPagedList<T>(this SqlCommand com,
            string orderbyClause,
            string tableName,
            string whereClause,
            int skip,
            int take,
            out int virtualItemsCount,
            string connectionString,
            params object[] whereClauseParameters_Pairs)
        {
            SqlConnection connectionBeingUsed = new SqlConnection();
            SqlDataReader reader = com.GetPagedReader(orderbyClause,
                tableName,
                whereClause,
                skip,
                take,
                connectionString,
                out connectionBeingUsed,
                out virtualItemsCount,
                whereClauseParameters_Pairs);
            return reader.ParseToObjectList<T>(connectionBeingUsed);
        }

        public static List<T> GetPagedList<T>(this SqlCommand com,
        string orderbyClause,
        string tableName,
        string commaDelimitedColumnNames,
        string whereClause,
        int skip,
        int take,
        out int virtualItemsCount,
        string connectionString,
        params object[] whereClauseParameters_Pairs)
        {
            SqlConnection connectionBeingUsed = new SqlConnection();
            SqlDataReader reader = com.GetPagedReader(
                skip, take, tableName, commaDelimitedColumnNames, whereClause, orderbyClause, connectionString,
                out connectionBeingUsed, out virtualItemsCount, whereClauseParameters_Pairs
                );
            return reader.ParseToObjectList<T>(connectionBeingUsed);
        }

        public static List<T> GetTypedList<T>(this SqlCommand com,
            string commandText,
            CommandType commandType,
            string connectionString,
            params object[] whereClauseParameters_Pairs)
        {
            SqlConnection connectionBeingUsed = new SqlConnection();
            SqlDataReader reader = com.GetReader(
                connectionString,
                commandText,
                commandType,
                out connectionBeingUsed,
                whereClauseParameters_Pairs);
            return reader.ParseToObjectList<T>(connectionBeingUsed);
        }
        #endregion

        #region Delete
        public static int BulkDelete(this SqlCommand com,
            string tableName,
            string columnName,
            IEnumerable<int> values,
            string connectionString)
        {
            if (values.Count<int>() > 0)
            {
                string query = string.Format("Delete from {0} where {1} IN (", tableName, columnName);
                query += values.ToCommaSeparatedString<int>();
                query += ")";

                SqlConnection conn = new SqlConnection(connectionString);
                com = conn.CreateCommand();
                com.CommandType = CommandType.Text;
                com.CommandText = query;
                conn.OpenSafely();
                int result = com.ExecuteNonQuery();
                conn.CloseSafely();
                return result;
            }
            return 0;
        }

        public static int BulkDelete(this SqlCommand com,
            string tableName, string columnName, IEnumerable<string> values, string connectionString)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            com = conn.CreateCommand();
            com.CommandType = CommandType.Text;

            List<string> listValues = values.ToListSafely<string>();
            if (values.Count<string>() > 0)
            {
                string query = string.Format("Delete from {0} where ", tableName);
                for (int i = 0; i < listValues.Count; i++)
                {
                    string v = listValues[i];
                    query += string.Format("({0}=@Param{1}) OR", columnName, i.ToString());
                    com.Parameters.AddWithValue(string.Format("@Param{0}", i.ToString()), v);
                }
                query = "".TrimEnd(query, "OR", false);


                com.CommandText = query;
                conn.OpenSafely();
                int result = com.ExecuteNonQuery();
                conn.CloseSafely();
                return result;
            }
            return 0;
        }

        public static int Delete(this SqlCommand com,
            string tableName, string columnName, object value, string connectionString)
        {
            string query = string.Format("Delete from {0} Where {1}=@Param1", tableName, columnName);
            SqlConnection conn = new SqlConnection(connectionString);
            com = conn.CreateCommand();
            com.CommandType = CommandType.Text;
            com.CommandText = query;
            com.Parameters.AddWithValue("@Param1", value);
            conn.OpenSafely();
            int result = com.ExecuteNonQuery();
            conn.CloseSafely();
            return result;
        }

        public static int Delete(this SqlCommand com,
            string tableName, string connectionString, string whereClause, params object[] ColumnValuePairs)
        {
            string query = string.Format("Delete from {0} ", tableName);//delete from table
            if (!String.IsNullOrEmpty(whereClause))
            {
                whereClause = whereClause.Contains("where") ?
                whereClause : string.Format(" WHERE {0}", whereClause);
            }
            query = query + whereClause; //add where
            for (int i = 0; i < ColumnValuePairs.Count<object>(); i = i + 2)
            {
                string columnName = ColumnValuePairs[i].ToString();
                object value = ColumnValuePairs[i + 1];
                query += string.Format("({0}=@Param{1}) AND", columnName, i.ToString());
                com.Parameters.AddWithValue(string.Format("@Param{0}", i.ToString()), value);
            }
            SqlConnection conn = new SqlConnection(connectionString);
            com = conn.CreateCommand();
            com.CommandType = CommandType.Text;
            com.CommandText = query;
            conn.OpenSafely();
            int result = com.ExecuteNonQuery();
            conn.CloseSafely();
            return result;
        }
        #endregion

        public static int static_ExecuteStoredProcedure(
            string procedureName, string connectionString, bool useTryCatch, params SqlParameter[] commandParameters)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = procedureName;
            foreach (SqlParameter pm in commandParameters)
            {
                command.Parameters.Add(pm);
            }
            connection.OpenSafely();
            int result = 0;
            if (useTryCatch)
            {
                try { result = command.ExecuteNonQuery(); }
                catch (Exception error) { Console.WriteLine(error.ToString()); }
            }
            else
            {
                result =  command.ExecuteNonQuery();
            }
            connection.CloseSafely();
            command.Dispose();
            connection.Dispose();
            return result;
        }
        public static int ExecuteStoredProcedure(this SqlCommand cmd,
            string procedureName, string connectionString, bool useTryCatch, params SqlParameter[] commandParameters)
        {
            return static_ExecuteStoredProcedure(procedureName, connectionString, useTryCatch, commandParameters);
        }
    }

    public class ColumnSwaper<IdType, ColumnType, SwapperType>
    {
        #region Properties (3)

        public IdType ClientIndex { get; set; }

        public ColumnType ColumnValue { get; set; }

        public SwapperType SwappedValue { get; set; }

        #endregion Properties
    }

    public class ColumnSwaper
    {
        #region Properties (3)

        public object ClientIndex { get; set; }

        public object ColumnValue { get; set; }

        public object SwappedValue { get; set; }

        #endregion Properties
    }

    public static class ColumnSwapperExtendedMethods
    {
        #region Methods (8)

        // Public Methods (8) 

        public static IEnumerable<ColumnSwaper> GetByClientId(
            this IEnumerable<ColumnSwaper> list, object id)
        {
            return from s in list
                   where "".MatchByString(s.ClientIndex, id)
                   select s;
        }

        public static IEnumerable<ColumnSwaper<IdType, ColType, SwaperType>> GetByClientId<IdType, ColType, SwaperType>(
            this IEnumerable<ColumnSwaper<IdType, ColType, SwaperType>> list, IdType id)
        {
            return from s in list
                   where "".MatchByString(s.ClientIndex, id)
                   select s;
        }

        public static object GetColumnValue(this IEnumerable<ColumnSwaper> list, object swappedValue)
        {
            IEnumerable<object> query = from s in list
                                        where "".MatchByString(s.SwappedValue, swappedValue)
                                        select s.ColumnValue;
            if (query.Count<object>() > 0)
                return query.First<object>();
            else return null;
        }

        public static ColType GetColumnValue<IdType, ColType, SwaperType>(this IEnumerable<ColumnSwaper<IdType, ColType, SwaperType>> list, SwaperType swappedValue)
        {
            IEnumerable<ColType> query = from s in list
                                         where "".MatchByString(s.SwappedValue, swappedValue)
                                         select s.ColumnValue;
            if (query.Count<ColType>() > 0)
                return query.First<ColType>();
            else return default(ColType);
        }

        public static ColumnSwaper GetSingleByClientId(
            this IEnumerable<ColumnSwaper> list, object id)
        {
            try
            {
                return (from s in list
                        where "".MatchByString(s.ClientIndex, id)
                        select s).First<ColumnSwaper>();
            }
            catch
            {
                return null;
            }
        }

        public static ColumnSwaper<IdType, ColType, SwaperType> GetSingleByClientId<IdType, ColType, SwaperType>(
            this IEnumerable<ColumnSwaper<IdType, ColType, SwaperType>> list, IdType id)
        {
            try
            {
                return (from s in list
                        where "".MatchByString(s.ClientIndex, id)
                        select s).First<ColumnSwaper<IdType, ColType, SwaperType>>();
            }
            catch
            {
                return null;
            }
        }

        public static object GetSwappedValue(this IEnumerable<ColumnSwaper> list, object columnValue)
        {
            IEnumerable<object> query = from s in list
                                        where "".MatchByString(s.ColumnValue, columnValue)
                                        select s.SwappedValue;
            if (query.Count<object>() > 0)
                return query.First<object>();
            else return null;
        }

        public static SwaperType GetSwappedValue<IdType, ColType, SwaperType>(this IEnumerable<ColumnSwaper<IdType, ColType, SwaperType>> list, ColType columnValue)
        {
            IEnumerable<SwaperType> query = from s in list
                                            where "".MatchByString(s.ColumnValue, columnValue)
                                            select s.SwappedValue;
            if (query.Count<SwaperType>() > 0)
                return query.First<SwaperType>();
            else return default(SwaperType);
        }

        #endregion Methods
    }

    public class FoundColumn<T>
    {
        #region Properties (2)

        public T ColumnValue { get; set; }

        public bool IsFound { get; set; }

        #endregion Properties
    }

