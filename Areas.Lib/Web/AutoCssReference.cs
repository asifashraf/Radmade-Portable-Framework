using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using WebAreas.Lib.Config;

namespace WebAreas.Lib.Web
{
    public static class AutoCssReference
    {
        public static string CssFile(string controller, string action)
        {
            return string.Format("/Content/Css/uncommon/{0}/{1}.css", controller, action);
        }

        public static string CssFile(ViewContext view, dynamic viewName)
        {
            string rightView = viewName ?? view.RouteData.Values["action"];
            return string.Format("/Content/Css/uncommon/{0}/{1}.css",
                view.RouteData.Values["controller"], rightView);
        }

        public static string CssControllerFile(ViewContext view)
        {
            return string.Format("/Content/Css/uncommon/{0}/{0}.css",
                view.RouteData.Values["controller"]);
        }

        static string CssKey(ViewContext view)
        {
            return string.Format("uncommon_{0}_{1}_css",
                view.RouteData.Values["controller"],
                view.RouteData.Values["action"]);
        }

        static string CssControllerKey(ViewContext view)
        {
            return string.Format("uncommon_{0}_controller_css",
                view.RouteData.Values["controller"]);
        }

        public static bool FindCssFile(ViewContext view, dynamic viewName)
        {
            //if cache enabled
            var context = HttpContext.Current;
            var key = CssKey(view);
            //check from cache
            var config = SettingsHelper.GetObject<MvcViewsHelpersConfig>();
            if(config.EnableCacheInApplicationStore){
                
                if (context.Application[key] != null)
                {
                    return Convert.ToBoolean(context.Application[key]);
                }
            }
            //end if cache enabled block

            var path = "~" + CssFile(view, viewName);
            var serverPath = context.Server.MapPath(path);
            var file = new FileInfo(serverPath);

            //if cache enabled
            if(config.EnableCacheInApplicationStore)
            {
                context.Application[key] = file.Exists;    
            }
            //end if cache enabled

            return file.Exists;
        }

        public static bool FindCssControllerFile(ViewContext view)
        {
            var context = HttpContext.Current;

            //check from cache
            var key = CssControllerKey(view);

            var config = SettingsHelper.GetObject<MvcViewsHelpersConfig>();
            if(config.EnableCacheInApplicationStore)
            {
                if (context.Application[key] != null)
                {
                    return Convert.ToBoolean(context.Application[key]);
                }
            }

            var path = "~" + CssControllerFile(view);
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