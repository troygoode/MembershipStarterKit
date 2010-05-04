using System.Collections.Generic;
using System.Web.Security;
using PagedList;

namespace MvcMembership
{
	public class AspNetMembershipProviderWrapper : IUserService, IPasswordService
	{
		private readonly MembershipProvider _membershipProvider;

		public AspNetMembershipProviderWrapper(MembershipProvider membershipProvider)
		{
			_membershipProvider = membershipProvider;
		}

		#region IPasswordService Members

		public void Unlock(MembershipUser user)
		{
			_membershipProvider.UnlockUser(user.UserName);
		}

		public void ChangePassword(MembershipUser user, string oldPassword, string newPassword)
		{
			_membershipProvider.ChangePassword(user.UserName, oldPassword, newPassword);
		}

		public void ChangePasswordQuestionAndAnswer(MembershipUser user, string password, string question, string answer)
		{
			_membershipProvider.ChangePasswordQuestionAndAnswer(user.UserName, password, question, answer);
		}

		public string GetPassword(MembershipUser user, string passwordAnswer)
		{
			return _membershipProvider.GetPassword(user.UserName, passwordAnswer);
		}

		public string GetPassword(MembershipUser user)
		{
			return user.GetPassword();
		}

		public string ResetPassword(MembershipUser user, string passwordAnswer)
		{
			return _membershipProvider.ResetPassword(user.UserName, passwordAnswer);
		}

		public string ResetPassword(MembershipUser user)
		{
			return user.ResetPassword();
		}

		#endregion

		#region IUserService Members

		public IPagedList<MembershipUser> FindAll(int pageIndex, int pageSize)
		{
			// get one page of users
			int totalUserCount;
			var usersCollection = _membershipProvider.GetAllUsers(pageIndex, pageSize, out totalUserCount);

			// convert from MembershipUserColletion to PagedList<MembershipUser> and return
			var converter = new EnumerableToEnumerableTConverter<MembershipUserCollection, MembershipUser>();
			var usersList = converter.ConvertTo<IEnumerable<MembershipUser>>(usersCollection);
			var usersPagedList = new StaticPagedList<MembershipUser>(usersList, pageIndex, pageSize, totalUserCount);
			return usersPagedList;
		}

		public IPagedList<MembershipUser> FindByEmail(string emailAddressToMatch, int pageIndex, int pageSize)
		{
			// get one page of users
			int totalUserCount;
			var usersCollection = _membershipProvider.FindUsersByEmail(emailAddressToMatch, pageIndex, pageSize, out totalUserCount);

			// convert from MembershipUserColletion to PagedList<MembershipUser> and return
			var converter = new EnumerableToEnumerableTConverter<MembershipUserCollection, MembershipUser>();
			var usersList = converter.ConvertTo<IEnumerable<MembershipUser>>(usersCollection);
			var usersPagedList = new StaticPagedList<MembershipUser>(usersList, pageIndex, pageSize, totalUserCount);
			return usersPagedList;
		}

		public IPagedList<MembershipUser> FindByUserName(string userNameToMatch, int pageIndex, int pageSize)
		{
			// get one page of users
			int totalUserCount;
			var usersCollection = _membershipProvider.FindUsersByName(userNameToMatch, pageIndex, pageSize, out totalUserCount);

			// convert from MembershipUserColletion to PagedList<MembershipUser> and return
			var converter = new EnumerableToEnumerableTConverter<MembershipUserCollection, MembershipUser>();
			var usersList = converter.ConvertTo<IEnumerable<MembershipUser>>(usersCollection);
			var usersPagedList = new StaticPagedList<MembershipUser>(usersList, pageIndex, pageSize, totalUserCount);
			return usersPagedList;
		}

		public MembershipUser Get(string userName)
		{
			return _membershipProvider.GetUser(userName, false);
		}

		public MembershipUser Get(object providerUserKey)
		{
			return _membershipProvider.GetUser(providerUserKey, false);
		}

		public void Update(MembershipUser user)
		{
			_membershipProvider.UpdateUser(user);
		}

		public void Delete(MembershipUser user)
		{
			_membershipProvider.DeleteUser(user.UserName, false);
		}

		public void Touch(MembershipUser user)
		{
			_membershipProvider.GetUser(user.UserName, true);
		}

		public void Touch(string userName)
		{
			_membershipProvider.GetUser(userName, true);
		}

		public void Touch(object providerUserKey)
		{
			_membershipProvider.GetUser(providerUserKey, true);
		}

		public int TotalUsers
		{
			get
			{
				int totalUsers;
				_membershipProvider.GetAllUsers(1, 1, out totalUsers);
				return totalUsers;
			}
		}

		public int UsersOnline
		{
			get
			{
				return _membershipProvider.GetNumberOfUsersOnline();
			}
		}

		#endregion
	}
}