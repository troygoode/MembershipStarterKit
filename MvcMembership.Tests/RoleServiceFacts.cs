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
		private readonly IRolesService _roleWrapper;
		private readonly Mock<MembershipUser> _user = new Mock<MembershipUser>();
		private readonly string _username;
		private readonly Random _rnd;

		public RoleServiceFacts()
		{
			_rnd = new Random();
			_username = RandomString();
			_user.SetupGet(x => x.UserName).Returns(_username);

			_roleWrapper = new AspNetRoleProviderWrapper(_roleProvider.Object);

			var list = new List<string>();
			for (var i = 0; i < _rnd.Next(); i++)
				list.Add(_rnd.Next().ToString());
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
			_roleProvider.Setup(x => x.GetRolesForUser(_username)).Returns(_roles).Verifiable();

			//act
			var result = _roleWrapper.FindByUser(_user.Object);

			//assert
			Assert.Equal(_roles, result);
			_roleProvider.Verify();
		}

		[Fact]
		public void FindByUserName_returns_list_of_roles_by_username()
		{
			//arrange
			_roleProvider.Setup(x => x.GetRolesForUser(_username)).Returns(_roles).Verifiable();

			//act
			var result = _roleWrapper.FindByUserName(_username);

			//assert
			Assert.Equal(_roles, result);
			_roleProvider.Verify();
		}

		[Fact]
		public void FindUserNamesByRole_returns_list_of_usernames()
		{
			//arrange
			var roleName = RandomString();
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
			var roleName = RandomString();

			//act
			_roleWrapper.AddToRole(_user.Object, roleName);

			//assert
			_roleProvider.Verify(
				x =>
				x.AddUsersToRoles(It.Is<string[]>(v => v.Contains(_username) && v.Length == 1),
				                  It.Is<string[]>(v => v.Contains(roleName) && v.Length == 1)));
		}

		[Fact]
		public void RemoveFromRole_removes_user_from_role()
		{
			//arrange
			var roleName = RandomString();

			//act
			_roleWrapper.RemoveFromRole(_user.Object, roleName);

			//assert
			_roleProvider.Verify(
				x =>
				x.RemoveUsersFromRoles(It.Is<string[]>(v => v.Contains(_username) && v.Length == 1),
				                       It.Is<string[]>(v => v.Contains(roleName) && v.Length == 1)));
		}

		[Theory, InlineData(true), InlineData(false)]
		public void IsInRole_returns_whether_or_not_supplied_user_is_in_role(bool isInRole)
		{
			//arrange
			var role = RandomString();
			_roleProvider.Setup(x => x.IsUserInRole(_username, role)).Returns(isInRole).Verifiable();

			//act
			var result = _roleWrapper.IsInRole(_user.Object, role);

			//assert
			Assert.Equal(isInRole, result);
			_roleProvider.Verify();
		}

		[Fact]
		public void Create_creates_role()
		{
			//arrange
			var role = RandomString();

			//act
			_roleWrapper.Create(role);

			//assert
			_roleProvider.Verify(x => x.CreateRole(role));
		}

		[Fact]
		public void Delete_deletes_role()
		{
			//arrange
			var role = RandomString();

			//act
			_roleWrapper.Delete(role);

			//assert
			_roleProvider.Verify(x => x.DeleteRole(role, false));
		}

		private string RandomString()
		{
			return _rnd.Next().ToString();
		}
	}
}