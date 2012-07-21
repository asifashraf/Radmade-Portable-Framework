using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using RadApi.Libs.Encryption;

namespace Areas.WebAuth
{
    public class Auth
    {
        public void CreateToken(string userPrimaryKey, string userNameOrEmail, DateTime expiryDate)
        {
            var token = new Token
                            {
                                Time =  DateTime.Now,
                                UserIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"],
                                UserPrimaryKey = userPrimaryKey,
                                UserNameOrEmail = userNameOrEmail,
                                Expiry = expiryDate
                            };
            var crypto = new Crypto();

            var tokenJs = (new JavaScriptSerializer()).Serialize(token);

            var encryptedToken = crypto.EncryptStringAES(tokenJs, WebAuthSettings.EncryptionSharedSecret);

            var cookie = new HttpCookie(WebAuthSettings.CookieKey, encryptedToken)
                             {Expires = expiryDate, HttpOnly = true};



            HttpContext.Current.Response.Cookies.Add(cookie);

            FormsAuthentication.SetAuthCookie(userNameOrEmail, true);

        }

        public Token GetAuthenticationData()
        {
            var cookie = HttpContext.Current.Request.Cookies[WebAuthSettings.CookieKey];

            if (cookie == null)
            {
                throw new WebAuthCookieNotFoundException();
            }

            var crypto = new Crypto();

            var tokenJs = crypto.DecryptStringAES(cookie.Value, WebAuthSettings.EncryptionSharedSecret);

            var token = (new JavaScriptSerializer()).Deserialize<Token>(tokenJs);

            if (token.Expiry < token.Time)
            {
                throw new TokenExpiredException();
            }

            if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != token.UserIPAddress)
            {
                throw new InvalidIPException();
            }

            return token;
        }

        public void RefreshToken(DateTime newExpiry)
        {
            var token = GetAuthenticationData();

            token.Expiry = newExpiry;

            CreateToken(token.UserPrimaryKey, token.UserNameOrEmail, token.Expiry);
        }

        public void RemoveToken()
        {
            var cookie = HttpContext.Current.Request.Cookies[WebAuthSettings.CookieKey];

            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1d);

                HttpContext.Current.Response.Cookies.Add(cookie);
            }

            FormsAuthentication.SignOut();
        }

        public bool TokenExists()
        {
            var token = GetAuthenticationData();

            return token != null;
        }

        public static void RedirectToLoginPageOnAuthenticationErrors()
        {
            var exc = HttpContext.Current.Server.GetLastError();

            if (exc is TokenExpiredException || exc is InvalidIPException || exc is WebAuthCookieNotFoundException)
            {
                var auth = new Auth();

                auth.RemoveToken();

                HttpContext.Current.Response.Redirect(string.Format(
                    "{0}?ReturnUrl={1}", WebAuthSettings.LoginUrl, HttpContext.Current.Server.UrlEncode(
                    HttpContext.Current.Request.RawUrl)) );
            }
        }
    }
}
