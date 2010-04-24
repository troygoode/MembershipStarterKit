namespace MvcMembership.Settings
{
	public interface IRegistrationSettings
	{
		bool RequiresUniqueEmailAddress{ get; }
	}
}