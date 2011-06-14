using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Security;
using Moq;
using MvcMembership;
using MvcMembership.Settings;
using PagedList;
using UserAdministration.Controllers;
using UserAdministration.Models;
using Xunit;
using Xunit.Extensions;

namespace UserAdministrationArea.Tests
{
	public class UserAdministrationControllerFacts
	{
		private readonly UserAdministrationController _controller;
		private readonly Mock<IMembershipSettings> _membershipSettings = new Mock<IMembershipSettings>();
		private readonly Mock<IUserService> _userService = new Mock<IUserService>();
		private readonly Mock<IPasswordService> _passwordService = new Mock<IPasswordService>();
		private readonly Mock<IRolesService> _rolesService = new Mock<IRolesService>();
		private readonly Mock<ISmtpClient> _smtpClient = new Mock<ISmtpClient>();

		public UserAdministrationControllerFacts()
		{
			_controller = new UserAdministrationController(_membershipSettings.Object, _userService.Object, _passwordService.Object, _rolesService.Object, _smtpClient.Object);
		}

		[Fact]
		public void Index_returns_default_view()
		{
			//act
			var result = _controller.Index(1);

			//assert
			Assert.IsType<ViewResult>(result);
			Assert.Empty(result.ViewName);
		}

		[Fact]
		public void Index_retrieves_users_from_membershipService_and_passes_to_view()
		{
			//arrange
			var users = new Mock<IPagedList<MembershipUser>>().Object;
			_userService.Setup(x => x.FindAll(It.IsAny<int>(), It.IsAny<int>())).Returns(users).Verifiable();

			//act
			var result = _controller.Index(1);

			//assert
			_userService.Verify();
			var viewModel = Assert.IsType<IndexViewModel>(result.ViewData.Model);
			Assert.Same(users, viewModel.Users);
		}

		[Fact]
		public void Index_retrieves_roles_from_rolesService_and_passes_to_view()
		{
			//arrange
			var roles = new[] { "one", "two", "three", "four" };
			_rolesService.Setup(x => x.FindAll()).Returns(roles).Verifiable();

			//act
			var result = _controller.Index(1);

			//assert
			_rolesService.Verify();
			var viewModel = Assert.IsType<IndexViewModel>(result.ViewData.Model);
			Assert.Same(roles, viewModel.Roles);
		}

		[Fact]
		public void Index_allows_the_pageIndex_to_be_specified_for_paging()
		{
			//arrange
			const int index = 5;

			//act
			_controller.Index(index);

			//assert
			_userService.Verify(x => x.FindAll(It.Is<int>(v => v == index), It.IsAny<int>()));
		}

		[Fact]
		public void Index_defaults_to_the_first_page_if_no_pageIndex_is_specified()
		{
			//act
			_controller.Index(null);

			//assert
			_userService.Verify(x => x.FindAll(It.Is<int>(v => v == 0), It.IsAny<int>()));
		}

		[Fact]
		public void CreateRole_redirects_to_index()
		{
			//act
			var result = _controller.CreateRole("");

			//assert
			Assert.Equal("Index", result.RouteValues["action"]);
			Assert.Null(result.RouteValues["controller"]);
		}

		[Fact]
		public void CreateRole_creates_role()
		{
			//arrange
			var role = new Random().Next().ToString();

			//act
			_controller.CreateRole(role);

			//assert
			_rolesService.Verify(x => x.Create(role));
		}

		[Fact]
		public void DeleteRole_redirects_to_index()
		{
			//act
			var result = _controller.DeleteRole("");

			//assert
			Assert.Equal("Index", result.RouteValues["action"]);
			Assert.Null(result.RouteValues["controller"]);
		}

		[Fact]
		public void DeleteRole_creates_role()
		{
			//arrange
			var role = new Random().Next().ToString();

			//act
			_controller.DeleteRole(role);

			//assert
			_rolesService.Verify(x => x.Delete(role));
		}

		[Fact]
		public void Role_returns_default_view()
		{
			//act
			var result = _controller.Role("");

			//assert
			Assert.IsType<ViewResult>(result);
			Assert.Empty(result.ViewName);
		}

		[Fact]
		public void Role_looks_up_users_in_role_and_passes_to_view()
		{
			//arrange
			var role = new Random().Next().ToString();
			var usernames = new List<string>();
			_rolesService.Setup(x => x.FindUserNamesByRole(role)).Returns(usernames).Verifiable();
			_userService.Setup(x => x.Get(It.Is<string>(username => usernames.Contains(username)))).Returns(
				new Mock<MembershipUser>().Object);

			//act
			var result = _controller.Role(role);

			//assert
			_rolesService.Verify();
			var viewModel = Assert.IsType<RoleViewModel>(result.ViewData.Model);
			Assert.Equal(role, viewModel.Role);
			Assert.Equal(usernames.Count, viewModel.Users.Count());
		}

		[Fact]
		public void Details_returns_default_view()
		{
			//arrange
			_membershipSettings.SetupGet(x => x.Password.ResetOrRetrieval.CanReset).Returns(true);
			_userService.Setup(x => x.Get(It.IsAny<Guid>())).Returns(new Mock<MembershipUser>().Object);

			//act
			var result = _controller.Details(Guid.Empty);

			//assert
			Assert.IsType<ViewResult>(result);
			Assert.Empty(result.ViewName);
		}

		[Fact]
		public void Details_looks_MembershipUser_up_by_id_and_passes_to_view()
		{
			//arrange
			var id = Guid.NewGuid();
			var user = new Mock<MembershipUser>().Object;
			_membershipSettings.SetupGet(x => x.Password.ResetOrRetrieval.CanReset).Returns(true);
			_userService.Setup(x => x.Get(id)).Returns(user).Verifiable();

			//act
			var result = _controller.Details(id);

			//assert
			_userService.Verify();
			var viewModel = Assert.IsType<DetailsViewModel>(result.ViewData.Model);
			Assert.Same(viewModel.User, user);
		}

		[Fact]
		public void Details_looks_roles_up_by_username_and_passes_to_view()
		{
			//arrange
			_membershipSettings.SetupGet(x => x.Password.ResetOrRetrieval.CanReset).Returns(true);
			var id = Guid.NewGuid();
			var username = new Random().Next().ToString();
			var user = new Mock<MembershipUser>();
			user.SetupGet(x => x.UserName).Returns(username);
			_userService.Setup(x => x.Get(id)).Returns(user.Object);
			var rolesSuperset = new[] { "one", "two", "three", "four", "five" };
			var rolesSubset = new[] { "two", "four" };
			_rolesService.Setup(x => x.FindAll()).Returns(rolesSuperset).Verifiable();
			_rolesService.Setup(x => x.FindByUser(user.Object)).Returns(rolesSubset).Verifiable();

			//act
			var result = _controller.Details(id);

			//assert
			var viewModel = Assert.IsType<DetailsViewModel>(result.ViewData.Model);
			Assert.Equal(rolesSuperset.Length, viewModel.Roles.Count);
			foreach (var role in rolesSuperset)
			{
				Assert.True(viewModel.Roles.ContainsKey(role));
				Assert.Equal(rolesSubset.Contains(role), viewModel.Roles[role]);
			}
		}

		[Fact]
		public void Details_uses_UserName_as_DisplayName()
		{
			//arrange
			_membershipSettings.SetupGet(x => x.Password.ResetOrRetrieval.CanReset).Returns(true);
			var id = Guid.NewGuid();
			const string username = "Lorem Ipsum";
			var user = new Mock<MembershipUser>();
			user.SetupGet(x => x.UserName).Returns(username);
			_userService.Setup(x => x.Get(id)).Returns(user.Object);

			//act
			var result = _controller.Details(id);

			//assert
			var viewModel = Assert.IsType<DetailsViewModel>(result.ViewData.Model);
			Assert.Same(viewModel.DisplayName, username);
		}

		[Fact]
		public void DetailsPost_redirects_to_DetailsGet()
		{
			//arrange
			var id = Guid.NewGuid();
			var user = new Mock<MembershipUser>();
			_userService.Setup(x => x.Get(id)).Returns(user.Object).Verifiable();

			//act
			var result = _controller.Details(id, "", "");

			//assert
			Assert.Equal("Details", result.RouteValues["action"]);
			Assert.Null(result.RouteValues["controller"]);
			Assert.Equal(id, result.RouteValues["id"]);
		}

		[Fact]
		public void DetailsPost_updates_email_address()
		{
			//arrange
			var id = Guid.NewGuid();
			var email = new Random().Next().ToString();
			var user = new Mock<MembershipUser>();
			user.SetupProperty(x => x.Email);
			_userService.Setup(x => x.Get(id)).Returns(user.Object).Verifiable();

			//act
			_controller.Details(id, email, "");

			//assert
			_userService.Verify();
			_userService.Verify(x => x.Update(It.Is<MembershipUser>(v => v == user.Object && v.Email == email)));
		}

		[Fact]
		public void DetailsPost_updates_comment()
		{
			//arrange
			var id = Guid.NewGuid();
			var comment = new Random().Next().ToString();
			var user = new Mock<MembershipUser>();
			user.SetupProperty(x => x.Comment);
			_userService.Setup(x => x.Get(id)).Returns(user.Object).Verifiable();

			//act
			_controller.Details(id, "", comment);

			//assert
			_userService.Verify();
			_userService.Verify(x => x.Update(It.Is<MembershipUser>(v => v == user.Object && v.Comment == comment)));
		}

		[Fact]
		public void DeleteUser_redirects_to_index()
		{
			//arrange
			var id = Guid.NewGuid();

			//act
			var result = _controller.DeleteUser(id);

			//assert
			Assert.Equal("Index", result.RouteValues["action"]);
			Assert.Null(result.RouteValues["controller"]);
		}

		[Fact]
		public void DeleteUser_deletes_user()
		{
			//arrange
			var id = Guid.NewGuid();
			var user = new Mock<MembershipUser>();
			_userService.Setup(x => x.Get(id)).Returns(user.Object).Verifiable();

			//act
			_controller.DeleteUser(id);

			//assert
			_userService.Verify();
			_userService.Verify(x => x.Delete(user.Object));
		}

		[Fact]
		public void ChangeApproval_redirects_to_Details()
		{
			//arrange
			var id = Guid.NewGuid();
			var user = new Mock<MembershipUser>();
			_userService.Setup(x => x.Get(id)).Returns(user.Object).Verifiable();

			//act
			var result = _controller.ChangeApproval(id, true);

			//assert
			Assert.Equal("Details", result.RouteValues["action"]);
			Assert.Null(result.RouteValues["controller"]);
			Assert.Equal(id, result.RouteValues["id"]);
		}

		[Theory, InlineData(true), InlineData(false)]
		public void ChangeApproval_approves_and_unapproves_user(bool isApproved)
		{
			//arrange
			var id = Guid.NewGuid();
			var user = new Mock<MembershipUser>();
			user.SetupProperty(x => x.IsApproved);
			_userService.Setup(x => x.Get(id)).Returns(user.Object).Verifiable();

			//act
			_controller.ChangeApproval(id, isApproved);

			//assert
			_userService.Verify(x => x.Update(It.Is<MembershipUser>(v => v == user.Object && v.IsApproved == isApproved)));
		}

		[Fact]
		public void Unlock_redirects_to_Details()
		{
			//arrange
			var id = Guid.NewGuid();

			//act
			var result = _controller.Unlock(id);

			//assert
			Assert.Equal("Details", result.RouteValues["action"]);
			Assert.Null(result.RouteValues["controller"]);
			Assert.Equal(id, result.RouteValues["id"]);
		}

		[Fact]
		public void Unlock_calls_Unlock_method()
		{
			//arrange
			var id = Guid.NewGuid();
			var user = new Mock<MembershipUser>();
			_userService.Setup(x => x.Get(id)).Returns(user.Object).Verifiable();

			//act
			_controller.Unlock(id);

			//assert
			_passwordService.Verify(x => x.Unlock(user.Object));
		}

		[Fact]
		public void ResetPassword_redirects_to_Details()
		{
			//arrange
			var id = Guid.NewGuid();
			var user = new Mock<MembershipUser>();
			user.SetupGet(x => x.Email).Returns(new Random().Next() + "@domain.com");
			_userService.Setup(x => x.Get(id)).Returns(user.Object);

			//act
			var result = _controller.ResetPassword(id);

			//assert
			Assert.Equal("Password", result.RouteValues["action"]);
			Assert.Null(result.RouteValues["controller"]);
			Assert.Equal(id, result.RouteValues["id"]);
		}

		[Fact]
		public void ResetPassword_calls_ResetPassword_method()
		{
			//arrange
			var id = Guid.NewGuid();
			var user = new Mock<MembershipUser>();
			user.SetupGet(x => x.Email).Returns(new Random().Next() + "@domain.com");
			_userService.Setup(x => x.Get(id)).Returns(user.Object);

			//act
			_controller.ResetPassword(id);

			//assert
			_passwordService.Verify(x => x.ResetPassword(user.Object));
		}

		[Fact]
		public void ResetPassword_sends_new_password_to_user_via_email()
		{
			//arrange
			var id = Guid.NewGuid();
			//arrange - generate new password
			var newPassword = new Random().Next().ToString();
			_passwordService.Setup(x => x.ResetPassword(It.IsAny<MembershipUser>())).Returns(newPassword);
			var emailAddress = new Random().Next() + "@domain.com";
			//arrange - retrieve user & email address
			var user = new Mock<MembershipUser>();
			user.SetupGet(x => x.Email).Returns(emailAddress);
			_userService.Setup(x => x.Get(id)).Returns(user.Object);
			//arrange - verify the message that is sent to the user
			var emailIsValid = false;
			_smtpClient.Setup(x => x.Send(It.IsAny<MailMessage>())).Callback<MailMessage>(msg =>
			{
				if (msg.To.Count == 1 &&
				msg.To[0].Address == emailAddress &&
				msg.Body.Contains(newPassword))
					emailIsValid = true;
			});

			//act
			_controller.ResetPassword(id);

			//assert
			Assert.True(emailIsValid);
		}

		[Fact]
		public void ResetPasswordWithAnswer_redirects_to_Details()
		{
			//arrange
			var id = Guid.NewGuid();
			var user = new Mock<MembershipUser>();
			user.SetupGet(x => x.Email).Returns(new Random().Next() + "@domain.com");
			_userService.Setup(x => x.Get(id)).Returns(user.Object);

			//act
			var result = _controller.ResetPasswordWithAnswer(id, string.Empty);

			//assert
			Assert.Equal("Password", result.RouteValues["action"]);
			Assert.Null(result.RouteValues["controller"]);
			Assert.Equal(id, result.RouteValues["id"]);
		}

		[Fact]
		public void ResetPasswordWithAnswer_calls_ResetPassword_method_and_passes_password_answer()
		{
			//arrange
			var id = Guid.NewGuid();
			var answer = new Random().Next().ToString();
			var user = new Mock<MembershipUser>();
			user.SetupGet(x => x.Email).Returns(new Random().Next() + "@domain.com");
			_userService.Setup(x => x.Get(id)).Returns(user.Object);

			//act
			_controller.ResetPasswordWithAnswer(id, answer);

			//assert
			_passwordService.Verify(x => x.ResetPassword(user.Object, answer));
		}

		[Fact]
		public void ResetPasswordWithAnswer_sends_new_password_to_user_via_email()
		{
			//arrange
			var id = Guid.NewGuid();
			//arrange - generate new password
			var newPassword = new Random().Next().ToString();
			_passwordService.Setup(x => x.ResetPassword(It.IsAny<MembershipUser>(), It.IsAny<string>())).Returns(newPassword);
			var emailAddress = new Random().Next() + "@domain.com";
			//arrange - retrieve user & email address
			var user = new Mock<MembershipUser>();
			user.SetupGet(x => x.Email).Returns(emailAddress);
			_userService.Setup(x => x.Get(id)).Returns(user.Object);
			//arrange - verify the message that is sent to the user
			var emailIsValid = false;
			_smtpClient.Setup(x => x.Send(It.IsAny<MailMessage>())).Callback<MailMessage>(msg =>
																							{
																								if (msg.To.Count == 1 &&
																								msg.To[0].Address == emailAddress &&
																								msg.Body.Contains(newPassword))
																									emailIsValid = true;
																							});

			//act
			_controller.ResetPasswordWithAnswer(id, "password answer");

			//assert
			Assert.True(emailIsValid);
		}

		[Fact]
		public void SetPassword_redirects_to_Details()
		{
			//arrange
			var id = Guid.NewGuid();
			var user = new Mock<MembershipUser>();
			user.SetupGet(x => x.Email).Returns(new Random().Next() + "@domain.com");
			_userService.Setup(x => x.Get(id)).Returns(user.Object);

			//act
			var result = _controller.SetPassword(id, string.Empty);

			//assert
			Assert.Equal("Password", result.RouteValues["action"]);
			Assert.Null(result.RouteValues["controller"]);
			Assert.Equal(id, result.RouteValues["id"]);
		}

		[Fact]
		public void SetPassword_calls_ChangePassword_method_and_passes_new_password()
		{
			//arrange
			var id = Guid.NewGuid();
			var password = new Random().Next().ToString();
			var user = new Mock<MembershipUser>();
			user.SetupGet(x => x.Email).Returns(new Random().Next() + "@domain.com");
			_userService.Setup(x => x.Get(id)).Returns(user.Object);

			//act
			_controller.SetPassword(id, password);

			//assert
			_passwordService.Verify(x => x.ChangePassword(user.Object, password));
		}

		[Fact]
		public void SetPassword_sends_new_password_to_user_via_email()
		{
			//arrange
			var id = Guid.NewGuid();
			//arrange - generate new password
			var newPassword = new Random().Next().ToString();
			var emailAddress = new Random().Next() + "@domain.com";
			//arrange - retrieve user & email address
			var user = new Mock<MembershipUser>();
			user.SetupGet(x => x.Email).Returns(emailAddress);
			_userService.Setup(x => x.Get(id)).Returns(user.Object);
			//arrange - verify the message that is sent to the user
			var emailIsValid = false;
			_smtpClient.Setup(x => x.Send(It.IsAny<MailMessage>())).Callback<MailMessage>(msg =>
			{
				if (msg.To.Count == 1 &&
				msg.To[0].Address == emailAddress &&
				msg.Body.Contains(newPassword))
					emailIsValid = true;
			});

			//act
			_controller.SetPassword(id, newPassword);

			//assert
			Assert.True(emailIsValid);
		}

		[Fact]
		public void AddToRole_redirects_to_details()
		{
			//arrange
			var id = Guid.NewGuid();

			//act
			var result = _controller.AddToRole(id, "");

			//assert
			Assert.Equal("UsersRoles", result.RouteValues["action"]);
			Assert.Null(result.RouteValues["controller"]);
			Assert.Equal(id, result.RouteValues["id"]);
		}

		[Fact]
		public void AddToRole_adds_user_to_role()
		{
			//arrange
			var id = Guid.NewGuid();
			var user = new Mock<MembershipUser>().Object;
			_userService.Setup(x => x.Get(id)).Returns(user).Verifiable();
			var role = new Random().Next().ToString();

			//act
			_controller.AddToRole(id, role);

			//assert
			_userService.Verify();
			_rolesService.Verify(x => x.AddToRole(user, role));
		}

		[Fact]
		public void RemoveFromRole_redirects_to_details()
		{
			//arrange
			var id = Guid.NewGuid();

			//act
			var result = _controller.RemoveFromRole(id, "");

			//assert
			Assert.Equal("UsersRoles", result.RouteValues["action"]);
			Assert.Null(result.RouteValues["controller"]);
			Assert.Equal(id, result.RouteValues["id"]);
		}

		[Fact]
		public void RemoveFromRole_removes_user_from_role()
		{
			//arrange
			var id = Guid.NewGuid();
			var user = new Mock<MembershipUser>().Object;
			_userService.Setup(x => x.Get(id)).Returns(user).Verifiable();
			var role = new Random().Next().ToString();

			//act
			_controller.RemoveFromRole(id, role);

			//assert
			_userService.Verify();
			_rolesService.Verify(x => x.RemoveFromRole(user, role));
		}

        [Fact]
        public void CreateUser_creates_a_new_user() 
        {
            // arrange
			var user = new Mock<MembershipUser>().Object;
            const string username = "jdoe";
            const string password = "password";
            const string email = "jdoe@example.com";
            const string passwordQuestion = "What is your quest?";
            const string passwordAnswer = "To seek the Holy Grail";
            _userService.Setup(x => x.Create(username, password, email, passwordQuestion, passwordAnswer)).Returns(user).Verifiable();

            // act
            var viewModel = new CreateUserViewModel { Username = username, 
                                                      Password = password, 
                                                      Email = email, 
                                                      PasswordQuestion = passwordQuestion, 
                                                      PasswordAnswer = passwordAnswer };
            _controller.CreateUser(viewModel);

            // assert
            _userService.Verify();
        }

        [Fact]
        public void CreateUser_redirects_to_newly_created_user() 
        {
            // arrange
            var id = Guid.NewGuid();
            var user = new Mock<MembershipUser>();
            user.Setup(x => x.ProviderUserKey).Returns(id);
            _userService.Setup(x => x.Create(null, null, null, null, null)).Returns(user.Object);

            // act
            var returnedResult = _controller.CreateUser(new CreateUserViewModel());

            // assert
            var result = Assert.IsType<RedirectToRouteResult>(returnedResult);
            Assert.Equal("Details", result.RouteValues["action"]);
            Assert.Null(result.RouteValues["controller"]);
            Assert.Equal(id, result.RouteValues["id"]);
        }

        [Fact]
        public void CreateUser_displays_username_validation_error_when_duplicate_username_supplied() 
        {
            // arrange
            var exception = new MembershipCreateUserException(MembershipCreateStatus.DuplicateUserName);
            _userService.Setup(x => x.Create(null, null, null, null, null)).Throws(exception);
            var viewModel = new CreateUserViewModel();

            // act
            var returnedResult = _controller.CreateUser(viewModel);

            // assert
            var result = Assert.IsType<ViewResult>(returnedResult);
            Assert.Equal("", result.ViewName);
            Assert.Equal(viewModel, result.Model);
            var modelState = result.ViewData.ModelState;
            Assert.True(modelState.ContainsKey(string.Empty));
            Assert.Equal(exception.Message, modelState[string.Empty].Errors.First().ErrorMessage);
        }
	}
}