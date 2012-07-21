using System;
using Areas.Common;

namespace Areas.WebAuth.Controllers
{
    public class TestWebAuthController : BaseController
    {
        public void GetToken()
        {
            var auth = new Auth();

            auth.CreateToken("1", "asif.log@gmail.com", DateTime.Now.AddDays(1));
        }

        public void RefreshToken()
        {
            var cookie = Request.Cookies[WebAuthSettings.CookieKey];

            if(cookie == null)
            {
                throw new WebAuthCookieNotFoundException();
            }

            var auth = new Auth();

            auth.RefreshToken(DateTime.Now.AddDays(2));

        }
    }
}
