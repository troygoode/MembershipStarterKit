namespace MvcMembership
{
	public class AspNetMembershipProviderUserServiceFactory : IUserServiceFactory
	{
		#region IUserServiceFactory Members

		public IUserService Make()
		{
			return new AspNetMembershipProviderWrapper();
		}

		#endregion
	}
}