using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Areas.Lib.InformationSchema
{
    public class TableInfo : ITableInfo
    {
        private string _name;
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

//        private string _description = "";
//        public string Description
//        {
//            get
//            {
//                if (_description.IsNullOrEmpty())
//                {
//                    _description = GetScalarByQuery(@"select [value] from fn_listextendedproperty 
//(null,'schema','dbo','TABLE',null,null,null) where objname <> 'sysdiagrams' 
//AND objname = '[[TableName]]'".Replace("[[TableName]]", this.Name)).Text();
//                }

//                return _description;
//            }
//            set
//            {
//                _description = value;
//            }
//        }

        private string _connectionString;
        internal string ConnectionString
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

        public TableInfo(string connectionString, string tableName)
        {
            _connectionString = connectionString;
            this.Name = tableName;
        }

        private string columnParamsSuffix = string.Empty;
        public string ColumnParamsSuffix
        {
            get
            {
                return columnParamsSuffix;
            }
            set
            {
                columnParamsSuffix = value;
            }
        }

        private List<TableColumn> _columns;
        public List<TableColumn> Columns
        {
            get
            {

                if (_columns == null)
                {
                    TableColumn cs = new TableColumn(this.ConnectionString);
                    _columns = cs.GetbyTable(this.Name);
                }

                return _columns;
            }
            set
            {
                _columns = value;
            }
        }

        private List<ConstraintInfo> _primaryKeys;
        public List<ConstraintInfo> PKConstraints
        {
            get
            {
                if (_primaryKeys == null)
                {
                    ConstraintInfo cs = new ConstraintInfo();
                    _primaryKeys = cs.GetPrimaryKeyOnTable(this.ConnectionString, this.Name);

                }

                return _primaryKeys;
            }
            set
            {
                _primaryKeys = value;
            }
        }

        public List<TableColumn> PKColumns
        {
            get
            {
                if (null == this.PKConstraints)
                {
                    ConstraintInfo cs = new ConstraintInfo();
                    this.PKConstraints = cs.GetPrimaryKeyOnTable(this.ConnectionString, this.Name);
                }
                return ConstraintInfo.MatchColumns(this.PKConstraints, this);
            }
        }

        public bool HasIdentityPK
        {
            get
            {
                IEnumerable<string> query = from c in this.Columns
                                            where c.IsIdSeed
                                            select c.Name;
                return query.Count() > 0;
            }
        }
    }
}
