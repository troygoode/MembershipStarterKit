using System.Threading;
using System.Web;
using System.Web.Mvc;
using SampleWebsite.Mvc3.Helpers;

namespace SampleWebsite.Mvc3.Areas.MvcMembership.Controllers
{
   public class BaseController : Controller
   {
      protected override void ExecuteCore()
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


         // Modify current thread's cultures            
         Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
         Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

         base.ExecuteCore();
      }
   }
}
