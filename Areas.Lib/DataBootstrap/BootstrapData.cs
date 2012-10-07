using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Areas.Lib.DataBootstrap
{
    public class BootstrapData
    {
        /// <summary>
        /// Either source column name or shceme to enter data
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Table name of this column
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Column name in the database
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// If this is foreign key column then FKColumn is the parent column name
        /// </summary>
        public string FKColumn { get; set; }

        /// <summary>
        /// If this is foreign key column then FKTable is the parent table name
        /// </summary>
        public string FKTable { get; set; }

        /// <summary>
        /// If this is foreign key column then FKPick is the number of top rows to spread. 
        /// In case of inline values the foreign key pick will multiply the inline values by top pick
        /// In case of source column binding from sample database this pick will be spread in a 
        /// sequence of top to bottom repeatedly in a fixed sequence and pattern.
        /// </summary>
        public int FKPick { get; set; }
        
    }
}
