using System.Collections.Generic;
using System.Web.Security;

namespace SampleWebsite.Mvc3.Areas.MvcMembership.Models.UserAdministration
{
	public class RoleViewModel
	{
		public string Role { get; set; }
		public IDictionary<string, MembershipUser> Users { get; set; }
	}
}