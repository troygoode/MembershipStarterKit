using System;
using System.Collections.Generic;
using System.Web.Security;

namespace MvcMembership
{
	public class AspNetRoleProviderWrapper : IRolesService
	{
		private readonly RoleProvider _roleProvider;

		public AspNetRoleProviderWrapper()
		{
			if (Roles.Enabled)
				_roleProvider = Roles.Provider;
		}

		public AspNetRoleProviderWrapper(RoleProvider roleProvider)
		{
			_roleProvider = roleProvider;
		}

		#region IRolesService Members

		public bool Enabled
		{
			get { return _roleProvider != null; }
		}

		public IEnumerable<string> FindAll()
		{
			return _roleProvider.GetAllRoles();
		}

		public IEnumerable<string> FindByUser(MembershipUser user)
		{
			return _roleProvider.GetRolesForUser(user.UserName);
		}

		public IEnumerable<string> FindByUserName(string userName)
		{
			return _roleProvider.GetRolesForUser(userName);
		}

		public IEnumerable<string> FindUserNamesByRole(string roleName)
		{
			return _roleProvider.GetUsersInRole(roleName);
		}

		public void AddToRole(MembershipUser user, string roleName)
		{
			_roleProvider.AddUsersToRoles(new[] {user.UserName}, new[] {roleName});
		}

		public void RemoveFromRole(MembershipUser user, string roleName)
		{
			_roleProvider.RemoveUsersFromRoles(new[] {user.UserName}, new[] {roleName});
		}

		public void RemoveFromAllRoles(MembershipUser user)
		{
			var roles = FindByUser(user);
			foreach(var role in roles)
				RemoveFromRole(user, role);
		}

		public bool IsInRole(MembershipUser user, string roleName)
		{
			return _roleProvider.IsUserInRole(user.UserName, roleName);
		}

		public void Create(string roleName)
		{
			_roleProvider.CreateRole(roleName);
		}

		public void Delete(string roleName)
		{
			_roleProvider.DeleteRole(roleName, false);
		}

		#endregion
	}
}