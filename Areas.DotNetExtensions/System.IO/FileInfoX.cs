using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Net;

    public static class FileInfoX
    {
		#region Fields (2) 

        static string _genPath = @"C:\file.txt";
        public static string NewLine = "\r\n";

		#endregion Fields 

		#region Properties (1) 

        public static FileInfo DefaultInstance
        {
            get
            {
                return new FileInfo(_genPath);
            }
        }

		#endregion Properties 

		#region Methods (13) 

		// Public Methods (13) 

        public static void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value.Trim());
            fs.Write(info, 0, info.Length);
        }

        public static void AddText(FileStream fs, string value, int startIndex, int endIndex)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value.Trim());
            fs.Write(info, startIndex, endIndex);
        }

        public static void CreateFile(this FileInfo file, string text)
        {
            // Delete the file if it exists.
            if (File.Exists(file.FullName))
            {
                File.Delete(file.FullName);
            }

            //Create the file.
            using (FileStream fs = File.Create(file.FullName))
            {
                AddText(fs, text);
            }
        }

        public static void CreateFile(this FileInfo file, byte[] data)
        {
            FileStream fs = null;

            fs = new FileStream(file.FullName, FileMode.Create, FileAccess.Write);

            fs.Write(data, 0, data.Length);
            fs.Close();
        }

        public static void CreateFile(this FileInfo file, string text,
            int startIndex, int endIndex)
        {
            // Delete the file if it exists.
            if (File.Exists(file.FullName))
            {
                try
                {
                    File.Delete(file.FullName);
                }
                catch { }
            }

            //Create the file.
            using (FileStream fs = File.Create(file.FullName))
            {
                AddText(fs, text, startIndex, endIndex);
            }
        }

        public static void Download(this FileInfo file, string Url)
        {
            Uri uri = new Uri(Url);
            WebClient ObjWebClient = new WebClient();
            ObjWebClient.DownloadFile(uri, file.FullName);
        }

        public static string Read(this FileInfo file)
        {
            //Open the stream and read it back.
            return File.ReadAllText(file.FullName);
        }

        public static List<string> ReadAllLines(this FileInfo file)
        {
            return File.ReadAllLines(file.FullName).ToList<string>();
        }

        public static DataTable ReadExcelFile(this FileInfo file)
        {
            String sConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0;",
                file.FullName);
            OleDbConnection objConn = new OleDbConnection(sConnectionString);
            objConn.Open();
            OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM [Sheet1$]", objConn);
            OleDbDataAdapter objAdapter1 = new OleDbDataAdapter();
            objAdapter1.SelectCommand = objCmdSelect;
            DataSet objDataset1 = new DataSet();
            objAdapter1.Fill(objDataset1);
            objConn.Close();
            return objDataset1.Tables[0];
        }

        public static string ReadFirstLine(this FileInfo file, bool skipEmptyRow)
        {
            try
            {
                return file.ReadTopRows(1, 0, skipEmptyRow)[0];
            }
            catch
            {
                return string.Empty;
            }
        }

        public static List<string> ReadHeadingsFromFirstRow(this FileInfo file, string delimiter)
        {
            string firstRow = file.ReadFirstLine(true);
            try
            {
                return firstRow.Split(firstRow, delimiter).ToList<string>();
            }
            catch
            {
                return new List<string>();
            }

        }

        public static List<string> ReadTopRows(this FileInfo file, int numOfRows, int skipFromTop, bool skipEmptyRows)
        {
            List<string> list = new List<string>();

            using (FileStream fs = File.OpenRead(file.FullName))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    for (int i = (numOfRows + skipFromTop); i > 0; i--)
                    {
                        string line = sr.ReadLine();
                        if (i <= numOfRows)
                        {
                            if (String.IsNullOrEmpty(line) && skipEmptyRows)
                            {
                                continue;
                            }
                            list.Add(line);
                        }
                    }
                }
            }
            return list;
        }

        public static FileInfo Search(
            this FileInfo instance,
            string fileName)
        {
            List<DirectoryInfo> roots =
            DirectoryInfoX.DefaultInstance.GetDriveRoots();
            FileInfo theFile = new FileInfo(_genPath);
            foreach (DirectoryInfo dir in roots)
            {
                FileInfo[] f = dir.GetFiles(fileName, SearchOption.AllDirectories);
                if (f.Length > 0)
                    theFile = f[0];
                break;
            }
            return theFile;
        }

		#endregion Methods 



        #region Read as DT........ call method from FlatFileReaders
        //public static DataTable ReadAsDT(this FileInfo file, int getTopRows,
        //    string delimiter, bool firstRowKeepHeadings, bool skipEmptyRows, out List<string> errors)
        //{
        //    errors = new List<string>();
        //    List<string> lines = new List<string>();
        //    List<string> headings = new List<string>();
        //    DataTable dataTable = new DataTable();
        //    if (getTopRows > 0)
        //    {
        //        lines = file.ReadTopRows(getTopRows, 0, skipEmptyRows);
        //    }
        //    else
        //    {
        //        lines = file.ReadAllLines();
        //    }

        //    //add headings
        //    headings = StringX.Inst.Split(lines[0], delimiter);
        //    if (firstRowKeepHeadings)
        //    {
        //        foreach (string h in headings)
        //        {
        //            dataTable.Columns.Add(new DataColumn(h));
        //        }
        //    }

        //    bool skipped = false;
        //    for(int l=0; l<lines.Count; l++)
        //    {
        //        string line = lines[l];
        //        if (!firstRowKeepHeadings)
        //        {
        //            skipped = true;
        //        }
        //        if (skipped)
        //        {
        //            List<string> data = line.Split(line, delimiter);
        //            if (data.Count != dataTable.Columns.Count)
        //            {
        //                errors.Add(string.Format("Data on line [{0}] does not match column headings",
        //                     l.ToString()));
        //                continue;
        //            }
        //            dataTable.Rows.Add(data.ToArray());
        //        }
        //        skipped = true;
        //    }
        //    return dataTable;
        //} 
        #endregion
    }

