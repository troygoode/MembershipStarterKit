using System.Net.Mail;

namespace MvcMembershipImport.Models
{
	public interface ISmtpClient
	{
		void Send(MailMessage mailMessage);
	}
}