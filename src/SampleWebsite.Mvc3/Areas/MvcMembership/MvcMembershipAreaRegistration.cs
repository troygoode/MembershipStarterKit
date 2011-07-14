using System.Web.Mvc;

namespace SampleWebsite.Mvc3.Areas.MvcMembership
{
	public class MvcMembershipAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "MvcMembership";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				"MvcMembership_default",
				"UserAdministration/{action}/{id}",
				new { controller = "UserAdministration", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}