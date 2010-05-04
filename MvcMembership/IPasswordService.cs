using System.Web.Security;

namespace MvcMembership
{
	public interface IPasswordService
	{
		void Unlock(MembershipUser user);
		string ResetPassword(MembershipUser user, string passwordAnswer);
	}
}