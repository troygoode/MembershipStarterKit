using System.Net.Mail;

namespace SampleWebsite.Models
{
	public interface ISmtpClient
	{
		void Send(MailMessage mailMessage);
	}
}