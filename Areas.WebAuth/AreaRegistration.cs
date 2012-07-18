using System.Web.Mvc;
using MvcContrib.PortableAreas;

namespace Areas.WebAuth
{
    public class AreaRegistration : PortableAreaRegistration
    {
        private const string AREA = "webauth";

        public override void RegisterArea(AreaRegistrationContext context, IApplicationBus bus)
        {
            context.MapRoute(AREA, AREA + "/{controller}/{action}/{id}",
                new { id = UrlParameter.Optional });


            RegisterAreaEmbeddedResources();
        }

        public override string AreaName
        {
            get { return AREA; }
        }
    }
}
