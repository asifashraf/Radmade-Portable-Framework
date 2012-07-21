using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

    public static class DataTableX
    {
        #region Methods (5)

        // Public Methods (5) 

        public static void DisplayOnConsole(this DataTable table)
        {
            Console.Write(table.ToGridString());
        }

        public static List<T> ParseToObjectList<T>(this DataTable table)
        {
            Type t = typeof(T);
            ConstructorInfo ci = t.GetConstructor(new Type[] { });
            List<T> list = new List<T>();
            PropertyInfo[] arrp = t.GetProperties();
            foreach (DataRow row in table.Rows)
            {
                object instance = ci.Invoke(new object[] { });
                foreach (PropertyInfo pi in arrp)
                {
                    try
                    {
                        pi.SetValue(instance, row[pi.Name], null);
                    }
                    catch { }
                }
                list.Add(instance.CastTo<T>());
            }

            return list;
        }

        public static DataTable Start(this DataTable table, IEnumerable<string> columns)
        {
            table = new DataTable();
            foreach (string c in columns)
            {
                table.Columns.Add(c);
            }
            return table;
        }

        public static DataTable Start(this DataTable table, IEnumerable<string> columns,
            string counterColumnName)
        {
            table = new DataTable();
            table.Columns.Add(counterColumnName);
            foreach (string c in columns)
            {
                table.Columns.Add(c);
            }
            return table;
        }

        public static string ToGridString(this DataTable table)
        {
            string text = string.Empty;
            foreach (DataColumn c in table.Columns)
            {
                text += string.Format(c.ColumnName + ",\t");
            }

            foreach (DataRow row in table.Rows)
            {
                text += string.Format("\r\n");
                foreach (DataColumn c in row.Table.Columns)
                {
                    text += string.Format("{0} \t", row[c.ColumnName].ToString());
                }
            }
            return text;
        }

        #endregion Methods
    }

