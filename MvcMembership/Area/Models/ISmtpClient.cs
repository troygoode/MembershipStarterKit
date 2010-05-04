using System.Net.Mail;

namespace MvcMembership.Area.Models
{
	public interface ISmtpClient
	{
		void Send(MailMessage mailMessage);
	}
}