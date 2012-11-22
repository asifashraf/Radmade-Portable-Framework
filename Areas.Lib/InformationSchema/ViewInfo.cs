using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace WebAreas.Lib.InformationSchema
{
    public class ViewInfo : ITableInfo
    {
        #region Fields

        private List<ViewColumn> _cols = null;

        #endregion Fields

        #region Constructors

        public ViewInfo() { }

        #endregion Constructors

        #region Properties

        public List<ViewColumn> Columns
        {
            get
            {                
                return _cols;
            }
            set
            {
                _cols = value;
            }
        }

        public string ConnectionString { get; set; }

        public string Name { get; set; }

        #endregion Properties
    }
}
