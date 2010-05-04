using System.Web.Security;

namespace MvcMembership.Settings
{
	public interface IPasswordSettings
	{
		IPasswordResetRetrievalSettings ResetOrRetrieval{ get; }
		int MinimumLength { get; }
		int MinimumNonAlphanumericCharacters { get; }
		string RegularExpressionToMatch { get; }
		MembershipPasswordFormat StorageFormat { get; }
	}
}