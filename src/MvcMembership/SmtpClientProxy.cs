using System.Net.Mail;

namespace MvcMembership
{
	public class SmtpClientProxy : ISmtpClient
	{
		private readonly SmtpClient _smtpClient;

		public SmtpClientProxy(SmtpClient smtpClient)
		{
			_smtpClient = smtpClient;
		}

		#region ISmtpClient Members

		public void Send(MailMessage mailMessage)
		{
			_smtpClient.Send(mailMessage);
		}

		#endregion
	}
}