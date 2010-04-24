using System.Web.Security;

namespace MvcMembership.Settings
{
	public class AspNetMembershipProviderSettingsWrapper : IMembershipSettings
	{
		public AspNetMembershipProviderSettingsWrapper(MembershipProvider provider)
			: this(
				new RegistrationSettings(provider.RequiresUniqueEmail),
				new PasswordSettings(
					new PasswordResetRetrievalSettings(provider.EnablePasswordReset,
					                                   provider.EnablePasswordRetrieval,
					                                   provider.RequiresQuestionAndAnswer),
					provider.MinRequiredPasswordLength,
					provider.MinRequiredNonAlphanumericCharacters,
					provider.PasswordStrengthRegularExpression,
					provider.PasswordFormat),
				new LoginSettings(provider.MaxInvalidPasswordAttempts,
				                  provider.PasswordAttemptWindow)
				)
		{
		}

		public AspNetMembershipProviderSettingsWrapper(IRegistrationSettings registration, IPasswordSettings password, ILoginSettings login)
		{
			Registration = registration;
			Password = password;
			Login = login;
		}

		#region IMembershipSettings Members

		public IRegistrationSettings Registration { get; private set; }
		public IPasswordSettings Password { get; private set; }
		public ILoginSettings Login { get; private set; }

		#endregion
	}
}