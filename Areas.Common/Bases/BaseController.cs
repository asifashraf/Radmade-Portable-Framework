using System.Web.Mvc;
using Areas.Common.Json;

namespace Areas.Common.Bases
{
    public abstract class BaseController : Controller
    {
        protected JsonResult FormatJson(ResultType messageStatus, string msg, object dataObject, string specialCode = "")
        {
            //return raw result
            if (messageStatus == ResultType.raw)
            {
                return Json(dataObject, JsonRequestBehavior.AllowGet);
            }

            //format json object
            var json = new JsonMessage(messageStatus, msg, dataObject, specialCode);

            return Json(json, JsonRequestBehavior.AllowGet);

        }
    }
}
