using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Areas.Lib.Config;

namespace Areas.Lib.Web
{
    public static class AutoJsReference
    {
        public static string JsFile(string controller, string action)
        {
            return String.Format("/Content/Js/uncommon/{0}/{1}.js", controller, action);
        }

        public static string JsFile(ViewContext view, dynamic viewName)
        {
            string rightView = viewName ?? view.RouteData.Values["action"];
            return String.Format("/Content/Js/uncommon/{0}/{1}.js",
                                 view.RouteData.Values["controller"], rightView);
        }

        public static string JsControllerFile(ViewContext view)
        {
            return String.Format("/Content/Js/uncommon/{0}/{0}.js",
                view.RouteData.Values["controller"]);
        }

        static string JsKey(ViewContext view)
        {
            return String.Format("uncommon_{0}_{1}_js",
                view.RouteData.Values["controller"],
                view.RouteData.Values["action"]);
        }

        static string JsControllerKey(ViewContext view)
        {
            return String.Format("uncommon_{0}_controller_js",
                view.RouteData.Values["controller"]);
        }

        public static bool FindJsFile(ViewContext view, dynamic viewName)
        {
            var context = HttpContext.Current;

            //check from cache
            var key = JsKey(view);
            var config = SettingsHelper.GetObject<MvcViewsHelpersConfig>();
            if(config.EnableCacheInApplicationStore)
            {
                if (context.Application[key] != null)
                {
                    return Convert.ToBoolean(context.Application[key]);
                }
            }

            var path = "~" + JsFile(view, viewName);
            var serverPath = context.Server.MapPath(path);
            var file = new FileInfo(serverPath);
            if(config.EnableCacheInApplicationStore)
            {
                context.Application[key] = file.Exists;    
            }
            return file.Exists;
        }

        public static bool FindJsControllerFile(ViewContext view)
        {
            var context = HttpContext.Current;

            //check from cache
            var key = JsControllerKey(view);

            var config = SettingsHelper.GetObject<MvcViewsHelpersConfig>();
            if(config.EnableCacheInApplicationStore)
            {
                if (context.Application[key] != null)
                {
                    return Convert.ToBoolean(context.Application[key]);
                }
            }

            var path = "~" + JsControllerFile(view);
            var serverPath = context.Server.MapPath(path);
            var file = new FileInfo(serverPath);
            if(config.EnableCacheInApplicationStore)
            {
                context.Application[key] = file.Exists;    
            }

            return file.Exists;
        }
    }
}