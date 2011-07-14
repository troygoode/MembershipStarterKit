using System.Web.Security;

namespace MvcMembership
{
	public interface IPasswordService
	{
		void Unlock(MembershipUser user);
		string ResetPassword(MembershipUser user);
		string ResetPassword(MembershipUser user, string passwordAnswer);
		void ChangePassword(MembershipUser user, string newPassword);
		void ChangePassword(MembershipUser user, string oldPassword, string newPassword);
	}
}