using System.Net.Mail;

namespace MvcMembership
{
	public interface ISmtpClient
	{
		void Send(MailMessage mailMessage);
	}
}