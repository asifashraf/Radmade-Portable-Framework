using System;
using System.Web.Mvc;

namespace Areas.WebAuth
{
    public class AuthorizationAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var auth = new Auth();

            auth.RefreshToken(DateTime.Now.AddDays(2));
        }
    }
}
