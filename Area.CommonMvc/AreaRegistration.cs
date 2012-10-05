using System.Web.Mvc;
using MvcContrib.PortableAreas;

namespace Area.CommonMvc
{
    public class CommonAreaRegistration : PortableAreaRegistration
    {
        private const string AREA = "common";

        public override void RegisterArea(AreaRegistrationContext context, IApplicationBus bus)
        {
            /*context.MapRoute(
                AreaName + "_content",
                base.AreaRoutePrefix + "/Content/{resourceName}",
                new { controller = "EmbeddedResource", action = "Index", resourcePath = "content" },
                new[] { "MvcContrib.PortableAreas" }
            );
*/
            context.MapRoute("resources",
                AreaName + "/Resource/{resourceName}",
                new { Controller = "EmbeddedResource", action = "Index" },
                new[] { "MvcContrib.PortableAreas" });

            //base.RegisterArea(context, bus);

            context.MapRoute(AREA, AREA + "/{controller}/{action}/{id}",
                new { controller = "Ping", action = "Index", id = UrlParameter.Optional });

        }

        public override string AreaName
        {
            get { return AREA; }
        }
    }
}
