using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using WebAreas.DotNetExtensions.System.Web.MVC;

public static class ModelStateDictionaryX
{
    public static List<FieldErrorsDetail> Errors(this ModelStateDictionary modelState)
    {
        var errors = new List<FieldErrorsDetail>();
        foreach (var key in modelState.Keys)
        {
            var error = new FieldErrorsDetail();
            error.Name = key;
            error.Errors = modelState[key].Errors.Select(e => e.ErrorMessage).ToListSafely();
            error.Value = modelState[key].Value.RawValue;
            errors.Add(error);
        }
        return errors;
    }
}

