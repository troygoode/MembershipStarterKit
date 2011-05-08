using System.Web.Mvc;
using MvcContrib.PortableAreas;

namespace UserAdministration
{
    public class UserAdministrationRegistration : PortableAreaRegistration
	{
		public override string AreaName
		{
			get
			{
                return "UserAdministration";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context, IApplicationBus bus)
		{
            context.MapRoute("resources",
                AreaName + "/Resource/{resourceName}",
                new { Controller = "EmbeddedResource", action = "Index" }, 
                new string[] {"MvcContrib.PortableAreas"});

            base.RegisterArea(context, bus);

			context.MapRoute(
                "UserAdministration", 
                AreaName + "/{controller}/{action}/{id}",
                new { controller = "UserAdministration", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
