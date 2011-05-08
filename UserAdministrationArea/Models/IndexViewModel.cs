using System.Collections.Generic;
using System.Web.Security;
using PagedList;

namespace UserAdministration.Models
{
	public class IndexViewModel
	{
		public IPagedList<MembershipUser> Users { get; set; }
		public IEnumerable<string> Roles { get; set; }
	}
}