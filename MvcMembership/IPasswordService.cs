using System.Web.Security;

namespace MvcMembership
{
	public interface IPasswordService
	{
		//commands
		void Unlock(MembershipUser user);
		void ChangePassword(MembershipUser user, string oldPassword, string newPassword);
		void ChangePasswordQuestionAndAnswer(MembershipUser user, string password, string question, string answer);

		//queries
		string GetPassword(MembershipUser user, string passwordAnswer);
		string GetPassword(MembershipUser user);

		//command AND query :-(
		//todo: figure out a clean way to separate this via CQS
		string ResetPassword(MembershipUser user, string passwordAnswer);
		string ResetPassword(MembershipUser user);
	}
}