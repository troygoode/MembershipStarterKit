using System;
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
			var randomPassword = Guid.NewGuid().ToString();
			_user.Setup(x => x.ResetPassword()).Returns(randomPassword);

			//act
			var newPassword = _membershipWrapper.ResetPassword(_user.Object);

			//assert
			Assert.Equal(randomPassword, newPassword);
		}

		[Fact]
		public void ResetPassword_calls_ResetPassword_on_supplied_user_object_and_passes_password_answer()
		{
			//arrange
			const string answer = "Some Answer";
			var randomPassword = Guid.NewGuid().ToString();
			_user.Setup(x => x.ResetPassword(answer)).Returns(randomPassword);

			//act
			var newPassword = _membershipWrapper.ResetPassword(_user.Object, answer);

			//assert
			Assert.Equal(randomPassword, newPassword);
		}

		[Fact]
		public void ChangePassword_resets_password_then_uses_new_password_to_change_password()
		{
			//arrange
			const string newPassword = "Lorem ipsum dolor.";
			var randomResetPassword = Guid.NewGuid().ToString();
			_user.Setup(x => x.ResetPassword()).Returns(randomResetPassword);
			_user.Setup(x => x.ChangePassword(randomResetPassword, newPassword)).Returns(true);

			//act
			_membershipWrapper.ChangePassword(_user.Object, newPassword);

			//assert
			_user.Verify(x => x.ChangePassword(randomResetPassword, newPassword));
		}

		[Fact]
		public void ChangePassword_throws_MembershipPasswordException_if_password_wasnt_changed()
		{
			//arrange
			_user.Setup(x => x.ChangePassword(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

			//act
			Assert.Throws<MembershipPasswordException>(() => _membershipWrapper.ChangePassword(_user.Object, "New Password"));
		}

		[Fact]
		public void ChangePassword_with_old_password_supplied_changes_password()
		{
			//arrange
			const string oldPassword = "Foo Bar";
			const string newPassword = "Lorem ipsum dolor.";
			_user.Setup(x => x.ChangePassword(oldPassword, newPassword)).Returns(true);

			//act
			_membershipWrapper.ChangePassword(_user.Object, oldPassword, newPassword);

			//assert
			_user.Verify(x => x.ChangePassword(oldPassword, newPassword));
		}

		[Fact]
		public void ChangePassword_with_old_password_supplied_throws_MembershipPasswordException_if_password_wasnt_changed()
		{
			//arrange
			_user.Setup(x => x.ChangePassword(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

			//act
			Assert.Throws<MembershipPasswordException>(() => _membershipWrapper.ChangePassword(_user.Object, "Old Password", "New Password"));
		}
	}
}