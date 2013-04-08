namespace WebAreas.Lib
{
    using Areas.DotNetExtensions;
    using Excel;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web.Script.Serialization;
    using WebAreas.Lib.ExcelData;

    public class DataHelper : IDisposable
    {
        public DataHelper(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        
        public string ConnectionString { get; set; }

        protected SqlConnection Connection { get; set; }

        protected SqlCommand Command { get; set; }

        //Reset connection and command
        protected void OpenConnection()
        {
            Connection = new SqlConnection(ConnectionString);
            Command = Connection.CreateCommand();
            Command.CommandType = System.Data.CommandType.Text;
            Command.Parameters.Clear();
            Command.CommandText = string.Empty;            
            this.Connection.OpenSafely();
        }

        //close connection and release resources
        protected void CloseConnection()
        {
            try
            {
                this.Connection.CloseSafely();
            }
            catch { }

            try
            {
                this.Command.Dispose();
            }
            catch { }

            try
            {
                this.Connection.Dispose();
            }
            catch { }
        }

        //Run query as string
        public void ExecuteQuery(string query, params object[] parametersArray)
        {
            OpenConnection();

            Command.CommandText = query;

            Command.SetCommandParametersByPairsArray(parametersArray);

            Command.ExecuteNonQuery();

            CloseConnection();
        }
        
        //execute stored procedure
        public void ExecuteStoredProcedure(string storedProcedure, params object[] parametersArray)
        {
            OpenConnection();

            Command.CommandType = System.Data.CommandType.StoredProcedure;

            Command.CommandText = storedProcedure;

            Command.SetCommandParametersByPairsArray(parametersArray);

            Command.ExecuteNonQuery();

            CloseConnection();
        }

        //get scalar by query 
        public object GetScalarByQuery(string query, params object[] parametersArray)
        {
            OpenConnection();

            Command.CommandText = query;

            var result = Command.ExecuteScalar();

            CloseConnection();

            return result;
        }
        
        //get scalar by stored procedure
        public object GetScalarByStoredProcedure(string storedProcedure, params object[] parametersArray)
        {
            OpenConnection();

            Command.CommandType = System.Data.CommandType.StoredProcedure;

            Command.CommandText = storedProcedure;

            var result = Command.ExecuteScalar();

            CloseConnection();

            return result;
        }

        //get data table by query
        public DataTable GetDataTable(string query)
        {
            var adapter = new SqlDataAdapter(query, ConnectionString);

            var table = new DataTable();

            adapter.Fill(table);

            return table;
        }

        //get list of items by query 
        public List<T> GetTypedList<T>(string query, params object[] parameters)
        {
            this.OpenConnection();
            this.Command.CommandText = query;
            this.Command.SetCommandParametersByPairsArray(parameters);
            var reader = this.Command.ExecuteReader();            
            var result = reader.ParseToObjectList<T>(this.Connection, true);
            this.CloseConnection();
            return result;
        }

        //Reset identity in tables
        public void ResetIdentityInTable(List<string> tables)
        {
            OpenConnection();

            var countTablesNames = tables.Count;
            var deleteStringBuilder = new StringBuilder();
            var fakeInsertBuilder = new StringBuilder();

            for (var i = 0; i < countTablesNames; i++)
            {
                var currentTableName = tables[i];

                fakeInsertBuilder.Append(@"
BEGIN TRY 
INSERT INTO [[TableFullName]] DEFAULT VALUES 
END TRY  
BEGIN CATCH  
Print '1' 
END CATCH 
".Replace("[[TableFullName]]", currentTableName).Replace("\r\n", " "));

                deleteStringBuilder.Append("Delete from [[table]]; ".Replace("[[table]]", currentTableName));

                try
                {
                    ExecuteQuery(fakeInsertBuilder.ToString());
                }
                catch
                {

                }

                try
                {
                    ExecuteQuery(deleteStringBuilder.ToString());
                }
                catch
                {

                }

                try
                {
                    //reset Identity
                    ExecuteQuery("DBCC CHECKIDENT('@TableName', RESEED, 0); ".Replace("@TableName", currentTableName));
                }
                catch
                {

                }
            }
            CloseConnection();
        }

        //Empty full database
        public void TruncateFullDatabase()
        {
            var query = @"
EXEC sp_msforeachtable ""ALTER TABLE ? NOCHECK CONSTRAINT all""
EXEC sp_MSForEachTable ""DELETE FROM ?""
exec sp_msforeachtable ""ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all""
";
            ExecuteQuery(query);
        }

        //Import full database from excel file
        //If sheet ends with .x that sheet will not be imported
        public List<string> BootstrapDatabaseFromExcelSheet(string filePath, ExcelVersion version)
        {            
            var jsDeserializerForOptions = new JavaScriptSerializer();

            TruncateFullDatabase();

            List<string> listOfProcessedTables = new List<string>();
            
            var datasetFromExcel = new ExcelReader().GetDataSet(version, filePath, true);

            if (this.OnExcelDataSetRead != null)
            {
                this.OnExcelDataSetRead(datasetFromExcel);
            }

            var dataTableImporter = new DataImport(this.ConnectionString);
            
            foreach (DataTable currentTable in datasetFromExcel.Tables)
            {
                var tableOptionsObject = new ExcelTableOptions();

                #region Exclude table
                if (currentTable.TableName.ToLower().EndsWith(".x"))
                {
                    continue;
                }
                #endregion

                var currentTableName = currentTable.TableName;

                #region Read table options

                if (currentTableName.Contains("{")
                        && currentTableName.Contains("}"))
                {
                    var tableOptionsText = currentTableName.Substring(currentTableName.IndexOf("{"));
                    //In table options JSON text, We use ^ instead of : and here we replace it
                    tableOptionsText = tableOptionsText.Replace("^", ":");
                    tableOptionsObject = jsDeserializerForOptions.Deserialize<ExcelTableOptions>(tableOptionsText);
                    currentTableName = currentTableName.Substring(0, currentTableName.IndexOf("{"));
                    tableOptionsObject.Id = currentTableName;
                }
                else
                {
                    tableOptionsObject.Id = currentTable.TableName;
                }

                #endregion

                listOfProcessedTables.Add(currentTable.TableName);

                //event Pre Column Options Work
                if (this.OnExcelPreColumnOptionsWork != null)
                {
                    this.OnExcelPreColumnOptionsWork(currentTable, tableOptionsObject);
                }

                foreach (DataColumn currentColumn in currentTable.Columns)
                {
                    #region Column driven by options
                    if (currentColumn.ColumnName.Contains("{")
                        && currentColumn.ColumnName.Contains("}"))
                    {
                        var columnOptionsText = currentColumn.ColumnName.Substring(currentColumn.ColumnName.IndexOf("{"));
                        var columnOptionsObject = jsDeserializerForOptions.Deserialize<ExcelColumnOptions>(columnOptionsText);
                        var columnNameB4Options = currentColumn.ColumnName.Substring(0, currentColumn.ColumnName.IndexOf("{"));

                        #region Column data Type conversion In case of Type option present in column options

                        if (columnOptionsObject.Type.IsNotNullOrEmpty())
                        {
                            var targetDotNetType = Type.GetType(columnOptionsObject.Type);
                            foreach (DataRow row4DataTypeChange in currentTable.Rows)
                            {
                                var typeConverter4ValueInColumn = new ManualConvert(row4DataTypeChange[currentColumn.ColumnName]);
                                try
                                {
                                    //set value
                                    row4DataTypeChange[currentColumn.ColumnName] = typeConverter4ValueInColumn.ConvertType(columnOptionsObject.Type);
                                }
                                catch (Exception err)
                                {
                                    //Set Db null
                                    row4DataTypeChange[currentColumn.ColumnName] = DBNull.Value;
                                }

                            }
                        }
                        #endregion

                        #region Query for foreign keys or alternative values
                        if (columnOptionsObject.Query.IsNotNullOrEmpty())
                        {
                            var nonDistinctColumnValues = new List<string>();

                            foreach (DataRow row4NonDistinctValues in currentTable.Rows)
                            {
                                nonDistinctColumnValues.Add(row4NonDistinctValues[currentColumn.ColumnName].Text());
                            }

                            var distinctColumnValues = nonDistinctColumnValues.Distinct<string>().ToList<string>();

                            var sbDistinctColumValues = new StringBuilder();

                            var indexForeachOnDistinctValues = 0;

                            foreach (var distinctColumnValue in distinctColumnValues)
                            {
                                if (indexForeachOnDistinctValues > 0)
                                {
                                    sbDistinctColumValues.Append(",");
                                }

                                sbDistinctColumValues.Append("'" + distinctColumnValue + "'");

                                indexForeachOnDistinctValues++;
                            }

                            var pairsTableFromDB = GetDataTable(columnOptionsObject.Query.Replace("@Values", sbDistinctColumValues.ToString()));
                            
                            //Insert non existing in db
                            if (columnOptionsObject.Insert.IsNotNullOrEmpty())
                            {
                                var listNonExistingItems = new List<string>();
                                foreach (DataRow rowInExcel in currentTable.Rows)
                                {
                                    var recordExists = false;
                                    foreach (DataRow rowInDBToReplace in pairsTableFromDB.Rows)
                                    {
                                        if (rowInExcel[currentColumn.ColumnName].Text() == rowInDBToReplace[0].Text())//first column matched
                                        {
                                            recordExists = true;
                                            break;
                                        }
                                    }
                                    if (!recordExists)
                                    {
                                        listNonExistingItems.Add(rowInExcel[currentColumn.ColumnName].Text());
                                    }
                                }

                                if (listNonExistingItems.Any())
                                {
                                    listNonExistingItems = listNonExistingItems.Distinct<string>().ToList<string>();

                                    foreach (var nonExistingValue in listNonExistingItems)
                                    {
                                        ExecuteQuery(columnOptionsObject.Insert, "@Value", nonExistingValue);
                                    }

                                    #region Fetch again

                                    indexForeachOnDistinctValues = 0;

                                    sbDistinctColumValues = new StringBuilder();

                                    foreach (var distinctColumnValue in distinctColumnValues)
                                    {
                                        if (indexForeachOnDistinctValues > 0)
                                        {
                                            sbDistinctColumValues.Append(",");
                                        }

                                        sbDistinctColumValues.Append("'" + distinctColumnValue + "'");

                                        indexForeachOnDistinctValues++;
                                    }

                                    pairsTableFromDB = GetDataTable(columnOptionsObject.Query.Replace("@Values", sbDistinctColumValues.ToString()));
                                    #endregion

                                }
                            }

                            foreach (DataRow rowInExcel in currentTable.Rows)
                            {
                                foreach (DataRow rowInDBToReplace in pairsTableFromDB.Rows)
                                {
                                    if (rowInExcel[currentColumn.ColumnName].Text() == rowInDBToReplace[0].Text())//first column matched
                                    {
                                        rowInExcel[currentColumn.ColumnName] = rowInDBToReplace[1]; //second column replaces original value
                                        break;
                                    }
                                }
                            }
                        }
                        #endregion

                        //change column name if Options found for current column
                        currentColumn.ColumnName = columnNameB4Options;
                    }
                    #endregion
                }

                //event Pre import
                if (this.OnPreTableImport != null)
                {
                    this.OnPreTableImport(currentTable, tableOptionsObject, currentTableName);
                }

                currentTable.TableName = currentTableName;

                dataTableImporter.FromDataTable(currentTable, true);

                //Pre table import event
                if (this.OnPostTableImport != null)
                {
                    this.OnPostTableImport(currentTable, tableOptionsObject, currentTableName);
                }
            }

            //Last event Post import
            if (this.OnExcelFileImported != null)
            {
                this.OnExcelFileImported();
            }

            return listOfProcessedTables;
        }

        public event ExcelDataSetRead OnExcelDataSetRead;
        public event ExcelPreColumnOptionsWork OnExcelPreColumnOptionsWork;
        public event ExcelPreTableImport OnPreTableImport;
        public event ExcelPostTableImport OnPostTableImport;
        public event ExcelFileImported OnExcelFileImported;


        public void Dispose()
        {
            this.CloseConnection();
        }
    }

    public static class ExcelSheetImportExtensions
    {
        public static DataTable FindExcelSheetTable(this DataSet dataset, string tableIdentityOrName)
        {
            var jsDeserializerForOptions = new JavaScriptSerializer();

            foreach (DataTable currentTable in dataset.Tables)
            {
                var tableOptionsObject = new ExcelTableOptions();

                #region Exclude table
                if (currentTable.TableName.ToLower().EndsWith(".x"))
                {
                    continue;
                }
                #endregion

                var currentTableName = currentTable.TableName;

                #region Read table options

                if (currentTableName.Contains("{")
                        && currentTableName.Contains("}"))
                {
                    var tableOptionsText = currentTableName.Substring(currentTableName.IndexOf("{"));
                    //In table options JSON text, We use ^ instead of : and here we replace it
                    tableOptionsText = tableOptionsText.Replace("^", ":");
                    tableOptionsObject = jsDeserializerForOptions.Deserialize<ExcelTableOptions>(tableOptionsText);
                    currentTableName = currentTableName.Substring(0, currentTableName.IndexOf("{"));
                    tableOptionsObject.Id = currentTableName;
                }
                else
                {
                    tableOptionsObject.Id = currentTable.TableName;
                }

                #endregion

                if (tableOptionsObject.Id == tableIdentityOrName)
                {
                    return currentTable;
                }
            }

            return null;
        }

        public static DataColumn FindExcelSheetColumn(this DataTable table, string columnName)
        {
            var jsDeserializerForOptions = new JavaScriptSerializer();

            foreach (DataColumn currentColumn in table.Columns)
            {
                var name = currentColumn.ColumnName;

                if (currentColumn.ColumnName.Contains("{")
                        && currentColumn.ColumnName.Contains("}"))
                {
                    var columnOptionsText = currentColumn.ColumnName.Substring(currentColumn.ColumnName.IndexOf("{"));
                    var columnOptionsObject = jsDeserializerForOptions.Deserialize<ExcelColumnOptions>(columnOptionsText);
                    name = currentColumn.ColumnName.Substring(0, currentColumn.ColumnName.IndexOf("{"));
                }

                if (name == columnName)
                {
                    return currentColumn;
                }
            }

            return null;
        }
    }
}
