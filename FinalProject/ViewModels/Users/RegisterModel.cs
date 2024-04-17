using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels.Users
{
    public class RegisterModel
    {
        [Required]
        public string FirstName { set; get; }
        [Required]
        public string LastName { set; get; }

        [Required]
        [EmailAddress]
        public string Email { set; get; }

        [Required]
        public string Password { set; get; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { set; get; }
        [Required]
        public string PhoneNumber { set; get; }
        [Required]
        public string Role { set; get; }
    }
}
