using System.ComponentModel.DataAnnotations;

namespace Gotcha.Web.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Please enter your email address!")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address!")]
        [Display(Name = "Email")]
        public required string Email { get; set; }
    }
}
