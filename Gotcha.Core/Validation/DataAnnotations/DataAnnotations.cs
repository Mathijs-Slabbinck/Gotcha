using System.ComponentModel.DataAnnotations;
using Gotcha.Core.Services.ValidationServices;

namespace Gotcha.Core.Validation.DataAnnotations
{
    public class NotReservedUsernameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string username)
                return ValidationResult.Success!; // we return null on purpose (hence the !) [validation passed]

            if (LastLineValidationService.IsReservedUsername(username))
            {
                string? errormMessage = ErrorMessage;

                if (errormMessage == null)
                    errormMessage = "This username is not allowed. Please choose another.";

                return new ValidationResult(errormMessage);
            }

            return ValidationResult.Success!; // we return null on purpose (hence the !) [validation passed]
        }
    }
}
