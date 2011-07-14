using System;
using System.Collections.Generic;
using System.Web.Security;
using Xunit;

namespace MvcMembership.Tests
{
	public class EnumerableToEnumerableTConverterFacts
	{
		[Fact]
		public void CanConvertTo_is_TRUE_for_converting_MembershipUserCollection_to_IEnumerableMembershipUser()
		{
			//arrange
			var converter = new EnumerableToEnumerableTConverter<MembershipUserCollection, MembershipUser>();

			//act
			var result = converter.CanConvertTo( typeof(IEnumerable<MembershipUser>) );

			//assert
			Assert.True( result );
		}

		[Fact]
		public void ConvertTo_returns_IEnumerableMembershipUser_with_all_MembershipUser_items_from_passed_MembershipUserCollection()
		{
			//arrange
			var converter = new EnumerableToEnumerableTConverter<MembershipUserCollection, MembershipUser>();
			var user1 = new MembershipUser( "AspNetSqlMembershipProvider", "User1", null, "user@domain.com", "Question", "Comment", true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now );
			var user2 = new MembershipUser( "AspNetSqlMembershipProvider", "User2", null, "user@domain.com", "Question", "Comment", true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now );

			//act
			var result = converter.ConvertTo(new MembershipUserCollection { user1, user2 }, typeof(IEnumerable<MembershipUser>));

			//assert
			var users = Assert.IsAssignableFrom<IEnumerable<MembershipUser>>( result );
			Assert.Contains( user1, users );
			Assert.Contains( user2, users );
		}
	}
}