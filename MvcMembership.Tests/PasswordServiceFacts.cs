using System.Web.Security;
using Moq;
using Xunit;

namespace MvcMembership.Tests
{
	public class PasswordServiceFacts
	{
		private readonly Mock<MembershipProvider> _membershipProvider = new Mock<MembershipProvider>();
		private readonly AspNetMembershipProviderWrapper _membershipWrapper;
		private readonly Mock<MembershipUser> _user = new Mock<MembershipUser>();

		public PasswordServiceFacts()
		{
			_membershipWrapper = new AspNetMembershipProviderWrapper(_membershipProvider.Object);
		}

		[Fact]
		public void Unlock_calls_Unlock_on_supplied_user_object()
		{
			//act
			_membershipWrapper.Unlock(_user.Object);

			//assert
			_user.Verify(x => x.UnlockUser());
		}

		[Fact]
		public void ResetPassword_calls_ResetPassword_on_supplied_user_object()
		{
			//arrange
			const string answer = "Some Answer";

			//act
			_membershipWrapper.ResetPassword(_user.Object, answer);

			//assert
			_user.Verify(x => x.ResetPassword(answer));
		}
	}
}