namespace MvcMembership.Settings
{
	public class LoginSettings : ILoginSettings
	{
		public LoginSettings(int maximumInvalidPasswordAttempts, int passwordAttemptWindowInMinutes)
		{
			MaximumInvalidPasswordAttempts = maximumInvalidPasswordAttempts;
			PasswordAttemptWindowInMinutes = passwordAttemptWindowInMinutes;
		}

		#region ILoginSettings Members

		public int MaximumInvalidPasswordAttempts { get; private set; }
		public int PasswordAttemptWindowInMinutes { get; private set; }

		#endregion
	}
}