using System.ComponentModel.DataAnnotations;
using Gotcha.Core.Services.ValidationServices;

namespace Gotcha.Core.Validation.DataAnnotations
{
    public class UserNameOrEmail : ValidationAttribute
    {
        // Same allowed characters as Identity's default UserOptions.AllowedUserNameCharacters
        private const string AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string input)
                return ValidationResult.Success!;

            if (string.IsNullOrWhiteSpace(input))
                return new ValidationResult("Please enter your username or email.");

            bool isValidEmail = UserValidationService.IsValidEmail(input);
            bool isValidUsername = IsValidUserName(input);

            if (!isValidEmail && !isValidUsername)
            {
                if (ErrorMessage == null)
                    ErrorMessage = "Please enter a valid username or email.";

                return new ValidationResult(ErrorMessage);
            }

            if (isValidUsername && UserValidationService.IsReservedUsername(input))
                return new ValidationResult("This username is not allowed.");

            return ValidationResult.Success!;
        }

        private static bool IsValidUserName(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;

            foreach (char c in username)
            {
                if (!AllowedUserNameCharacters.Contains(c))
                    return false;
            }

            return true;
        }
    }
}
