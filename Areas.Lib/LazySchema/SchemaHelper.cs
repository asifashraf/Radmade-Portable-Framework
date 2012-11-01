using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Areas.Lib.LazySchema
{
    /// <summary>
    /// Does not fetch anything in constructor. 
    /// You can call different functions and every function will fetch its data lazily when its called. 
    /// This is different from InfoSchema because InfoSchema loads all database schema in the constructor.
    /// </summary>
    public class SchemaHelper
    {
        public DataHelper db;
        public SchemaHelper(string connectionString)
        {
            db = new DataHelper(connectionString);
        }

        /// <summary>
        /// Loads collection of primary keys on the given table name.
        /// </summary>
        /// <param name="TableName">Name of the table</param>
        /// <returns></returns>
        public List<LazyPrimaryKey> GetPrimaryKeyNamesByTableName(string TableName)
        {
            var query = @"select 
 ind.name, 
 ind.object_id as TableId,
 col.name as PkColumnName,
 dsp.name as space_name, 
 ind.index_id, 
 ind.type, 
 ind.is_unique, ind.ignore_dup_key, ind.is_primary_key, ind.is_unique_constraint, ind.fill_factor, ind.is_padded, 
 ind.is_disabled, ind.is_hypothetical, ind.allow_row_locks, ind.allow_page_locks, 
 convert(bit, IndexProperty(ind.object_id, ind.name, N'IsFulltextKey')) as is_FulltextKey, 
 sta.no_recompute, ind_col.index_column_id,  ind_col.key_ordinal, ind_col.is_descending_key, 
 ind_col.is_included_column, ind_col.partition_ordinal , ind_col.column_id , ind.data_space_id 
 from sys.indexes ind left outer join sys.stats sta 
 on sta.object_id = ind.object_id and sta.stats_id = ind.index_id 
 left outer join (sys.index_columns ind_col 
 inner join sys.columns col on col.object_id = ind_col.object_id and col.column_id = ind_col.column_id )  
 on ind_col.object_id = ind.object_id and ind_col.index_id = ind.index_id left outer join sys.data_spaces dsp 
 on dsp.data_space_id = ind.data_space_id  
 where ind.object_id = object_id(N'[[TableName]]') 
and is_primary_key = 1  and ind.index_id >= 0 and ind.type <> 3 and ind.type <> 4 
and ind.is_hypothetical = 0   order by ind.index_id, ind_col.key_ordinal"
                .Replace("[[TableName]]", TableName);
            return db.GetTypedList<LazyPrimaryKey>(query);
            
        }

        public List<LazyTable> GetTables()
        {
            return db.GetTypedList<LazyTable>("Select table_name as [Name],table_schema as [Schema] from information_schema.Tables Where table_name <> 'sysdiagrams' AND Table_type = 'BASE TABLE'");
        }

        public List<LazyFk> GetForeignKeys()
        {
            var query = @"--Query to find foreign key to other tables
                        select Fk.name as Name, Fk.object_id as ObjectId, Fk.is_disabled as IsDisabled, 
                        Fk.is_not_for_replication as IsNotFotReplication, 
                        Fk.delete_referential_action as DeleteReferentialAction, 
                        Fk.update_referential_action as UpdateReferentialAction, 
                        object_name(Fk.parent_object_id) as FkTableName, 
                        schema_name(Fk.schema_id) as FkTableSchema, 
                        TbR.name as PkTableName, 
                        schema_name(TbR.schema_id) as PkTableSchema, 
                        col_name(Fk.parent_object_id, 
                        Fk_Cl.parent_column_id) as FkColumnName, 
                        col_name(Fk.referenced_object_id, 
                        Fk_Cl.referenced_column_id) as PkColumnName, 
                        Fk_Cl.constraint_column_id as ConstraintColumnId, 
                        Fk.is_not_trusted as IsNotTrusted
                        from sys.foreign_keys Fk 
                        left outer join sys.tables TbR on TbR.object_id = Fk.referenced_object_id 
                        inner join sys.foreign_key_columns Fk_Cl on Fk_Cl.constraint_object_id = Fk.object_id ";

            return db.GetTypedList<LazyFk>(query);
        }
    }
}
