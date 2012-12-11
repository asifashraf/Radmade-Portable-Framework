using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAreas.DotNetExtensions.System.Web.MVC
{
    public class FieldErrorsDetail
    {
        public string Name { get; set; }

        public List<string> Errors { get; set; }

        public object Value { get; set; }
    }
}
