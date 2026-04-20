using System.ComponentModel.DataAnnotations;

namespace Gotcha.API.Dtos.Auth
{
    public class SignInRequestDto
    {
        [Required(ErrorMessage = "Username or email is required.")]
        [StringLength(254, MinimumLength = 1)]
        public required string UsernameOrEmail { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(128, MinimumLength = 1)]
        public required string Password { get; set; }
    }
}
