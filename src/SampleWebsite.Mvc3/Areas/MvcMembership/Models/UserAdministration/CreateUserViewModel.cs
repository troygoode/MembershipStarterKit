using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace SampleWebsite.Mvc3.Areas.MvcMembership.Models.UserAdministration
{
   public class CreateUserViewModel
   {
      [Display(Name = "Username" , ResourceType= typeof(Resources.Resources))]
      [Required(ErrorMessageResourceName = "YouMustSpecifyUsername", ErrorMessageResourceType = typeof(Resources.Resources))]
      public string Username { get; set; }

      [Display(Name = "Password", ResourceType = typeof(Resources.Resources))]
      [Required(ErrorMessageResourceName = "YouMustSpecifyPassword", ErrorMessageResourceType = typeof(Resources.Resources)), DataType(DataType.Password)]
      public string Password { get; set; }

      [Display(Name = "PasswordAgain", ResourceType=typeof(Resources.Resources))]
      [Required(ErrorMessageResourceName = "YouMustConfirmPassword", ErrorMessageResourceType = typeof(Resources.Resources)), DataType(DataType.Password)]
      public string ConfirmPassword { get; set; }

      [Display(Name = "EmailAddress", ResourceType=typeof(Resources.Resources))]
      [Required(ErrorMessageResourceName = "YouMustSpecifiyEmailAddress", ErrorMessageResourceType = typeof(Resources.Resources)), Email]
      public string Email { get; set; }

      [Display(Name = "SecretQuestion", ResourceType=typeof(Resources.Resources))]
      public string PasswordQuestion { get; set; }

      [StringLength(100)]
      [Display(Name = "SecretAnswer", ResourceType=typeof(Resources.Resources))]
      public string PasswordAnswer { get; set; }

      [Display(Name = "InitialRoles", ResourceType=typeof(Resources.Resources))]
      public IDictionary<string, bool> InitialRoles { get; set; }
   }
}