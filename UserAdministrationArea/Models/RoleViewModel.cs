using System.Collections.Generic;
using System.Web.Security;

namespace UserAdministration.Models
{
	public class RoleViewModel
	{
		public string Role { get; set; }
		public IEnumerable<MembershipUser> Users { get; set; }
	}
}