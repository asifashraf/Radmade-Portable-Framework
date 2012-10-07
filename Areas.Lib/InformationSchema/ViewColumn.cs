using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Areas.Lib.InformationSchema
{
    public class ViewColumn : IColumnInfo
    {
        #region Constructors

        public ViewColumn() { }

        #endregion Constructors

        #region Properties


        public string DataType { get; set; }

        public object DefaultValue { get; set; }

        public string IsNullable { get; set; }

        public bool AllowsNull
        {

            get
            {
                return this.IsNullable.MatchByString("YES");
            }
        }

        public int MaxLength { get; set; }

        public string Name { get; set; }

        public int NumericPrecision { get; set; }

        public int OctetLength { get; set; }

        public int Position { get; set; }
        public string ViewName { get; set; }
        #endregion Properties

    }
}
