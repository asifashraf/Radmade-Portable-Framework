using System.Web.Mvc;
using MvcContrib.PortableAreas;

namespace Area.CommonMvc
{
    public class AreaRegistration : PortableAreaRegistration
    {
        private const string AREA = "common";

        public override void RegisterArea(AreaRegistrationContext context, IApplicationBus bus)
        {
            context.MapRoute(AREA, AREA + "/{controller}/{action}/{id}",
                new { controller = "Ping", action = "Index", id = UrlParameter.Optional });


            RegisterAreaEmbeddedResources();
        }

        public override string AreaName
        {
            get { return AREA; }
        }
    }
}
