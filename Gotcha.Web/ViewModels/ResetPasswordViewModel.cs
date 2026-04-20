using System.ComponentModel.DataAnnotations;

namespace Gotcha.Web.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        public required Guid UserId { get; set; }

        [Required]
        public required string Token { get; set; }

        [Required(ErrorMessage = "Please enter a new password!")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Please repeat your new password!")]
        [Compare("Password", ErrorMessage = "Passwords do not match!")]
        [DataType(DataType.Password)]
        [Display(Name = "Repeat Password")]
        public required string RepeatPassword { get; set; }
    }
}
