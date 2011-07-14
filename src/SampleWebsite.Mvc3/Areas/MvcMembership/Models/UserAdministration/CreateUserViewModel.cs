using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace SampleWebsite.Mvc3.Areas.MvcMembership.Models.UserAdministration
{
	public class CreateUserViewModel
	{
		[Required]
		public string Username { get; set; }

		[Required, DataType(DataType.Password)]
		public string Password { get; set; }

		[Required, Email]
		public string Email { get; set; }

		[Display(Name = "Secret Question")]
		public string PasswordQuestion { get; set; }

		[StringLength(100)]
		[Display(Name = "Secret Answer")]
		public string PasswordAnswer { get; set; }
	}
}