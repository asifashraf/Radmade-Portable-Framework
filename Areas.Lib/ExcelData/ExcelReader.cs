using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAreas.Lib.ExcelData
{
    public class ExcelReader
    {
        public ExcelReader()
        {

        }

        public DataSet GetDataSet(ExcelVersion version, string filePath, bool isFirstRowAsColumnNames = true)
        {
            var reader = GetExcelDataReader(version, filePath, isFirstRowAsColumnNames);
            var result = reader.AsDataSet();
            reader.Close();
            return result;
        }

        public IExcelDataReader GetExcelDataReader(ExcelVersion version, string filePath, bool isFirstRowAsColumnNames = true)
        {
            var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            var excelReader = version == ExcelVersion.Version_97_2003 ?
                ExcelReaderFactory.CreateBinaryReader(stream) :
                ExcelReaderFactory.CreateOpenXmlReader(stream);
            excelReader.IsFirstRowAsColumnNames = isFirstRowAsColumnNames;
            return excelReader;
        }
    }

    public enum ExcelVersion
    {
        Version_97_2003,
        Version_2007
    }

    public delegate void ExcelDataSetRead(DataSet dataset);

    public delegate void ExcelPreColumnOptionsWork(DataTable dataTable, ExcelTableOptions options);

    public delegate void ExcelPreTableImport(DataTable dataTable, ExcelTableOptions options, string targetTableNameInDatabase);

    public delegate void ExcelPostTableImport(DataTable dataTable, ExcelTableOptions options, string targetTableNameInDatabase);

    public delegate void ExcelFileImported();

    public class ExcelTableOptions
    {
        //Description
        public string Desc { get; set; }

        public string Id { get; set; }
    }

    public class ExcelColumnOptions
    {
        //Description
        public string Desc { get; set; }

        //Sample: "select [Description],DivisionId from Divisions where description IN (@Values)"
        public string Query { get; set; }

        //Sample: "Insert into Divisions(CompanyId, [Description]) Values(1, @Value)"
        public string Insert { get; set; }

        //Sample: System.DateTime 
        public string Type { get; set; }

        public string JoinWith { get; set; }

        public string JoinFormat { get; set; }

        public string Func { get; set; }
    }
}
