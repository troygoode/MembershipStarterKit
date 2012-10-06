using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace SampleWebsite.Mvc3.Areas.MvcMembership.Models.UserAdministration
{
	public class CreateUserViewModel
	{
		[Display(Name = "User Name")]
		[Required]
		public string Username { get; set; }

		[Required, DataType(DataType.Password)]
		public string Password { get; set; }

		[Display(Name = "Password (Again...)")]
		[Required, DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }

		[Display(Name = "Email Address")]
		[Required, Email]
		public string Email { get; set; }

		[Display(Name = "Secret Question")]
		public string PasswordQuestion { get; set; }

		[StringLength(100)]
		[Display(Name = "Secret Answer")]
		public string PasswordAnswer { get; set; }

		[Display(Name = "Initial Roles")]
		public IDictionary<string, bool> InitialRoles { get; set; }
	}
}