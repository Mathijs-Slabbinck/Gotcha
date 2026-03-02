using System.ComponentModel.DataAnnotations;
using Gotcha.Core.Services.ValidationServices;

namespace Gotcha.Core.Validation.DataAnnotations
{
    public class Name : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string name)
                return ValidationResult.Success!; // we return null on purpose (hence the !) [validation passed]

            if (UserValidationService.IsReservedName(name))
            {
                if (ErrorMessage == null)
                    ErrorMessage = "This name is not allowed. Please use your real name.";

                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success!; // we return null on purpose (hence the !) [validation passed]
        }
    }
}
