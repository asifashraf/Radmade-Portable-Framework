using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace WebAreas.Lib.InformationSchema
{
    public static class ExtensionMethods
    {
        /// <summary>
        ///<example>('table1','table2','table3')</example> 
        /// </summary>
        public static string ToWhereClauseInCommaList(this IEnumerable<string> list)
        {
            if (list.Count() == 0)
                return string.Empty;
            StringBuilder sb = new StringBuilder("(");
            bool isFirst = true;
            foreach (string s in list)
            {
                if (!isFirst)
                {
                    sb.Append(",");
                }
                sb.Append(string.Format("'{0}'", s));                
                isFirst = false;
            }
            sb.Append(")");
            return sb.ToString();
        }
        public static bool IsColumnInPrimaryKeys(this TableInfo table, TableColumn column)
        {
            IEnumerable<string> query = from pks in table.PKColumns
                                        where "".MatchByString(column.Name, pks.Name)
                                        select pks.Name;
            return query.HaveMembers();
        }
        public static string MakeParams(this TableInfo table)
        {
            string columnParams = string.Empty;
            string commaPcs = string.Empty;
            foreach (TableColumn pcs in table.Columns)
            {
                columnParams += string.Format("{2}{0} {1}",
                    InfoSchema.ParseType(pcs.DataType), pcs.Name.WithFirstCharLower() + table.ColumnParamsSuffix, commaPcs);
                commaPcs = ",";
            }
            return columnParams;
        }
        public static string MakeParamsNoIdentity(this TableInfo table)
        {
            string columnParams = string.Empty;
            string commaPcs = string.Empty;
            foreach (TableColumn column in table.Columns)
            {
                if (table.HasIdentityPK && table.IsColumnInPrimaryKeys(column))
                {
                    continue;
                }
                columnParams += string.Format("{2}{0} {1}",
                    InfoSchema.ParseType(column.DataType), column.Name.WithFirstCharLower() + table.ColumnParamsSuffix, commaPcs);
                commaPcs = ",";

            }
            return columnParams;
        }
        public static string MakeAssignments(this TableInfo table, string entityVarName)
        {
            StringBuilder sb = new StringBuilder();
            foreach (TableColumn cs in table.Columns)
            {
                string colName = cs.Name.WithFirstCharUpper();
                string singleTableName = cs.TableName.Singularize().WithFirstCharUpper();
                if (colName.MatchCaseSensitive(singleTableName))
                {
                    colName = colName + "1";
                }

                AppendLine(sb, "			{0}.{1} = {2};", entityVarName, colName,
                    cs.Name.WithFirstCharLower() + table.ColumnParamsSuffix);
            }
            return sb.ToString();
        }

        static void AppendLine(StringBuilder sb, string text, params object[] parameters)
        {
            //    string[] arrStrings = new string[parameters.Length];
            //    for (int i = 0; i < parameters.Length; i++)
            //    {
            //        arrStrings[i] = parameters[i].ToString();
            //    }
            sb.AppendLine(string.Format(text, parameters));
            //arrStrings = null;
            //text = null;
            //parameters = null;
            parameters = null;
        }

        public static string MakeAssignmentsNoSeed(this TableInfo table, string entityVarName)
        {
            StringBuilder sb = new StringBuilder();
            foreach (TableColumn cs in table.Columns)
            {
                if (table.HasIdentityPK && table.IsColumnInPrimaryKeys(cs))
                {
                    continue;
                }

                AppendLine(sb,"			{0}.{1} = {2};", entityVarName, cs.Name.WithFirstCharUpper(),
                    cs.Name.WithFirstCharLower() + table.ColumnParamsSuffix);
            }
            return sb.ToString();
        }
    }
}
