namespace MvcMembership.Settings
{
	public interface ILoginSettings
	{
		int MaximumInvalidPasswordAttempts { get; }
		int PasswordAttemptWindowInMinutes { get; }
	}
}