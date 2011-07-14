namespace MvcMembership.Settings
{
	public interface IMembershipSettings
	{
		IRegistrationSettings Registration{ get; }
		IPasswordSettings Password{ get; }
		ILoginSettings Login{ get; }
	}
}