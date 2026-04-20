using System.ComponentModel.DataAnnotations;
using Gotcha.Core.Enums;
using Gotcha.Core.Validation.DataAnnotations;

namespace Gotcha.Web.ViewModels
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "Please enter your first name!")]
        [Name(ErrorMessage = "Please enter a valid first name!")]
        [Display(Name = "First Name")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name!")]
        [Name(ErrorMessage = "Please enter a valid last name!")]
        [Display(Name = "Last Name")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your username!")]
        [UserName(ErrorMessage = "Please enter a valid username!")]
        [Display(Name = "Username")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Please enter your email!")]
        [EmailAddress(ErrorMessage = "Please enter a valid email!")]
        [Display(Name = "Email")]
        public required string Email { get; set; }

        // not in a RegisterBaseViewModel because 'Please enter a password!' vs 'Please enter your password!'
        [Required(ErrorMessage = "Please enter a password!")]
        [DataType(DataType.Password, ErrorMessage = "Please enter a valid password!")]
        [Display(Name = "Password")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Please repeat your password!")]
        [Compare("Password", ErrorMessage = "Passwords do not match!")]
        [DataType(DataType.Password, ErrorMessage = "Please enter a valid password!")]
        [Display(Name = "Repeat Password")]
        public required string RepeatPassword { get; set; }

        [Required(ErrorMessage = "Please enter your birthday!")]
        [BirthDay(ErrorMessage = "Please enter a valid birthday!")]
        [Display(Name = "Birthday")]
        public DateTime? BirthDay { get; set; }

        [Required(ErrorMessage = "Please enter your gender!")]
        //[Gender(ErrorMessage = "Please enter a valid gender!")]
        [Display(Name = "Gender")]
        public Genders? Gender { get; set; }

        [Display(Name = "Profile Image")]
        public IFormFile? ProfileImage { get; set; }

        [GuardianRequired("BirthDay", ErrorMessage = "Please enter a guardian email address!")] // if age (years) < 16, guardian email is required (COPPA/GDPR)
        [EmailAddress(ErrorMessage = "Please enter a valid email address!")]
        [Display(Name = "Parent/Guardian Email")]
        public string? GuardianEmail { get; set; }
    }
}
