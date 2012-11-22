using System.Web.Mvc;
using System.Web.Routing;
using Area.CommonMvc;
using WebAreas.Lib.Web;
using MvcContrib.PortableAreas;

namespace Areas.CommonMvc.TestWebUI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.ashx/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            /*AreaRegistration.RegisterAllAreas();*/
            AreaRegistrationUtil.RegisterAreasForAssemblyOf<CommonAreaRegistration>();
            
            //Portable areas configuration
            PortableAreaRegistration.RegisterEmbeddedViewEngine();

            RegisterGlobalFilters(GlobalFilters.Filters);

            RegisterRoutes(RouteTable.Routes);
            
        }
    }
}