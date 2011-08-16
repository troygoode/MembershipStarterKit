using System.Web.Security;
using PagedList;

namespace MvcMembership
{
	public interface IUserService
	{
		int TotalUsers { get; }
		int UsersOnline{ get; }
        IPagedList<MembershipUser> FindAll(int pageNumber, int pageSize);
        IPagedList<MembershipUser> FindByEmail(string emailAddressToMatch, int pageNumber, int pageSize);
        IPagedList<MembershipUser> FindByUserName(string userNameToMatch, int pageNumber, int pageSize);
		MembershipUser Get(string userName);
		MembershipUser Get(object providerUserKey);
		MembershipUser Create(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved);
		MembershipUser Create(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey);
		void Update(MembershipUser user);
		void Delete(MembershipUser user);
		void Delete(MembershipUser user, bool deleteAllRelatedData);
		MembershipUser Touch(MembershipUser user);
		MembershipUser Touch(string userName);
		MembershipUser Touch(object providerUserKey);
	}
}