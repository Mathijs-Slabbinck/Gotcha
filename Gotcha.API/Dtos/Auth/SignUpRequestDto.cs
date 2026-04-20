using System.ComponentModel.DataAnnotations;

namespace Gotcha.API.Dtos.Auth
{
    public class SignUpRequestDto
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 1)]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, MinimumLength = 1)]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, MinimumLength = 3)]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email is not a valid format.")]
        [StringLength(254)]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(128, MinimumLength = 12, ErrorMessage = "Password must be at least 12 characters.")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [StringLength(20)]
        public required string Gender { get; set; }

        [Required(ErrorMessage = "Birth date is required.")]
        public DateTime BirthDate { get; set; }

        [EmailAddress(ErrorMessage = "Guardian email is not a valid format.")]
        [StringLength(254)]
        public string? GuardianEmail { get; set; }
    }
}
