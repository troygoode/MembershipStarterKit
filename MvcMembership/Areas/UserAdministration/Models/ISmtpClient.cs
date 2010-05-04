using System.Net.Mail;

namespace MvcMembership.Areas.UserAdministration.Models
{
	public interface ISmtpClient
	{
		void Send(MailMessage mailMessage);
	}
}