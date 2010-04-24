namespace MvcMembership.Settings
{
	public interface IPasswordResetRetrievalSettings
	{
		bool CanReset { get; }
		bool CanRetrieve { get; }
		bool RequiresQuestionAndAnswer { get; }
	}
}