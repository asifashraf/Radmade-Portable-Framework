using System;
using System.Web;
using System.Web.Mvc;
using Areas.Common.Bases;
using Areas.Common.Json;
using Areas.WebAuth.Exceptions;
using Areas.WebAuth.Types;

namespace Areas.WebAuth.Controllers
{
    public class TestWebAuthController : BaseController
    {
        public JsonResult GetToken()
        {
            var auth = new Auth();

            var token = auth.CreateToken("1", "asif.log@gmail.com", "192.168.1.1", DateTime.Now.AddDays(1));

            Response.Cookies.Add(new HttpCookie(WebAuthSettings.CookieKey, token));

            return FormatJson(ResultType.Raw, "", token);
        }

        public JsonResult RefreshToken()
        {
            var cookie = Request.Cookies[WebAuthSettings.CookieKey];

            if(cookie == null)
            {
                throw new WebAuthCookieNotFoundException();
            }

            var auth = new Auth();

            var newToken = auth.RefreshToken(cookie.Value, DateTime.Now.AddDays(2));

            Response.Cookies.Add(new HttpCookie(WebAuthSettings.CookieKey, newToken));

            return FormatJson(ResultType.Raw, "", newToken);


        }
    }
}
