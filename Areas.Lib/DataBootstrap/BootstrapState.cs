using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Areas.Lib.DataBootstrap
{
    public class BootstrapState
    {
        public BootstrapState(string deleteStatements) 
        {
            this.DeleteStatements = deleteStatements;
        }

        public string DeleteStatements { get; set; }

    }
}
