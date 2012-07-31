using System.Web.Mvc;

namespace Areas.Lib.Web
{
    public abstract class BaseController : Controller
    {
        protected JsonResult FormatJson(ResultType messageStatus, string msg, object dataObject, string specialCode = "")
        {
            //format json object
            var json = new JsonMessage(messageStatus, msg, dataObject, specialCode);

            return Json(json, JsonRequestBehavior.AllowGet);

        }
    }
}
