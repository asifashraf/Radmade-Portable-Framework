using System.Web.Mvc;

namespace WebAreas.Lib.Web
{
    public abstract class BaseController : Controller
    {
        protected JsonResult FormatJson(ResultType status, string text, object json = null)
        {            
            return Json(new { status = status.Name(), text = text, @object=json  }, 
                JsonRequestBehavior.AllowGet);
        }
    }
}
