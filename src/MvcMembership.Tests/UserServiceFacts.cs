using System;
using System.Linq;
using System.Web.Security;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace MvcMembership.Tests
{
	public class UserServiceFacts
	{
		private readonly Mock<MembershipProvider> _membershipProvider = new Mock<MembershipProvider>();
		private readonly AspNetMembershipProviderWrapper _membershipWrapper;
		private readonly Mock<MembershipUser> _user = new Mock<MembershipUser>();

		public UserServiceFacts()
		{
			_membershipWrapper = new AspNetMembershipProviderWrapper(_membershipProvider.Object);
		}

		[Fact]
		public void FindAll_passes_paging_info_to_provider_and_converts_returned_collection_to_PagedListOfMembershipUser()
		{
			//arrange
			var users = new[]
			            	{
			            		new MembershipUser("AspNetSqlMembershipProvider", "TEST1", "", "", "", "", true, false, DateTime.Now,
			            		                   DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now),
			            		new MembershipUser("AspNetSqlMembershipProvider", "TEST2", "", "", "", "", true, false, DateTime.Now,
			            		                   DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
			            	};
			var usercollection = new MembershipUserCollection();
			var membership = new FakeMembershipProvider_FindAll
			                 	{
			                 		ReturnedUsers = usercollection,
			                 		TotalRecords = 123,
			                 		PageIndex = -1,
			                 		PageSize = -1
			                 	};
			var service = new AspNetMembershipProviderWrapper(membership);
			const int pageNumber = 3;
			const int size = 10;

			//act
            var result = service.FindAll(pageNumber, size);

			//assert
            Assert.Equal(pageNumber - 1, membership.PageIndex);
			Assert.Equal(size, membership.PageSize);
			Assert.Equal(usercollection.Count, result.Count());
			foreach (var user in result)
				Assert.Contains(user, users);
		}

		[Fact]
		public void
			FindByUserName_passes_partial_username_to_provider_and_converts_returned_collection_to_PagedListOfMembershipUser()
		{
			//arrange
			var users = new[]
			            	{
			            		new MembershipUser("AspNetSqlMembershipProvider", "TEST1", "", "", "", "", true, false, DateTime.Now,
			            		                   DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now),
			            		new MembershipUser("AspNetSqlMembershipProvider", "TEST2", "", "", "", "", true, false, DateTime.Now,
			            		                   DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
			            	};
			var usercollection = new MembershipUserCollection();
			var membership = new FakeMembershipProvider_FindByUserName
			                 	{
			                 		ReturnedUsers = usercollection,
			                 		TotalRecords = 123,
			                 		PageIndex = -1,
			                 		PageSize = -1
			                 	};
			var service = new AspNetMembershipProviderWrapper(membership);
			const int pageNumber = 3;
			const int size = 10;
			var username = new Random().Next().ToString();

			//act
            var result = service.FindByUserName(username, pageNumber, size);

			//assert
            Assert.Equal(pageNumber - 1, membership.PageIndex);
			Assert.Equal(size, membership.PageSize);
			Assert.Equal(usercollection.Count, result.Count());
			foreach (var user in result)
				Assert.Contains(user, users);
		}

		[Fact]
		public void
			FindByEmail_passes_partial_email_address_to_provider_and_converts_returned_collection_to_PagedListOfMembershipUser()
		{
			//arrange
			var users = new[]
			            	{
			            		new MembershipUser("AspNetSqlMembershipProvider", "TEST1", "", "", "", "", true, false, DateTime.Now,
			            		                   DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now),
			            		new MembershipUser("AspNetSqlMembershipProvider", "TEST2", "", "", "", "", true, false, DateTime.Now,
			            		                   DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
			            	};
			var usercollection = new MembershipUserCollection();
			var membership = new FakeMembershipProvider_FindByEmail
			                 	{
			                 		ReturnedUsers = usercollection,
			                 		TotalRecords = 123,
			                 		PageIndex = -1,
			                 		PageSize = -1
			                 	};
			var service = new AspNetMembershipProviderWrapper(membership);
            const int pageNumber = 3;
			const int size = 10;
			var emailAddress = new Random().Next().ToString();

			//act
            var result = service.FindByEmail(emailAddress, pageNumber, size);

			//assert
            Assert.Equal(pageNumber - 1, membership.PageIndex);
			Assert.Equal(size, membership.PageSize);
			Assert.Equal(usercollection.Count, result.Count());
			foreach (var user in result)
				Assert.Contains(user, users);
		}

		[Fact]
		public void Get_by_ProviderUserKey_passes_key_to_provider_and_doesnt_update_timestamp_and_returns_user()
		{
			//arrange
			var puk = Guid.NewGuid();
			_membershipProvider.Setup(x => x.GetUser(puk, false)).Returns(_user.Object).Verifiable();

			//act
			var result = _membershipWrapper.Get(puk);

			//assert
			Assert.Same(_user.Object, result);
			_membershipProvider.Verify();
		}

		[Fact]
		public void Get_by_UserName_passes_username_to_provider_and_doesnt_update_timestamp_and_returns_user()
		{
			//arrange
			var userName = new Random().Next().ToString();
			_membershipProvider.Setup(x => x.GetUser(userName, false)).Returns(_user.Object).Verifiable();

			//act
			var result = _membershipWrapper.Get(userName);

			//assert
			Assert.Same(_user.Object, result);
			_membershipProvider.Verify();
		}

		[Fact]
		public void Touch_by_user_returns_user_by_username_and_updates_last_active_timestamp()
		{
			//arrange
			var userName = new Random().Next().ToString();
			_user.SetupGet(x => x.UserName).Returns(userName);
			_membershipProvider.Setup(x=> x.GetUser(userName, true)).Returns(_user.Object).Verifiable();

			//act
			var result = _membershipWrapper.Touch(_user.Object);

			//assert
			Assert.Same(_user.Object, result);
			_membershipProvider.Verify();
		}

		[Fact]
		public void Touch_by_username_returns_user_by_username_and_updates_last_active_timestamp()
		{
			//arrange
			var userName = new Random().Next().ToString();
			_membershipProvider.Setup(x => x.GetUser(userName, true)).Returns(_user.Object).Verifiable();

			//act
			var result = _membershipWrapper.Touch(userName);

			//assert
			Assert.Same(_user.Object, result);
			_membershipProvider.Verify();
		}

		[Fact]
		public void Touch_by_providerUserKey_returns_user_by_providerUserKey_and_updates_last_active_timestamp()
		{
			//arrange
			var puk = Guid.NewGuid();
			_membershipProvider.Setup(x => x.GetUser(puk, true)).Returns(_user.Object).Verifiable();

			//act
			var result = _membershipWrapper.Touch(puk);

			//assert
			Assert.Same(_user.Object, result);
			_membershipProvider.Verify();
		}

		[Fact]
		public void Update_passes_user_to_provider_for_updating()
		{
			//act
			_membershipWrapper.Update(_user.Object);

			//assert
			_membershipProvider.Verify(x => x.UpdateUser(_user.Object));
		}

		[Fact]
		public void Delete_passes_username_to_Delete_method()
		{
			//arrange
			var username = new Random().Next().ToString();
			_user.SetupGet(x => x.UserName).Returns(username);

			//act
			_membershipWrapper.Delete(_user.Object);

			//assert
			_membershipProvider.Verify(x => x.DeleteUser(username, false));
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void Delete_passes_username_and_supplied_flag_to_Delete_method(bool deleteAllUserData)
		{
			//arrange
			var username = new Random().Next().ToString();
			_user.SetupGet(x => x.UserName).Returns(username);

			//act
			_membershipWrapper.Delete(_user.Object, deleteAllUserData);

			//assert
			_membershipProvider.Verify(x => x.DeleteUser(username, deleteAllUserData));
		}

		[Fact]
		public void TotalUsers_retrieves_single_user_and_returns_total_user_count()
		{
			//arrange
			const int totalRecords = 123;
			var membership = new FakeMembershipProvider_FindAll
			                 	{
			                 		TotalRecords = totalRecords,
			                 		PageIndex = -1,
			                 		PageSize = -1
			                 	};
			var service = new AspNetMembershipProviderWrapper(membership);

			//act
			var result = service.TotalUsers;

			//assert
			Assert.Equal(1, membership.PageIndex);
			Assert.Equal(1, membership.PageSize);
			Assert.Equal(totalRecords, result);
		}

		[Fact]
		public void UsersOnline_returns_NumberOfUsersOnline_from_provder()
		{
			//arrange
			var usersOnline = new Random().Next();
			_membershipProvider.Setup(x => x.GetNumberOfUsersOnline()).Returns(usersOnline).Verifiable();

			//act
			var result = _membershipWrapper.UsersOnline;

			//assert
			_membershipProvider.Verify();
			Assert.Equal(usersOnline, result);
		}

		#region Nested type: FakeMembershipProvider_FindAll

		private class FakeMembershipProvider_FindAll : FakeMembershipProvider
		{
			public MembershipUserCollection ReturnedUsers { get; set; }
			public int TotalRecords { get; set; }
			public int PageIndex { get; set; }
			public int PageSize { get; set; }

			public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
			{
				PageIndex = pageIndex;
				PageSize = pageSize;
				totalRecords = TotalRecords;
				return ReturnedUsers;
			}
		}

		#endregion

		#region Nested type: FakeMembershipProvider_FindByEmail

		private class FakeMembershipProvider_FindByEmail : FakeMembershipProvider
		{
			public MembershipUserCollection ReturnedUsers { get; set; }
			public int TotalRecords { get; set; }
			public int PageIndex { get; set; }
			public int PageSize { get; set; }

			public override MembershipUserCollection FindUsersByEmail(string emailAddressToMatch, int pageIndex, int pageSize,
			                                                          out int totalRecords)
			{
				PageIndex = pageIndex;
				PageSize = pageSize;
				totalRecords = TotalRecords;
				return ReturnedUsers;
			}
		}

		#endregion

		#region Nested type: FakeMembershipProvider_FindByUserName

		private class FakeMembershipProvider_FindByUserName : FakeMembershipProvider
		{
			public MembershipUserCollection ReturnedUsers { get; set; }
			public int TotalRecords { get; set; }
			public int PageIndex { get; set; }
			public int PageSize { get; set; }

			public override MembershipUserCollection FindUsersByName(string userNameToMatch, int pageIndex, int pageSize,
			                                                         out int totalRecords)
			{
				PageIndex = pageIndex;
				PageSize = pageSize;
				totalRecords = TotalRecords;
				return ReturnedUsers;
			}
		}

		#endregion
	}
}