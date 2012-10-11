using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
namespace Areas.Lib.InformationSchema
{
    public class InfoSchema
    {
        #region Fields
        private string _connectionString;
        private List<TableInfo> _tables = new List<TableInfo>();
        private List<ViewInfo> _views = new List<ViewInfo>();
        private List<ConstraintInfo> _constraints = new List<ConstraintInfo>();
        List<TableColumn> _tableColumns = new List<TableColumn>();
        List<ViewColumn> _viewColumns = new List<ViewColumn>();
        #endregion Fields

        public DataHelper db { get; set; }
        #region Constructors
        /// <summary>
        /// By default loads all data immediately unless second param is sent as false
        /// </summary>
        /// <param name="connectionString">db connectionstring</param>
        /// <param name="completeLoadImmediately">load immediately or late</param>
        public InfoSchema(string connectionString)
        {
            _connectionString = connectionString;
            LoadComplete();
        }   
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString">connectionstring</param>
        /// <param name="completeLoadImmediately">load immediately or late</param>        
        public InfoSchema(
            string connectionString,
           string[] tablesFilter, string[] viewsFilter, bool completeLoadImmediately = true)
        {
            _connectionString = connectionString;
            this.SetTableFilter(tablesFilter);
            this.SetViewFilter(viewsFilter);
            if (completeLoadImmediately)
            {
                this.LoadComplete();
            }
        }
        #endregion Constructors
        
        #region load methods        
        public void LoadTables()
        {
            this.Tables = GetTables(this.Connectionstring, TableFilters.ToArray<string>()); // load tables
        }
        public void LoadViews()
        {
            this.Views = GetViews(this.Connectionstring, ViewFilters.ToArray<string>()); // load views
        }
        public void LoadConstraints()
        {
            this.Constraints = GetConstraints(this.Connectionstring, TableFilters.ToArray<string>());
        }
        public void LoadTableColumns()
        {
            this.TableColumns = GetTableColumns(this.Connectionstring, TableFilters.ToArray<string>());
            foreach (TableInfo t in this.Tables)
            {
                t.Columns = (from c in this.TableColumns
                             where c.TableName == t.Name
                             select c).ToList<TableColumn>();

                t.PKConstraints = (from cn in this.Constraints
                                   where cn.table_name == t.Name
                                   && cn.constraint_type == "PRIMARY KEY"
                                   select cn).ToList<ConstraintInfo>();

                //load description for columns
                string query = @"select  [value] as [Description],COL_NAME(major_id,minor_id) as ColumnName  
from sys.extended_properties xp   
where xp.class = 1 and  xp.minor_id > 0 and 
xp.major_id = object_id(N'[[schema]].[[table]]') 
and xp.name in (N'MS_Description')"
                    .Replace("[[schema]]", t.SchemaName).Replace("[[table]]", t.Name);
                SqlConnection conn;
                using (IDataReader reader = SqlCommandX.Instance.GetReader(
                     _connectionString, query, CommandType.Text, out conn))
                {
                    while (reader.Read())
                    {
                        var col = reader["ColumnName"].Text();
                        var desc = reader["Description"].Text();
                        if (col.IsNotNullOrEmpty())
                        {
                            var column = (from c in t.Columns where c.Name.ToLower() == col.ToLower() select c).One();
                            if (column.IsNotNull())
                            {
                                column.Description = desc;
                            }
                        }                        
                    }
                }
                conn.Destroy();
            }
        }
        public void LoadViewColumns()
        {
            this.ViewColumns = GetViewColumns(this.Connectionstring, ViewFilters.ToArray<string>());
            foreach (ViewInfo view in this.Views)
            {
                view.Columns = (from c in this.ViewColumns
                             where c.ViewName.ToLower() == view.Name.ToLower()
                             select c).ToList<ViewColumn>();
            }
        }
        public void LoadComplete()
        {
            LoadTables();
            LoadViews();
            LoadConstraints();
            LoadTableColumns();
            LoadViewColumns();
        }

        
        #endregion

        #region Properties
        List<SchemaInfo> schemas;
        public List<SchemaInfo> Schemas 
        {
            get 
            {
                if (null == schemas)
                {
                    schemas = db.GetTypedList<SchemaInfo>("select * from INFORMATION_SCHEMA.SCHEMATA");
                }
                return schemas;
            }
        }

        /// <summary>
        /// List of all constraints, tables filter is applied if present
        /// </summary>
        public List<ConstraintInfo> Constraints
        {
            get
            {
                return _constraints;
            }
            set
            {
                _constraints = value;
            }
        }
        /// <summary>
        /// List of all table columns, tables filter is applied if present
        /// </summary>
        public List<TableColumn> TableColumns
        {
            get
            {
                return _tableColumns;
            }
            set
            {
                _tableColumns = value;
            }
        }
        /// <summary>
        /// List of all view columns, views filter is applied if present
        /// </summary>
        public List<ViewColumn> ViewColumns
        {
            get
            {
                return _viewColumns;
            }
            set
            {
                _viewColumns = value;
            }
        }
        /// <summary>
        /// The connectionString is passed during the construction
        /// </summary>
        public string Connectionstring
        {
            get
            {
                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }

        /// <summary>
        /// List of all tables, tables filter is applied if present
        /// </summary>
        public List<TableInfo> Tables
        {
            get
            {
                return _tables;
            }
            set
            {
                _tables = value;
            }
        }

        /// <summary>
        /// List of all views, views filter is applied if present
        /// </summary>
        public List<ViewInfo> Views
        {
            get
            {
                return _views;
            }
            set
            {
                _views = value;
            }
        }

        /// <summary>
        /// To apply filter on table name on all levels
        /// </summary>
        public List<string> TableFilters
        {
            get
            {
                return tableFilters;
            }
            set
            {
                tableFilters = value;
            }
        }
        private List<string> tableFilters = new List<string>();

        /// <summary>
        /// To apply filter on view name on all levels
        /// </summary>
        public List<string> ViewFilters
        {
            get
            {
                return viewFilters;
            }
            set
            {
                viewFilters = value;
            }
        }
        private List<string> viewFilters = new List<string>();
        
        #endregion Properties
        
        #region Methods

        // Public Methods (4) 

        public void Dispose()
        {
            this.Tables = null;
        }

        /// <summary>
        /// Converts to .NET Framework type
        /// </summary>
        /// <param name="columnType"></param>
        /// <returns></returns>
        public static string ParseType(string columnType)
        {
            switch (columnType.Trim().ToLower())
            {
                case "bigint": return "Int64";
                case "int": return "Int32";
                case "smallint": return "Int16";
                case "tinyint": return "Byte";



                case "bit": return "Boolean";

                case "decimal": return "Decimal";
                case "numeric": return "Double";
                case "money": return "Decimal";
                case "smallmoney": return "decimal";
                case "float": return "float";
                case "real": return "Double";
                case "datetime": return "DateTime";
                case "smalldatetime": return "DateTime";
                case "char": return "String";
                case "varchar": return "String";
                case "text": return "String";
                case "nchar": return "String";
                case "nvarchar": return "String";
                case "ntext": return "String";
                case "binary": return "Byte[]";
                case "varbinary": return "Byte[]";
                case "image": return "Byte[]";
                case "uniqueidentifier": return "Guid";
                case "xml": return "System.Xml.Linq.XElement";

            }
            throw new Exception(columnType + " couldn't find appropriate dotnet type");
        }

        public static object ValidateDbType(string dbType, string userFriendlytype, object value)
        {
            string error = string.Format("'{0}' is not a valid '{1}' data type.", value.ToString(), userFriendlytype);

            switch (dbType.ToLower())
            {
                case "bigint":
                    try
                    {
                        Int64 temp = Convert.ToInt64(value);
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }

                case "System.Int64":
                    try
                    {
                        Int64 temp = Convert.ToInt64(value);
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }

                case "int":
                    try
                    {
                        Int32 temp = Convert.ToInt32(value);
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }

                case "System.Int32":
                    try
                    {
                        Int32 temp = Convert.ToInt32(value);
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }

                case "smallint": try
                    {
                        Int16 temp = Convert.ToInt16(value);
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }

                case "System.Int16":
                    try
                    {
                        Int16 temp = Convert.ToInt16(value);
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }

                case "tinyint":
                    try
                    {
                        System.Byte temp = Convert.ToByte(value);
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }

                case "System.Byte":
                    try
                    {
                        System.Byte temp = Convert.ToByte(value);
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }


                case "bit": try
                    {
                        System.Boolean temp = Convert.ToBoolean(value);
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }

                case "System.Boolean":
                    try
                    {
                        System.Boolean temp = Convert.ToBoolean(value);
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }


                case "decimal":
                    try
                    {
                        System.Decimal temp = Convert.ToDecimal(value);
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }

                case "System.Decimal":
                    try
                    {
                        System.Decimal temp = Convert.ToDecimal(value);
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }


                case "numeric":
                    try
                    {
                        System.Decimal temp = Convert.ToDecimal(value);
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }


                case "money":
                    try
                    {
                        System.Double temp = Convert.ToDouble(value);
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }

                case "System.Double":
                    try
                    {
                        System.Double temp = Convert.ToDouble(value);
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }



                case "smallmoney":
                    try
                    {
                        decimal temp = Convert.ToDecimal(value);
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }


                case "System.Single":
                    try
                    {
                        System.Single temp = Convert.ToSingle(value);
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }



                case "float":
                    try
                    {
                        Single temp = Convert.ToSingle(value);
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }


                case "real":
                    try
                    {
                        Double temp = Convert.ToDouble(value);
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }


                case "datetime":
                    try
                    {
                        DateTime temp = Convert.ToDateTime(value);
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }

                case "System.DateTime":
                    try
                    {
                        DateTime temp = Convert.ToDateTime(value);
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }

                case "smalldatetime":
                    try
                    {
                        DateTime temp = Convert.ToDateTime(value);
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }



                case "char":
                    try
                    {
                        string temp = Convert.ToString(value);
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }
                case "System.Char":
                    try
                    {
                        Char temp = (char)value;
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }

                case "varchar": try
                    {
                        string temp = Convert.ToString(value);
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }

                case "text": try
                    {
                        string temp = Convert.ToString(value);
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }

                case "nchar": try
                    {
                        string temp = Convert.ToString(value);
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }

                case "nvarchar": try
                    {
                        string temp = Convert.ToString(value);
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }

                case "ntext": try
                    {
                        string temp = Convert.ToString(value);
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }
                case "System.String": try
                    {
                        string temp = Convert.ToString(value);
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }
                case "binary": try
                    {
                        Byte[] temp = (Byte[])value;
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }

                case "varbinary": try
                    {
                        Byte[] temp = (Byte[])value;
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }

                case "image": try
                    {
                        Byte[] temp = (Byte[])value;
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }

                case "uniqueidentifier": try
                    {
                        Guid temp = new Guid(value.ToString());
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }
                case "System.Guid":
                    try
                    {
                        Guid temp = new Guid(value.ToString());
                        return string.Empty;
                    }
                    catch
                    {
                        return error;
                    }

            }

            return error;
        }

        public static bool ValidateDbType(object value, TypeInfo type,
        bool yesNoAsBoolAllowed,
        bool considerNullOrEmptyOrDbNullValidated,
        int characterlength,
        out object parsedValue)
        {

            parsedValue = value;

            #region null handling
            if (considerNullOrEmptyOrDbNullValidated)
            {
                if (null == value
                    || DBNull.Value == value
                    || "".MatchByString(string.Empty, value))
                {
                    return true;
                }
            }
            #endregion

            switch (type)
            {
                #region Decimal
                case TypeInfo.Decimal:
                case TypeInfo.money:
                case TypeInfo.smallmoney:
                    try
                    {
                        Decimal _Decimal = Convert.ToDecimal(value);
                        parsedValue = _Decimal;
                        return true;
                    }
                    catch { return false; }

                #endregion

                #region Int64
                case TypeInfo.bigint:
                    try
                    {
                        Int64 _int64 = Convert.ToInt64(value);
                        parsedValue = _int64;
                        return true;
                    }
                    catch { return false; }

                #endregion

                #region Int32
                case TypeInfo.Int:
                    try
                    {
                        Int32 _int32 = Convert.ToInt32(value);
                        parsedValue = _int32;
                        return true;
                    }
                    catch { return false; }

                #endregion

                #region Int16
                case TypeInfo.smallint:
                    try
                    {
                        Int16 _int16 = Convert.ToInt16(value);
                        parsedValue = _int16;
                        return true;
                    }
                    catch { return false; }

                #endregion

                #region byte
                case TypeInfo.tinyint:
                    try
                    {
                        byte _byte = Convert.ToByte(value);
                        parsedValue = _byte;
                        return true;
                    }
                    catch { return false; }

                #endregion

                #region Double
                case TypeInfo.numeric:
                case TypeInfo.real:
                    try
                    {
                        Double _double = Convert.ToDouble(value);
                        parsedValue = _double;
                        return true;
                    }
                    catch { return false; }

                #endregion

                #region Float
                case TypeInfo.Float:
                    try
                    {
                        float _fl = Convert.ToSingle(value);
                        parsedValue = _fl;
                        return true;
                    }
                    catch { return false; }

                #endregion

                #region Guid
                case TypeInfo.uniqueidentifier:
                    try
                    {
                        Guid _g = new Guid(value.Text());
                        parsedValue = _g;
                        return true;
                    }
                    catch { return false; }

                #endregion

                #region Byte[]
                case TypeInfo.binary:
                case TypeInfo.varbinary:
                case TypeInfo.image:
                    throw new Exception(String.Format("validate db type:{0} case not handled", type.ts()));
                #endregion

                #region bool
                case TypeInfo.bit:
                    try
                    {
                        bool _bool = Convert.ToBoolean(value);
                        parsedValue = _bool;
                        return true;
                    }
                    catch
                    {
                        if (yesNoAsBoolAllowed)
                        {
                            if ("".MatchByString("yes", value) || "".MatchByString("no", value) || "".MatchByString("n", value) || "".MatchByString("y", value))
                            {
                                parsedValue = "".MatchByString("yes", value) ? true : false;
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        return false;
                    }

                #endregion

                #region String
                case TypeInfo.Char:
                case TypeInfo.nchar:
                case TypeInfo.ntext:
                case TypeInfo.nvarchar:
                case TypeInfo.text:
                case TypeInfo.varchar:
                    bool _string = true;
                    if (characterlength > 0)
                    {
                        if ((value.Text().Length > characterlength))
                        {
                            _string = false;
                        }
                    }
                    return _string;

                #endregion

                #region date time
                case TypeInfo.smalldatetime:
                case TypeInfo.datetime:
                    try
                    {
                        DateTime _date = Convert.ToDateTime(value);
                        parsedValue = _date;
                        return true;
                    }
                    catch { return false; }

                #endregion
            }
            throw new Exception(String.Format("validate db type:{0} case not handled", type.ts()));
        }
        // Private Methods (2) 
        public void SetTableFilter(params string[] tableNames)
        {
            this.TableFilters = new List<string>();
            this.TableFilters.AddRange(tableNames);
        }
        public void SetViewFilter(params string[] viewNames)
        {
            this.ViewFilters = new List<string>();
            this.ViewFilters.AddRange(viewNames);
        }
        #endregion Methods
        
        #region Get Methods
        public static List<TableInfo> GetTables(string connectionString, params string[] filterTables)
        {
            List<TableInfo> tempList = new List<TableInfo>();
            string query = "Select table_name,table_schema from information_schema.Tables Where table_name <> 'sysdiagrams' AND Table_type = 'BASE TABLE'";
            if (filterTables.Length > 0)
            {
                query = query + " AND table_name in " + filterTables.ToWhereClauseInCommaList();
            }
            SqlConnection conn;
            using (IDataReader reader = SqlCommandX.Instance.GetReader(
                connectionString,
                query,
                 CommandType.Text, out conn))
            {
                while (reader.Read())
                {
                    string tableName = reader["table_name"].ToString().ToLower();
                    string schemaName = reader["table_schema"].Text();
                    tempList.Add(new TableInfo(connectionString, schemaName, reader["table_name"].ToString()));
                }
            }
            conn.Destroy();
            return tempList;
        }
        public static List<ViewInfo> GetViews(string connectionString, params string[] filterViews)
        {
            List<ViewInfo> views = new List<ViewInfo>();
            SqlCommand cmd = new SqlCommand();
            string query = "Select TABLE_NAME as Name from information_schema.views";
            if (filterViews.Length > 0)
            {
                query = query + " WHERE TABLE_NAME in " + filterViews.ToWhereClauseInCommaList();
            }
            views = cmd.GetTypedList<ViewInfo>(query,
                CommandType.Text, connectionString);
            foreach (ViewInfo v in views)
            {
                v.ConnectionString = connectionString;
            }
            return views;
        }
        public static List<TableColumn> GetTableColumns(string connectionString, params string[] filterTables)
        {
            List<TableColumn> _columns = new List<TableColumn>();
            string query = "Select *,IDENT_SEED(table_name) as id_seed from information_schema.columns";
       
            if (filterTables.Length > 0)
            {
                query = query + " WHERE table_name in " + filterTables.ToWhereClauseInCommaList();
            }
            SqlConnection conn;
            using (IDataReader reader = SqlCommandX.Instance.GetReader(
                 connectionString, query, CommandType.Text, out conn))
            {
                while (reader.Read())
                {
                    TableColumn cs = new TableColumn(
                        reader["column_name"].ToString(),
                        reader["table_name"].ToString(),
                        (reader["Is_Nullable"].ToString() == "YES") ? true : false,
                        reader["data_type"].ToString(),
                                                (reader["id_seed"].Text() == "1"));

                    if (DBNull.Value != reader["character_maximum_length"])
                    {
                        cs.MaxLength = Convert.ToInt32(reader["character_maximum_length"]);
                    }

                    if (DBNull.Value != reader["character_octet_length"])
                    {
                        cs.OctetLength = Convert.ToInt32(reader["character_octet_length"]);
                    }

                    if (DBNull.Value != reader["numeric_precision"])
                    {
                        cs.NumericPrecision = Convert.ToInt32(reader["numeric_precision"]);
                    }

                    if (DBNull.Value != reader["numeric_precision_radix"])
                    {
                        cs.NumericPrecisionRadix = Convert.ToInt32(reader["numeric_precision_radix"]);
                    }

                    if (DBNull.Value != reader["numeric_scale"])
                    {
                        cs.NumericScale = Convert.ToInt32(reader["numeric_scale"]);
                    }

                    if (DBNull.Value != reader["datetime_precision"])
                    {
                        cs.DateTimePrecision = Convert.ToInt32(reader["datetime_precision"]);
                    }
                    _columns.Add(cs);
                }
            }

            //load description property from tables
            

            conn.Destroy();
            return _columns;
        }
        /// <summary>
        /// Get list of all constraints, you can apply table filter
        /// </summary>
        public static List<ConstraintInfo> GetConstraints(string connectionString, params string[] filterTables)
        {
            List<ConstraintInfo> list = new List<ConstraintInfo>();
            SqlConnection conn;
            string query = filterTables.Length > 0 ? ConstraintInfo.queryWithFilter(filterTables) : ConstraintInfo.query;
            using (IDataReader reader = SqlCommandX.Instance.GetReader(
                connectionString, query, CommandType.Text, out conn))
            {
                while (reader.Read())
                {
                    ConstraintInfo cs = new ConstraintInfo();
                    if (DBNull.Value != reader["table_name"])
                    {
                        cs.table_name = reader["table_name"].ToString();
                    }
                    if (DBNull.Value != reader["field_name"])
                    {
                        cs.field_name = reader["field_name"].ToString();
                    }
                    if (DBNull.Value != reader["constraint_type"])
                    {
                        cs.constraint_type = reader["constraint_type"].ToString();
                    }
                    if (DBNull.Value != reader["constraint_name"])
                    {
                        cs.constraint_name = reader["constraint_name"].ToString();
                    }

                    if (DBNull.Value != reader["match_type"])
                    {
                        cs.match_type = reader["match_type"].ToString();
                    }

                    if (DBNull.Value != reader["on_update"])
                    {
                        cs.on_update = reader["on_update"].ToString();
                    }

                    if (DBNull.Value != reader["on_delete"])
                    {
                        cs.on_delete = reader["on_delete"].ToString();
                    }

                    if (DBNull.Value != reader["references_table"])
                    {
                        cs.references_table = reader["references_table"].ToString();
                    }

                    if (DBNull.Value != reader["references_field"])
                    {
                        cs.references_field = reader["references_field"].ToString();
                    }

                    if (DBNull.Value != reader["field_position"])
                    {
                        cs.field_position = int.Parse(reader["field_position"].ToString());
                    }
                    list.Add(cs);
                }
            }
            conn.Destroy();
            return list;
        }
        public static List<ViewColumn> GetViewColumns(string connectionString, params string[] filterViews)
        {
            SqlCommand cmd = new SqlCommand();
            string query = "Select COLUMN_NAME AS Name, table_name as ViewName, ORDINAL_POSITION AS Position, COLUMN_DEFAULT As DefaultValue,IS_NULLABLE AS IsNullable, DATA_TYPE as DataType,  CHARACTER_MAXIMUM_LENGTH as MaxLength,NUMERIC_PRECISION as NumericPrecision, CHARACTER_OCTET_LENGTH as OctetLength from information_schema.Columns order by ORDINAL_POSITION";
            if (filterViews.Length > 0)
            {
                query = "Select COLUMN_NAME AS Name, table_name as ViewName, ORDINAL_POSITION AS Position, COLUMN_DEFAULT As DefaultValue,IS_NULLABLE AS IsNullable, DATA_TYPE as DataType,  CHARACTER_MAXIMUM_LENGTH as MaxLength,NUMERIC_PRECISION as NumericPrecision, CHARACTER_OCTET_LENGTH as OctetLength from information_schema.Columns WHERE table_name in [[filter]] order by ORDINAL_POSITION"
                    .Replace("[[filter]]", filterViews.ToWhereClauseInCommaList());
            }
            return cmd.GetTypedList<ViewColumn>(query,CommandType.Text, connectionString);
        }
        #endregion
    }
}
