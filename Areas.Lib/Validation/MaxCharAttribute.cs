using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace WebAreas.Lib.Validation
{
    public class MaxCharAttribute : MaxLengthAttribute
    {
        public MaxCharAttribute(int length)
            : base(length)
        {

        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("Maximum {0} characters are allowed for {1}", this.Length, name);
        }
    }
}
