using System.Web.Mvc;

namespace MvcMembershipImport
{
    public class MvcMembershipImportAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "MvcMembershipImport";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "MvcMembershipImport",
                "Membership/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { typeof(MvcMembershipImport.Controllers.UserAdministrationController).Namespace }
            );
        }
    }
}
