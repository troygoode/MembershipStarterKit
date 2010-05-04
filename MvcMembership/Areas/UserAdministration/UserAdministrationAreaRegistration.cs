using System.Web.Mvc;
using MvcMembership.Areas.UserAdministration.Controllers;

namespace MvcMembership.Areas.UserAdministration
{
	public class UserAdministrationAreaRegistration : AreaRegistration
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
				"MvcMembership_UserAdministration",
				"UserAdministration/{controller}/{action}/{id}",
				new { action = "Index", id = UrlParameter.Optional },
				new [] { typeof(UserAdministrationController).Namespace }
			);
		}
	}
}
