using System;
using System.Web.Security;
using Moq;
using Xunit;

namespace MvcMembership.Tests
{
	public class PasswordServiceFacts
	{
		private readonly Random _rnd;
		private readonly Mock<MembershipProvider> _membershipProvider = new Mock<MembershipProvider>();
		private readonly IPasswordService _membershipWrapper;
		private readonly string _username;
		private readonly Mock<MembershipUser> _user = new Mock<MembershipUser>();

		public PasswordServiceFacts()
		{
			_rnd = new Random();
			_username = RandomString();
			_user.SetupGet(x => x.UserName).Returns(_username);
			_membershipWrapper = new AspNetMembershipProviderWrapper(_membershipProvider.Object);
		}

		[Fact]
		public void Unlock_calls_Unlock_via_provider()
		{
			//arrange
			_membershipProvider.Setup(x=> x.UnlockUser(_username)).Verifiable();

			//act
			_membershipWrapper.Unlock(_user.Object);

			//assert
			_membershipProvider.Verify();
		}

		[Fact]
		public void ChangePassword_calls_ChangePassword_via_provider()
		{
			//arrange
			var oldPassword = RandomString();
			var newPassword = RandomString();
			_membershipProvider.Setup(x => x.ChangePassword(_username, oldPassword, newPassword)).Returns(true).Verifiable();

			//act
			_membershipWrapper.ChangePassword(_user.Object, oldPassword, newPassword);

			//assert
			_membershipProvider.Verify();
		}

		[Fact]
		public void ChangePasswordQuestionAndAnswer_calls_ChangePasswordQuestionAndAnswer_via_provider()
		{
			//arrange
			var password = RandomString();
			var question = RandomString();
			var answer = RandomString();
			_membershipProvider.Setup(x => x.ChangePasswordQuestionAndAnswer(_username, password, question, answer)).Returns(true).Verifiable();

			//act
			_membershipWrapper.ChangePasswordQuestionAndAnswer(_user.Object, password, question, answer);

			//assert
			_membershipProvider.Verify();
		}

		[Fact]
		public void GetPasswordViaAnswer_calls_GetPassword_via_provider()
		{
			//arrange
			var answer = RandomString();
			var password = RandomString();
			_membershipProvider.Setup(x => x.GetPassword(_username, answer)).Returns(password).Verifiable();

			//act
			var result = _membershipWrapper.GetPassword(_user.Object, answer);

			//assert
			Assert.Equal(password, result);
			_membershipProvider.Verify();
		}

		[Fact]
		public void GetPassword_calls_GetPassword_via_user_object()
		{
			//arrange
			var password = RandomString();
			_user.Setup(x => x.GetPassword()).Returns(password).Verifiable();

			//act
			var result = _membershipWrapper.GetPassword(_user.Object);

			//assert
			Assert.Equal(password, result);
			_user.Verify();
		}

		[Fact]
		public void ResetPasswordViaAnswer_calls_ResetPassword_via_provider()
		{
			//arrange
			var answer = RandomString();
			var password = RandomString();
			_membershipProvider.Setup(x => x.ResetPassword(_username, answer)).Returns(password).Verifiable();

			//act
			var result = _membershipWrapper.ResetPassword(_user.Object, answer);

			//assert
			Assert.Equal(password, result);
			_membershipProvider.Verify();
		}

		[Fact]
		public void ResetPassword_calls_ResetPassword_on_supplied_user_object()
		{
			//arrange
			var password = RandomString();
			_user.Setup(x => x.ResetPassword()).Returns(password).Verifiable();

			//act
			var result = _membershipWrapper.ResetPassword(_user.Object);

			//assert
			Assert.Equal(password, result);
			_user.Verify();
		}

		private string RandomString()
		{
			return _rnd.Next().ToString();
		}
	}
}