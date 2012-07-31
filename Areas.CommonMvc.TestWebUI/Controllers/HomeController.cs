using System.Web.Mvc;

namespace Areas.CommonMvc.TestWebUI.Controllers
{
    public class HomeController : Controller
    {
         public ViewResult Index()
         {
             return View();
         }
    }
}