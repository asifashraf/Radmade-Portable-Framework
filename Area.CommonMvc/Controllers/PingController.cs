using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;

namespace Area.CommonMvc.Controllers
{
    public class PingController : BaseController
    {
        public JsonResult GetBrowserInfo()
        {
            var browser = Request.Browser;
            var s = "Browser Capabilities\n"
                + "Type = " + browser.Type + "\n"
                + "Name = " + browser.Browser + "\n"
                + "Version = " + browser.Version + "\n"
                + "Major Version = " + browser.MajorVersion + "\n"
                + "Minor Version = " + browser.MinorVersion + "\n"
                + "Platform = " + browser.Platform + "\n"
                + "Is Beta = " + browser.Beta + "\n"
                + "Is Crawler = " + browser.Crawler + "\n"
                + "Is AOL = " + browser.AOL + "\n"
                + "Is Win16 = " + browser.Win16 + "\n"
                + "Is Win32 = " + browser.Win32 + "\n"
                + "Supports Frames = " + browser.Frames + "\n"
                + "Supports Tables = " + browser.Tables + "\n"
                + "Supports Cookies = " + browser.Cookies + "\n"
                + "Supports VBScript = " + browser.VBScript + "\n"
                + "Supports JavaScript = " +
                    browser.EcmaScriptVersion + "\n"
                + "Supports Java Applets = " + browser.JavaApplets + "\n"
                + "Supports ActiveX Controls = " + browser.ActiveXControls
                      + "\n"
                + "Supports JavaScript Version = " +
                    browser["JavaScriptVersion"] + "\n";

            return FormatJson(ResultType.data, "Browser capabilities", s);
        }

        public JsonResult RequestStatus()
        {
            var cookies = new Dictionary<string, string>();

            foreach (string cookieName in Request.Cookies.AllKeys)
            {
                if (!cookies.ContainsKey(cookieName))
                {
                    var httpCookie = Request.Cookies[cookieName];
                    if (httpCookie != null) cookies.Add(cookieName, httpCookie.Value);
                }
            }

            return FormatJson(ResultType.data, "Request status", 
                new
                {
                    Time = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                    Session,
                    User,
                    Cookie = cookies
                });
        }
    }
}
