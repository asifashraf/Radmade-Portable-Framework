

namespace Areas.Lib.DataBootstrap
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Web.Script.Serialization;
    public class Bootstrapper : IDisposable
    {
        public DataHelper source { get; set; }

        public DataHelper target { get; set; }

        public Bootstrapper(string sourceConnectionString, string targetConnectionString)
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
            var tableNames = topToBottomCommaSepTables.Split(new char[]{ ',' }).ToList<string>();
            
            var countTablesNames = tableNames.Count;
            
            //delete all data
            var deleteStringBuilder = new StringBuilder();

            for (var i = countTablesNames - 1; i >= 0; i--)
            {
                var currentTableName = tableNames[i];                

                deleteStringBuilder.Append("Delete from [[table]]; ".Replace("[[table]]", currentTableName));
            }

            target.ExecuteQuery(deleteStringBuilder.ToString());

            //Select all data from sample db
            var sampleData = source.GetDataTable("select * from SampleData");

            //Insert
            var targetDbSchema = new InformationSchema.InfoSchema(target.ConnectionString, true);

            var jserializer = new JavaScriptSerializer();

            for (var i = 0; i < countTablesNames; i++)
            {
                var currentTableName = tableNames[i];

                var inlineValues = currentTableName.StartsWith("[");

                var currentTableSchema = targetDbSchema.Tables.Where(t => t.Name.MatchByString(currentTableName)).One();

                if (currentTableSchema.IsNotNull())
                {
                    //take list of columns where description is contains token
                    var columns = currentTableSchema.Columns
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

                        var bsData = jserializer.Deserialize<BootstrapData>(bsString);

                        bsData.TableName = currentTableSchema.Name;

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

                    //reset Identity
                    target.ExecuteQuery("DBCC CHECKIDENT('@TableName', RESEED, 0); ".Replace("@TableName", currentTableSchema.Name));
                    
                    
                    /*The concept of value spreading
                     * Table type SourceBound===
                    {{ Source: 'CompanyStreet' }}
                    {{ Source: '*asif, atif, wajahat' }}
                    company street will appear 500 times with binding
                    other column will just spread in the same pattern among 500 rows

                    * Table type Inline Values===
                    {{ FKColumn: 'CompanyId' , FKTable: 'Companies', FkPick: 2 }}
                    {{ Source: '*admin,dev,user,driver,customer,manager,employee,peon,teacher,student' }}
                    values will be multiplied if FK is picked and inline values are found.
                    So for FK 1 and then for FK2 the number of given values will be double in such situation.
                    first of FK 1 and then for FK 2

                    * Table type Inline Values===
                    values will be fixed number if no FK is found
                    {{ Source: '*admin,dev,user,driver,customer,manager,employee,peon,teacher,student' }}
                    only one time and no repeat

                     * Null parameter for all values
                    {{}} send null value in parameter
                        */


                    //iterate over all sample db rows
                    var rowCount = sampleData.Rows.Count;
                    for (var r = 0; r < rowCount; r++ )
                    {
                        var sbInsert = new StringBuilder();

                        var row = sampleData.Rows[r];

                        //create insert statement using table and column names
                        sbInsert.Append("INSERT into " + currentTableSchema.Name +
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
