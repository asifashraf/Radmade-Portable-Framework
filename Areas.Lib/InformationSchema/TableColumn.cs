using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace WebAreas.Lib.InformationSchema
{
    public class TableColumn: IColumnInfo
    {
        #region Fields (14)

        private bool _allowsNull;
        private int _charMaxLength;
        private int _charOctLength;
        private string _dataType;
        private int _dateTimePrecision;
        bool _isIdSeed = false;
        private bool _isPk;
        private string _name;
        private int _numericPrecision;
        private int _numericPrecisionRadix;
        private int _numericScale;
        private string _description;
        private TableInfo _table;
        private string _tableName;
        string ConnectionString;

        #endregion Fields

        #region Constructors (2)

        public TableColumn(string name, string tableName,
            bool allowsNull, string dataType, bool isIdSeeded)
        {
            this.Name = name;
            this.AllowsNull = allowsNull;
            this.DataType = dataType;
            this.TableName = tableName;
            this.IsIdSeed = isIdSeeded;
        }

        public TableColumn(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        #endregion Constructors

        #region Properties (13)

        public bool AllowsNull
        {
            get
            {
                return _allowsNull;
            }
            set
            {
                _allowsNull = value;
            }
        }

        public int MaxLength
        {
            get
            {
                return _charMaxLength;
            }
            set
            {
                _charMaxLength = value;
            }
        }

        public int OctetLength
        {
            get
            {
                return _charOctLength;
            }
            set
            {
                _charOctLength = value;
            }
        }

        public string DataType
        {
            get
            {
                return _dataType;
            }
            set
            {
                _dataType = value;
            }
        }

        public int DateTimePrecision
        {
            get
            {
                return _dateTimePrecision;
            }
            set
            {
                _dateTimePrecision = value;
            }
        }

        public bool IsIdSeed
        {
            get
            {
                return _isIdSeed;
            }
            set
            {
                _isIdSeed = value;
            }
        }

        public bool IsPrimaryKey
        {
            get
            {
                return _isPk;
            }
            set
            {
                _isPk = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public int NumericPrecision
        {
            get
            {
                return _numericPrecision;
            }
            set
            {
                _numericPrecision = value;
            }
        }

        public int NumericPrecisionRadix
        {
            get
            {
                return _numericPrecisionRadix;
            }
            set
            {
                _numericPrecisionRadix = value;
            }
        }

        public int NumericScale
        {
            get
            {
                return _numericScale;
            }
            set
            {
                _numericScale = value;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        public TableInfo Table
        {
            get
            {
                return _table;
            }
            set
            {
                _table = value;
            }
        }

        public string TableName
        {
            get
            {
                return _tableName;
            }
            set
            {
                _tableName = value;
            }
        }

        #endregion Properties

        #region Methods (2)

        // Public Methods (2) 

        

        public List<TableColumn> GetbyTable(string tableName)
        {
            List<TableColumn> _columns = new List<TableColumn>();
            string query = @"Select *,IDENT_SEED(table_name) as id_seed from information_schema.columns col
        LEFT OUTER JOIN ::fn_listextendedproperty(NULL, 'schema','dbo','table', '[[TableName]]' ,'column', null) des ON col.column_name = des.objname COLLATE latin1_general_ci_ai
        where table_name = '[[TableName]]'".Replace("[[TableName]]", tableName);
            SqlConnection conn;
            using (IDataReader reader = SqlCommandX.Instance.GetReader(
                 this.ConnectionString, query, CommandType.Text, out conn))
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

                    if (DBNull.Value != reader["value"])
                    {
                        cs.Description = reader["value"].Text();
                    }
                    _columns.Add(cs);
                }
            }
            conn.Destroy();
            return _columns;
        }

        #endregion Methods

        public string EscapeTypeNameMatch()
        {
            string myName = this.Name.WithFirstCharUpper();
            string TypeNameEColumn = this.TableName.Singularize().WithFirstCharUpper();
            if (myName.MatchCaseSensitive(TypeNameEColumn))
            {
                myName = myName + "1";
            }
            return myName;
        }
    }
}
