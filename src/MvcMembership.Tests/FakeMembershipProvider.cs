using System.Web.Security;

namespace MvcMembership.Tests
{
	public abstract class FakeMembershipProvider : MembershipProvider
	{
		protected bool _enablePasswordReset;
		protected bool _enablePasswordRetrieval;
		protected int _maxInvalidPasswordAttempts;
		protected int _minRequiredNonAlphanumericCharacters;
		protected int _minRequiredPasswordLength;
		protected int _passwordAttemptWindow;
		protected MembershipPasswordFormat _passwordFormat;
		protected string _passwordStrengthRegularExpression;
		protected bool _requiresQuestionAndAnswer;
		protected bool _requiresUniqueEmail;
		public override bool EnablePasswordRetrieval { get { return _enablePasswordRetrieval; } }
		public override bool EnablePasswordReset { get { return _enablePasswordReset; } }
		public override bool RequiresQuestionAndAnswer { get { return _requiresQuestionAndAnswer; } }
		public override string ApplicationName { get; set; }
		public override int MaxInvalidPasswordAttempts { get { return _maxInvalidPasswordAttempts; } }
		public override int PasswordAttemptWindow { get { return _passwordAttemptWindow; } }
		public override bool RequiresUniqueEmail { get { return _requiresUniqueEmail; } }
		public override MembershipPasswordFormat PasswordFormat { get { return _passwordFormat; } }
		public override int MinRequiredPasswordLength { get { return _minRequiredPasswordLength; } }
		public override int MinRequiredNonAlphanumericCharacters { get { return _minRequiredNonAlphanumericCharacters; } }
		public override string PasswordStrengthRegularExpression { get { return _passwordStrengthRegularExpression; } }

		public override MembershipUser CreateUser( string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status )
		{
			throw new System.NotImplementedException();
		}

		public override bool ChangePasswordQuestionAndAnswer( string username, string password, string newPasswordQuestion, string newPasswordAnswer )
		{
			throw new System.NotImplementedException();
		}

		public override string GetPassword( string username, string answer )
		{
			throw new System.NotImplementedException();
		}

		public override bool ChangePassword( string username, string oldPassword, string newPassword )
		{
			throw new System.NotImplementedException();
		}

		public override string ResetPassword( string username, string answer )
		{
			throw new System.NotImplementedException();
		}

		public override void UpdateUser( MembershipUser user )
		{
			throw new System.NotImplementedException();
		}

		public override bool ValidateUser( string username, string password )
		{
			throw new System.NotImplementedException();
		}

		public override bool UnlockUser( string userName )
		{
			throw new System.NotImplementedException();
		}

		public override MembershipUser GetUser( object providerUserKey, bool userIsOnline )
		{
			throw new System.NotImplementedException();
		}

		public override MembershipUser GetUser( string username, bool userIsOnline )
		{
			throw new System.NotImplementedException();
		}

		public override string GetUserNameByEmail( string email )
		{
			throw new System.NotImplementedException();
		}

		public override bool DeleteUser( string username, bool deleteAllRelatedData )
		{
			throw new System.NotImplementedException();
		}

		public override MembershipUserCollection GetAllUsers( int pageIndex, int pageSize, out int totalRecords )
		{
			throw new System.NotImplementedException();
		}

		public override int GetNumberOfUsersOnline()
		{
			throw new System.NotImplementedException();
		}

		public override MembershipUserCollection FindUsersByName( string usernameToMatch, int pageIndex, int pageSize, out int totalRecords )
		{
			throw new System.NotImplementedException();
		}

		public override MembershipUserCollection FindUsersByEmail( string emailToMatch, int pageIndex, int pageSize, out int totalRecords )
		{
			throw new System.NotImplementedException();
		}
	}
}