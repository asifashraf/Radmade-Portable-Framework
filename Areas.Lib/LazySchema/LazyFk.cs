using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAreas.Lib.LazySchema
{
    public class LazyFk
    {
        public string Name { get; set; }

        public long ObjectId { get; set; }

        public bool IsDisabled { get; set; }

        public bool IsNotFotReplication { get; set; }

        public byte DeleteReferentialAction { get; set; }

        public byte UpdateReferentialAction { get; set; }

        public string FkTableName { get; set; }

        public string FkTableSchema { get; set; }

        public string FkTableFullName
        {
            get
            {
                return GetFullName(FkTableName, FkTableSchema);
            }
        }

        string GetFullName(string tableName, string schema)
        {
            var schemaToUse = schema.IsNullOrEmpty() ? "dbo" : schema;
            return string.Format("{0}.{1}", schema, tableName);
        }

        public string PkTableName { get; set; }

        public string PkTableSchema { get; set; }

        public string PkTableFullName
        {
            get
            {
                return GetFullName(PkTableName, PkTableSchema);
            }
        }

        public string FkColumnName { get; set; }

        public string PkColumnName { get; set; }

        public int ConstraintColumnId { get; set; }

        public bool IsNotTrusted { get; set; }
    }
}
