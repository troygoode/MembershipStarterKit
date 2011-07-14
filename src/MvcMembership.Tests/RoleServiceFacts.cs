using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace MvcMembership.Tests
{
	public class RoleServiceFacts
	{
		private readonly Mock<RoleProvider> _roleProvider = new Mock<RoleProvider>();
		private readonly string[] _roles;
		private readonly AspNetRoleProviderWrapper _roleWrapper;

		public RoleServiceFacts()
		{
			_roleWrapper = new AspNetRoleProviderWrapper(_roleProvider.Object);
			var rnd = new Random();
			var list = new List<string>();
			for (var i = 0; i < rnd.Next(); i++)
				list.Add(rnd.Next().ToString());
			_roles = list.ToArray();
		}

		[Fact]
		public void FindAll_returns_list_of_roles()
		{
			//arrange
			_roleProvider.Setup(x => x.GetAllRoles()).Returns(_roles).Verifiable();

			//act
			var result = _roleWrapper.FindAll();

			//assert
			Assert.Equal(_roles, result);
			_roleProvider.Verify();
		}

		[Fact]
		public void FindByUser_returns_list_of_roles_by_username()
		{
			//arrange
			var username = new Random().Next().ToString();
			var user = new Mock<MembershipUser>();
			user.SetupGet(x => x.UserName).Returns(username);
			_roleProvider.Setup(x => x.GetRolesForUser(username)).Returns(_roles).Verifiable();

			//act
			var result = _roleWrapper.FindByUser(user.Object);

			//assert
			Assert.Equal(_roles, result);
			_roleProvider.Verify();
		}

		[Fact]
		public void FindByUserName_returns_list_of_roles_by_username()
		{
			//arrange
			var username = new Random().Next().ToString();
			_roleProvider.Setup(x => x.GetRolesForUser(username)).Returns(_roles).Verifiable();

			//act
			var result = _roleWrapper.FindByUserName(username);

			//assert
			Assert.Equal(_roles, result);
			_roleProvider.Verify();
		}

		[Fact]
		public void FindUserNamesByRole_returns_list_of_usernames()
		{
			//arrange
			var roleName = new Random().Next().ToString();
			var users = new List<string>().ToArray();
			_roleProvider.Setup(x => x.GetUsersInRole(roleName)).Returns(users).Verifiable();

			//act
			var result = _roleWrapper.FindUserNamesByRole(roleName);

			//assert
			_roleProvider.Verify();
			Assert.Same(users, result);
		}

		[Fact]
		public void AddToRole_adds_user_to_role()
		{
			//arrange
			var roleName = new Random().Next().ToString();
			var username = new Random().Next().ToString();
			var user = new Mock<MembershipUser>();
			user.SetupGet(x => x.UserName).Returns(username);

			//act
			_roleWrapper.AddToRole(user.Object, roleName);

			//assert
			_roleProvider.Verify(
				x =>
				x.AddUsersToRoles(It.Is<string[]>(v => v.Contains(username) && v.Length == 1),
				                  It.Is<string[]>(v => v.Contains(roleName) && v.Length == 1)));
		}

		[Fact]
		public void RemoveFromRole_removes_user_from_role()
		{
			//arrange
			var roleName = new Random().Next().ToString();
			var username = new Random().Next().ToString();
			var user = new Mock<MembershipUser>();
			user.SetupGet(x => x.UserName).Returns(username);

			//act
			_roleWrapper.RemoveFromRole(user.Object, roleName);

			//assert
			_roleProvider.Verify(
				x =>
				x.RemoveUsersFromRoles(It.Is<string[]>(v => v.Contains(username) && v.Length == 1),
				                       It.Is<string[]>(v => v.Contains(roleName) && v.Length == 1)));
		}

		[Theory, InlineData(true), InlineData(false)]
		public void IsInRole_returns_whether_or_not_supplied_user_is_in_role(bool isInRole)
		{
			//arrange
			var username = new Random().Next().ToString();
			var user = new Mock<MembershipUser>();
			user.SetupGet(x => x.UserName).Returns(username);
			var role = new Random().Next().ToString();
			_roleProvider.Setup(x => x.IsUserInRole(username, role)).Returns(isInRole).Verifiable();

			//act
			var result = _roleWrapper.IsInRole(user.Object, role);

			//assert
			Assert.Equal(isInRole, result);
			_roleProvider.Verify();
		}

		[Fact]
		public void Create_creates_role()
		{
			//arrange
			var role = new Random().Next().ToString();

			//act
			_roleWrapper.Create(role);

			//assert
			_roleProvider.Verify(x => x.CreateRole(role));
		}

		[Fact]
		public void Delete_deletes_role()
		{
			//arrange
			var role = new Random().Next().ToString();

			//act
			_roleWrapper.Delete(role);

			//assert
			_roleProvider.Verify(x => x.DeleteRole(role, false));
		}
	}
}