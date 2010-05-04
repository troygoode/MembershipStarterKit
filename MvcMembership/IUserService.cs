using System.Web.Security;
using PagedList;

namespace MvcMembership
{
	public interface IUserService
	{
		//commands
		//todo: add create user method
		void Update(MembershipUser user);
		void Touch(MembershipUser user);
		void Touch(string userName);
		void Touch(object providerUserKey);
		void Delete(MembershipUser user);

		//queries
		IPagedList<MembershipUser> FindAll(int pageIndex, int pageSize);
		IPagedList<MembershipUser> FindByEmail(string emailAddressToMatch, int pageIndex, int pageSize);
		IPagedList<MembershipUser> FindByUserName(string userNameToMatch, int pageIndex, int pageSize);
		MembershipUser Get(string userName);
		MembershipUser Get(object providerUserKey);
		int TotalUsers { get; }
		int UsersOnline { get; }
	}
}