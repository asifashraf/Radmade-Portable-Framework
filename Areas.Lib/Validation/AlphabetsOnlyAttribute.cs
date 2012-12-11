using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebAreas.Lib.Validation
{
    public class AlphabetsOnlyAttribute : ValidationAttribute, IClientValidatable
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var regex = new Regex(@"^[a-zA-Z]+$");
            if (regex.IsMatch(value.Text()).Not())
            {
                return new ValidationResult("Only alphabets allowed");
            }
            return ValidationResult.Success;
        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
            ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule();
            rule.ValidationType = "alphabetsOnly";
            rule.ErrorMessage = "Only alphabets allowed";
            //rule.ValidationParameters.Add
            //("param", DateTime.Now.ToString("dd-MM-yyyy"));
            return new List<ModelClientValidationRule> 
            { 
                rule 
            };
        }
        
    }
}
