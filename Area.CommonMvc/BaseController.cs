using System.Web.Mvc;

namespace Area.CommonMvc
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
