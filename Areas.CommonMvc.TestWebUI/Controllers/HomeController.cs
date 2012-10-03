using System.Web.Mvc;

namespace Areas.CommonMvc.TestWebUI.Controllers
{
    using System.Web.Script.Serialization;

    using Areas.Lib.UploadProgress.Upload;
    using Areas.Lib.Web;

    public class HomeController : BaseController
    {
         public ActionResult Index()
         {

             return View();
         }
    }
}