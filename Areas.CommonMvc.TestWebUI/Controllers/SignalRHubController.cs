using System.Web.Mvc;

namespace Areas.CommonMvc.TestWebUI.Controllers
{
    public class SignalRHubController : Controller
    {
        //
        // GET: /SignalR/

        public ActionResult MessageBroadcaster()
        {
            return View();
        }

    }
}
