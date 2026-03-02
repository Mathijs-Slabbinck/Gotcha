using System.ComponentModel.DataAnnotations;
using Gotcha.Core.Validation.DataAnnotations;

namespace Gotcha.Web.ViewModels
{
    public class HomeViewModel
    {
        [Required(ErrorMessage = "Please enter your username or email!")]
        [UserNameOrEmail(ErrorMessage = "Please enter a valid username or email!")]
        [Display(Name = "Username / Email")]
        public required string UserNameOrEmail { get; set; }

        // not in a RegisterBaseViewModel because 'Please enter a password!' vs 'Please enter your password!'
        [Required(ErrorMessage = "Please enter a password!")]
        [DataType(DataType.Password, ErrorMessage = "Please enter a valid password!")]
        [Display(Name = "Password")]
        public required string Password { get; set; }
    }
}
