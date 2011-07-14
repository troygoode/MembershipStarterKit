namespace MvcMembership.Settings
{
	public class RegistrationSettings : IRegistrationSettings
	{
		public RegistrationSettings(bool requiresUniqueEmailAddress)
		{
			RequiresUniqueEmailAddress = requiresUniqueEmailAddress;
		}

		#region IRegistrationSettings Members

		public bool RequiresUniqueEmailAddress { get; private set; }

		#endregion
	}
}