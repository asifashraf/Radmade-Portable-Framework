using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Areas.Lib.InformationSchema
{
    public class ForeignKey
    {
        public string Name { get; set; }

        public string ObjectId { get; set; }

        public bool IsDisabled { get; set; }

        public bool IsNotFotReplication { get; set; }

        public bool DeleteReferentialAction { get; set; }

        public bool UpdateReferentialAction { get; set; }

        public string FkTableName { get; set; }

        public string FkTableSchema { get; set; }

        public string FkTableFullName
        {
            get
            {
                return TableInfo.GetFullName(FkTableName, FkTableSchema);
            }
        }

        public string PkTableName { get; set; }

        public string PkTableSchema { get; set; }

        public string PkTableFullName
        {
            get
            {
                return TableInfo.GetFullName(PkTableName, PkTableSchema);
            }
        }

        public string FkColumnName { get; set; }

        public string PkColumnName { get; set; }

        public int ConstraintColumnId { get; set; }

        public bool IsNotTrusted { get; set; }
    }
}
