using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace WebAreas.Lib.InformationSchema
{
    public class ConstraintInfo
    {
        #region Fields (10)

        public string constraint_name;
        public string constraint_type;
        public string field_name;
        public int field_position;
        public string match_type;
        public string on_delete;
        public string on_update;
        public string references_field;
        public string references_table;
        public string table_name;

        #endregion Fields

        #region Methods

        // Public Methods (4) 

        public List<ConstraintInfo> GetForiegnKeyOnTable(string connectionString, string tableName)
        {
            List<ConstraintInfo> list = new List<ConstraintInfo>();
            SqlConnection conn;
            using (IDataReader reader = SqlCommandX.Instance.GetReader(
                connectionString, query_fk_on_table(table_name), CommandType.Text,
                out conn))
            {
                while (reader.Read())
                {
                    ConstraintInfo cs = new ConstraintInfo();
                    if (DBNull.Value != reader["table_name"])
                    {
                        cs.table_name = reader["table_name"].ToString();
                    }
                    if (DBNull.Value != reader["field_name"])
                    {
                        cs.field_name = reader["field_name"].ToString();
                    }
                    if (DBNull.Value != reader["constraint_type"])
                    {
                        cs.constraint_type = reader["constraint_type"].ToString();
                    }
                    if (DBNull.Value != reader["constraint_name"])
                    {
                        cs.constraint_name = reader["constraint_name"].ToString();
                    }

                    if (DBNull.Value != reader["match_type"])
                    {
                        cs.match_type = reader["match_type"].ToString();
                    }

                    if (DBNull.Value != reader["on_update"])
                    {
                        cs.on_update = reader["on_update"].ToString();
                    }

                    if (DBNull.Value != reader["on_delete"])
                    {
                        cs.on_delete = reader["on_delete"].ToString();
                    }

                    if (DBNull.Value != reader["references_table"])
                    {
                        cs.references_table = reader["references_table"].ToString();
                    }

                    if (DBNull.Value != reader["references_field"])
                    {
                        cs.references_field = reader["references_field"].ToString();
                    }

                    if (DBNull.Value != reader["field_position"])
                    {
                        cs.field_position = int.Parse(reader["field_position"].ToString());
                    }
                    list.Add(cs);
                }
            }
            conn.Destroy();
            return list;
        }

        public List<ConstraintInfo> GetPrimaryKeyOnTable(string connectionString, string tableName)
        {
            List<ConstraintInfo> list = new List<ConstraintInfo>();

            SqlConnection conn;
            using (IDataReader reader = SqlCommandX.Instance.GetReader(
                connectionString, query_pk_on_table(table_name), CommandType.Text,
                out conn))
            {
                while (reader.Read())
                {
                    ConstraintInfo cs = new ConstraintInfo();
                    if (DBNull.Value != reader["table_name"])
                    {
                        cs.table_name = reader["table_name"].ToString();
                    }
                    if (DBNull.Value != reader["field_name"])
                    {
                        cs.field_name = reader["field_name"].ToString();
                    }
                    if (DBNull.Value != reader["constraint_type"])
                    {
                        cs.constraint_type = reader["constraint_type"].ToString();
                    }
                    if (DBNull.Value != reader["constraint_name"])
                    {
                        cs.constraint_name = reader["constraint_name"].ToString();
                    }

                    if (DBNull.Value != reader["match_type"])
                    {
                        cs.match_type = reader["match_type"].ToString();
                    }

                    if (DBNull.Value != reader["on_update"])
                    {
                        cs.on_update = reader["on_update"].ToString();
                    }

                    if (DBNull.Value != reader["on_delete"])
                    {
                        cs.on_delete = reader["on_delete"].ToString();
                    }

                    if (DBNull.Value != reader["references_table"])
                    {
                        cs.references_table = reader["references_table"].ToString();
                    }

                    if (DBNull.Value != reader["references_field"])
                    {
                        cs.references_field = reader["references_field"].ToString();
                    }

                    if (DBNull.Value != reader["field_position"])
                    {
                        cs.field_position = int.Parse(reader["field_position"].ToString());
                    }
                    list.Add(cs);
                }
            }
            return list;
        }

        public static List<TableColumn> MatchColumns(List<ConstraintInfo> constraints, TableInfo table)
        {
            List<TableColumn> list = new List<TableColumn>();
            List<TableColumn> tableColumns = table.Columns;

            foreach (ConstraintInfo cs in constraints)
            {
                var query = from c in tableColumns
                            where c.Name == cs.field_name
                            select c;
                if (query.Count<TableColumn>() > 0)
                {
                    list.Add(query.First<TableColumn>());
                }
            }
            return list;
        }

        #endregion Methods
        
        #region queries
        public static string queryWithFilter(params string[] tablesFilter)
        {
            string filter = tablesFilter.ToWhereClauseInCommaList();
            return @"SELECT k.table_name,
          k.column_name field_name,
          c.constraint_type,
			c.constraint_name,
          CASE c.is_deferrable WHEN 'NO' THEN 0 ELSE 1 END 'is_deferrable',
          CASE c.initially_deferred WHEN 'NO' THEN 0 ELSE 1 END 'is_deferred',
          rc.match_option 'match_type',
 		  rc.update_rule 'on_update',
          rc.delete_rule 'on_delete',
          ccu.table_name 'references_table',
          ccu.column_name 'references_field',
          k.ordinal_position 'field_position'
     FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE k
     LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS c
       ON k.table_name = c.table_name
      AND k.table_schema = c.table_schema
      AND k.table_catalog = c.table_catalog
      AND k.constraint_catalog = c.constraint_catalog
      AND k.constraint_name = c.constraint_name
LEFT JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc
       ON rc.constraint_schema = c.constraint_schema
      AND rc.constraint_catalog = c.constraint_catalog
      AND rc.constraint_name = c.constraint_name
LEFT JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu
       ON rc.unique_constraint_schema = ccu.constraint_schema
      AND rc.unique_constraint_catalog = ccu.constraint_catalog
      AND rc.unique_constraint_name = ccu.constraint_name
    WHERE k.constraint_catalog = DB_NAME()
AND table_name IN [[filter]]
 ORDER BY k.constraint_name,
          k.ordinal_position".Replace("[[filter]]", filter);
        }
        public static string query = @"SELECT k.table_name,
          k.column_name field_name,
          c.constraint_type,
			c.constraint_name,
          CASE c.is_deferrable WHEN 'NO' THEN 0 ELSE 1 END 'is_deferrable',
          CASE c.initially_deferred WHEN 'NO' THEN 0 ELSE 1 END 'is_deferred',
          rc.match_option 'match_type',
 		  rc.update_rule 'on_update',
          rc.delete_rule 'on_delete',
          ccu.table_name 'references_table',
          ccu.column_name 'references_field',
          k.ordinal_position 'field_position'
     FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE k
     LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS c
       ON k.table_name = c.table_name
      AND k.table_schema = c.table_schema
      AND k.table_catalog = c.table_catalog
      AND k.constraint_catalog = c.constraint_catalog
      AND k.constraint_name = c.constraint_name
LEFT JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc
       ON rc.constraint_schema = c.constraint_schema
      AND rc.constraint_catalog = c.constraint_catalog
      AND rc.constraint_name = c.constraint_name
LEFT JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu
       ON rc.unique_constraint_schema = ccu.constraint_schema
      AND rc.unique_constraint_catalog = ccu.constraint_catalog
      AND rc.unique_constraint_name = ccu.constraint_name
    WHERE k.constraint_catalog = DB_NAME()
 ORDER BY k.constraint_name,
          k.ordinal_position";

        public static string query_pk_on_table(string tableName)
        {
            return @"SELECT k.table_name,
          k.column_name field_name,
          c.constraint_type,
            c.constraint_name,
          CASE c.is_deferrable WHEN 'NO' THEN 0 ELSE 1 END 'is_deferrable',
          CASE c.initially_deferred WHEN 'NO' THEN 0 ELSE 1 END 'is_deferred',
          rc.match_option 'match_type',
 		  rc.update_rule 'on_update',
          rc.delete_rule 'on_delete',
          ccu.table_name 'references_table',
          ccu.column_name 'references_field',
          k.ordinal_position 'field_position'
     FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE k
     LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS c
       ON k.table_name = c.table_name
      AND k.table_schema = c.table_schema
      AND k.table_catalog = c.table_catalog
      AND k.constraint_catalog = c.constraint_catalog
      AND k.constraint_name = c.constraint_name
LEFT JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc
       ON rc.constraint_schema = c.constraint_schema
      AND rc.constraint_catalog = c.constraint_catalog
      AND rc.constraint_name = c.constraint_name
LEFT JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu
       ON rc.unique_constraint_schema = ccu.constraint_schema
      AND rc.unique_constraint_catalog = ccu.constraint_catalog
      AND rc.unique_constraint_name = ccu.constraint_name
    WHERE k.constraint_catalog = DB_NAME()
      AND k.table_name = 'Store_Import_Export_Queues'
AND	c.Constraint_type = 'PRIMARY KEY'
 ORDER BY k.constraint_name,
          k.ordinal_position".Replace("Store_Import_Export_Queues", tableName);
        }

        public static string query_fk_on_table(string tableName)
        {
            return @"SELECT k.table_name,
          k.column_name field_name,
          c.constraint_type,
            c.constraint_name,      
          CASE c.is_deferrable WHEN 'NO' THEN 0 ELSE 1 END 'is_deferrable',
          CASE c.initially_deferred WHEN 'NO' THEN 0 ELSE 1 END 'is_deferred',
          rc.match_option 'match_type',
 		  rc.update_rule 'on_update',
          rc.delete_rule 'on_delete',
          ccu.table_name 'references_table',
          ccu.column_name 'references_field',
          k.ordinal_position 'field_position'
     FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE k
     LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS c
       ON k.table_name = c.table_name
      AND k.table_schema = c.table_schema
      AND k.table_catalog = c.table_catalog
      AND k.constraint_catalog = c.constraint_catalog
      AND k.constraint_name = c.constraint_name
LEFT JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc
       ON rc.constraint_schema = c.constraint_schema
      AND rc.constraint_catalog = c.constraint_catalog
      AND rc.constraint_name = c.constraint_name
LEFT JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu
       ON rc.unique_constraint_schema = ccu.constraint_schema
      AND rc.unique_constraint_catalog = ccu.constraint_catalog
      AND rc.unique_constraint_name = ccu.constraint_name
    WHERE k.constraint_catalog = DB_NAME()
      AND k.table_name = 'Store_Import_Export_Queues'
AND	c.Constraint_type = 'FOREIGN KEY'
 ORDER BY k.constraint_name,
          k.ordinal_position".Replace("Store_Import_Export_Queues", tableName);
        }

        public static string query_by_table(string table_name)
        {
            return @"SELECT k.table_name,
          k.column_name field_name,
          c.constraint_type,
            c.constraint_name,
          CASE c.is_deferrable WHEN 'NO' THEN 0 ELSE 1 END 'is_deferrable',
          CASE c.initially_deferred WHEN 'NO' THEN 0 ELSE 1 END 'is_deferred',
          rc.match_option 'match_type',
 		  rc.update_rule 'on_update',
          rc.delete_rule 'on_delete',
          ccu.table_name 'references_table',
          ccu.column_name 'references_field',
          k.ordinal_position 'field_position'
     FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE k
     LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS c
       ON k.table_name = c.table_name
      AND k.table_schema = c.table_schema
      AND k.table_catalog = c.table_catalog
      AND k.constraint_catalog = c.constraint_catalog
      AND k.constraint_name = c.constraint_name
LEFT JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc
       ON rc.constraint_schema = c.constraint_schema
      AND rc.constraint_catalog = c.constraint_catalog
      AND rc.constraint_name = c.constraint_name
LEFT JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ccu
       ON rc.unique_constraint_schema = ccu.constraint_schema
      AND rc.unique_constraint_catalog = ccu.constraint_catalog
      AND rc.unique_constraint_name = ccu.constraint_name
    WHERE k.constraint_catalog = DB_NAME()
      AND k.table_name = 'testconstraints2'
 ORDER BY k.constraint_name,
          k.ordinal_position
".Replace("testconstraints2", table_name);
        }

        #endregion
    }
}
