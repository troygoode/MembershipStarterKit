using System;
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
			user.UnlockUser();
		}

		public string ResetPassword(MembershipUser user)
		{
			return user.ResetPassword();
		}

		public string ResetPassword(MembershipUser user, string passwordAnswer)
		{
			return user.ResetPassword(passwordAnswer);
		}

		public void ChangePassword(MembershipUser user, string newPassword)
		{
			var resetPassword = user.ResetPassword();
			if(!user.ChangePassword(resetPassword, newPassword))
				throw new MembershipPasswordException("Could not change password.");
		}

		public void ChangePassword(MembershipUser user, string oldPassword, string newPassword)
		{
			if (!user.ChangePassword(oldPassword, newPassword))
				throw new MembershipPasswordException("Could not change password.");
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

        public MembershipUser Create(string username, string password, string email, string passwordQuestion, string passwordAnswer)
        {
            var providerUserKey = Guid.NewGuid();
            const bool isApproved = true;
            MembershipCreateStatus status;
            var user = _membershipProvider.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, out status);
            if(status != MembershipCreateStatus.Success) 
            {
                throw new MembershipCreateUserException(status);
            }
            return user;
        }

	    public void Update(MembershipUser user)
		{
			_membershipProvider.UpdateUser(user);
		}

		public void Delete(MembershipUser user)
		{
			_membershipProvider.DeleteUser(user.UserName, false);
		}

		public void Delete(MembershipUser user, bool deleteAllRelatedData)
		{
			_membershipProvider.DeleteUser(user.UserName, deleteAllRelatedData);
		}

		public MembershipUser Touch(MembershipUser user)
		{
			return _membershipProvider.GetUser(user.UserName, true);
		}

		public MembershipUser Touch(string userName)
		{
			return _membershipProvider.GetUser(userName, true);
		}

		public MembershipUser Touch(object providerUserKey)
		{
			return _membershipProvider.GetUser(providerUserKey, true);
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