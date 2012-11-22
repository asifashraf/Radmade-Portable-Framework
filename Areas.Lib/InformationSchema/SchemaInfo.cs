using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAreas.Lib.InformationSchema
{
    public class SchemaInfo
    {
        public string CatalogName { get; set; }
        public string SchemaName { get; set; }
        public string SchemaOwner { get; set; }
        public string DefaultCharacterSetCatalog { get; set; }
        public string DefaultCharacterSetSchema { get; set; }
        public string DefaultCharacterSetName { get; set; }

        public List<ITableInfo> Tables { get; set; }
    }
}
