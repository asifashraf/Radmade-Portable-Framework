using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Areas.Lib.LazySchema
{
    public class LazyTable
    {
        public string Name { get; set; }

        public string Schema { get; set; }

        public string FullName
        {
            get 
            { 
                return Schema.IsNullOrEmpty() ? 
                    string.Format("dbo.{0}", this.Name) 
                    : 
                    string.Format("{0}.{1}", Schema, Name); 
            }
        }
    }
}
