using System.ComponentModel.DataAnnotations;
using Microsoft.Web.Mvc;

namespace UserAdministration.Models 
{
    public class CreateUserViewModel 
    {
        [Required]
        public string Username { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Display(Name="Secrect Question")]
        public string PasswordQuestion { get; set; }
        [StringLength(100)]
        [Display(Name="Secrect Answer")]
        public string PasswordAnswer { get; set; }
    }
}