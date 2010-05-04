using System.Collections.Generic;
using System.Web.Security;

namespace MvcMembership
{
	public interface IRolesService
	{
		//commands
		void Create(string roleName);
		void AddToRole(MembershipUser user, string roleName);
		void RemoveFromRole(MembershipUser user, string roleName);
		void Delete(string roleName);

		//queries
		IEnumerable<string> FindAll();
		IEnumerable<string> FindByUser(MembershipUser user);
		IEnumerable<string> FindByUserName(string userName);
		IEnumerable<string> FindUserNamesByRole(string roleName);
		bool IsInRole(MembershipUser user, string roleName);
	}
}