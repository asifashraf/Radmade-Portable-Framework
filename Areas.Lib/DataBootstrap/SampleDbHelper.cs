using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Areas.Lib.DataBootstrap
{
    public class SampleDbHelper : IDisposable
    {
        public DataHelper source { get; set; }

        public DataHelper target { get; set; }

        public SampleDbHelper(string sourceConnectionString, string targetConnectionString)
        {
            this.source = new DataHelper(sourceConnectionString);

            this.target = new DataHelper(targetConnectionString);
        }

        public void SetColumn(string columnName, object value)
        {
            source.ExecuteQuery("update SampleData SET " + columnName + " = @value", "@value", value);
        }

        public object GetTopColumn(string columnName)
        {
            return source.GetScalarByQuery("select top 1 " + columnName + " from SampleData");
        }

        public BootstrapState BootstrapDB(string topToBottomCommaSepTables)
        {
            var order = topToBottomCommaSepTables.Split(new char[]{ ',' }).ToList<string>();
            
            var countTablesToBootstrap = order.Count;
            
            //delete all data
            var deleteStatements = new StringBuilder();
            for (var i = countTablesToBootstrap - 1; i >= 0; i--)
            {
                var current = order[i];                

                deleteStatements.Append("Delete from [[table]]; ".Replace("[[table]]", current));
            }

            target.ExecuteQuery(deleteStatements.ToString());

            var sampleData = source.GetDataTable("select * from SampleData");

            //Insert
            var dbSchema = new InformationSchema.InfoSchema(target.ConnectionString, true);

            var jss = new JavaScriptSerializer();

            for (var i = 0; i < countTablesToBootstrap; i++)
            {
                var current = order[i];

                var table = dbSchema.Tables.Where(t => t.Name.MatchByString(current)).One();

                if (table.IsNotNull())
                {
                    //take list of columns where description is contains token
                    var columns = table.Columns
                        .Where(c => !String.IsNullOrEmpty(c.Description))
                        .Where(c => c.Description.Contains("{{"))
                        .Where(c => c.Description.Contains("}}")).ToListSafely();

                    var countColumns = columns.Count();

                    if (countColumns < 1)
                    {
                        continue;
                    }

                    var bsColumns = new List<BootstrapData>();

                    var sbColumnNames = new StringBuilder();
                    
                    var sbColumnParamNames = new StringBuilder();                    

                    //iterate in columns
                    for (var c = 0; c < countColumns; c++)
                    {
                        var column = columns[c];

                        //take bootstrap data
                        var bsString = column.Description.Substring(column.Description.IndexOf("{{") + 1);

                        bsString = bsString.Substring(0, bsString.IndexOf("}}") + 1);

                        var bsData = jss.Deserialize<BootstrapData>(bsString);

                        bsData.TableName = table.Name;

                        bsData.ColumnName = column.Name;

                        bsColumns.Add(bsData);

                        if (c > 0)
                        {
                            sbColumnNames.Append(", ");
                            sbColumnParamNames.Append(",");
                        }

                        sbColumnNames.Append(column.Name);

                        sbColumnParamNames.Append(string.Format("@{0}", column.Name));
                    }                   

                    var countBsColumns = bsColumns.Count;

                    //iterate over all sample db rows
                    var rowCount = sampleData.Rows.Count;
                    for (var r = 0; r < rowCount; r++ )
                    {
                        var sbInsert = new StringBuilder();

                        var row = sampleData.Rows[r];

                        //create insert statement using table and column names
                        sbInsert.Append("INSERT into " + table.Name +
                            "(" + sbColumnNames.ToString()
                            + ") values(" + sbColumnParamNames.ToString() + "); ");

                        var parameters = new List<object>();
                        //iterate over all items in the list of bootstrap data on columns
                        for (var cc = 0; cc < countBsColumns; cc++)
                        {
                            //take parameters from data table of sample db
                            var currentBsColumn = bsColumns[cc];
                            parameters.Add(string.Format("@{0}", currentBsColumn.ColumnName));
                            parameters.Add(row[currentBsColumn.Source]);
                        }

                        //insert data
                        target.ExecuteQuery(sbInsert.ToString(), parameters.ToArray());
                    }
                }
            }

            return new BootstrapState(string.Empty);
        }

        public void Dispose()
        {
            source.Dispose();

            target.Dispose();
        }
    }
}
