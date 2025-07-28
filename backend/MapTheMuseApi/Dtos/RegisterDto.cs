using System.ComponentModel.DataAnnotations;
using Azure.Identity;

namespace MapTheMuseApi.Dtos
{
    public class RegisterDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(6)]
        public string Password { get; set; }
        [Required, Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string PreferredLanguage { get; set; }
    }
}