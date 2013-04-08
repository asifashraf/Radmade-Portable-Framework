using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAreas.Lib
{
    public class DataImport
    {
        public string ConnectionString { get; set; }

        public DataImport(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public void FromDataTable(DataTable table, bool resetIdentity, string tableName = "")
        {
            if (tableName.IsNullOrEmpty())
            {
                tableName = table.TableName;
            }
            var rowCount = table.Rows.Count;
            var sbColumnNames = new StringBuilder();
            var sbColumnParamNames = new StringBuilder();
            var countColumns = table.Columns.Count;
            var importerDB = new DataHelper(this.ConnectionString);

            if (resetIdentity)
            {
                importerDB.ResetIdentityInTable(new List<string>() { tableName });
            }

            //iterate in columns
            for (var c = 0; c < countColumns; c++)
            {
                var column = table.Columns[c];

                if (c > 0)
                {
                    sbColumnNames.Append(", ");
                    sbColumnParamNames.Append(",");
                }

                sbColumnNames.Append(column.ColumnName);

                sbColumnParamNames.Append(string.Format("@{0}", column.ColumnName));
            }

            var boundColumns = new Dictionary<string, string>();

            for (var r = 0; r < rowCount; r++)
            {
                var sbInsert = new StringBuilder();

                var row = table.Rows[r];

                //create insert statement using table and column names
                sbInsert.Append("INSERT into " + tableName +
                    "(" + sbColumnNames.ToString()
                    + ") values(" + sbColumnParamNames.ToString() + "); ");

                var parameters = new List<object>();
                //iterate over all items in the list of bootstrap data on columns
                for (var cc = 0; cc < countColumns; cc++)
                {
                    //take parameters from data table of sample db
                    var currentColumn = table.Columns[cc];
                    parameters.Add(string.Format("@{0}", currentColumn.ColumnName));

                    //read from origianl value in the sequence
                    parameters.Add(row[currentColumn.ColumnName]);
                }

                //insert data
                importerDB.ExecuteQuery(sbInsert.ToString(), parameters.ToArray());
            }
        }

    }
}
