using System.Web.Mvc;

namespace Areas.CommonMvc.TestWebUI.Controllers
{
    using System.Web.Script.Serialization;

    using WebAreas.Lib.Web;

    public class HomeController : BaseController
    {
         public ActionResult Index()
         {

             return View();
         }
    }
}