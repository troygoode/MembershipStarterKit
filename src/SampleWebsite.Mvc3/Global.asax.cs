using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SampleWebsite.Mvc3.Helpers;

namespace SampleWebsite.Mvc3
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

         routes.MapRoute(
            "Default", // Route name
            "{controller}/{action}/{id}", // URL with parameters
            new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
         );

      }

      public override string GetVaryByCustomString(HttpContext context, string arg)
      {
         // It seems this executes multiple times and early, so we need to extract language again from cookie.
         if (arg == "culture") // culture name (e.g. "en-US") is what should vary caching
         {
            string cultureName = null;
            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
               cultureName = cultureCookie.Value;
            else
               cultureName = Request.UserLanguages[0]; // obtain it from HTTP header AcceptLanguages

            // Validate culture name
            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

            return cultureName.ToLower();// use culture name as cache key, "es", "en-us", "es-cl", etc.
         }

         return base.GetVaryByCustomString(context, arg);
      }

      protected void Application_Start()
      {
         AreaRegistration.RegisterAllAreas();

         RegisterGlobalFilters(GlobalFilters.Filters);
         RegisterRoutes(RouteTable.Routes);
      }
   }
}