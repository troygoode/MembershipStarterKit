using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using MvcContrib.PortableAreas;

namespace SampleWebsite
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterRoutes( RouteCollection routes )
		{
			routes.IgnoreRoute( "{resource}.axd/{*pathInfo}" );

			routes.MapRoute(
				"Default",                                              // Route name
				"{controller}/{action}/{id}",                           // URL with parameters
				new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
			);

		}

		protected void Application_Start()
		{
            AreaRegistration.RegisterAllAreas();

            PortableAreaRegistration.RegisterEmbeddedViewEngine();

            RegisterRoutes(RouteTable.Routes);
        }

		protected void Application_AuthenticateRequest()
		{
			if (HttpContext.Current.User != null)
				Membership.GetUser(true);
		}
	}
}