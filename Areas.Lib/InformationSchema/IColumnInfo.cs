using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Areas.Lib.InformationSchema
{
    public interface IColumnInfo
    {
        string Name { get; set; }
        string DataType { get; set; }
        bool AllowsNull { get; }
        int MaxLength { get; set; }
        int OctetLength { get; set; }
        int NumericPrecision { get; set; }
    }
}
