using System.ComponentModel.DataAnnotations;

namespace Gotcha.Web.ViewModels
{
    public class HomeViewModel
    {
        [Required(ErrorMessage = "Please enter your email address!")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address!")]
        [Display(Name = "Email")]
        public required string Email { get; set; }

        // not in a RegisterBaseViewModel because 'Please enter a password!' vs 'Please enter your password!'
        [Required(ErrorMessage = "Please enter a password!")]
        [DataType(DataType.Password, ErrorMessage = "Please enter a valid password!")]
        [Display(Name = "Password")]
        public required string Password { get; set; }

        [Display(Name = "Remember me")]
        public required bool RememberMe { get; set; }
    }
}
