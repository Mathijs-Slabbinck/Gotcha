using System.ComponentModel.DataAnnotations;
using Gotcha.Core.Services.ValidationServices;

namespace Gotcha.Core.Validation.DataAnnotations
{
    public class BirthDay : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not DateTime birthDate)
                return ValidationResult.Success!; // we return null on purpose (hence the !) [validation passed]

            if (!UserValidationService.IsValidBirthDay(birthDate))
            {
                if (ErrorMessage == null)
                    ErrorMessage = "Please enter a valid date of birth. You must be at least 13 years old.";

                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success!; // we return null on purpose (hence the !) [validation passed]
        }
    }
}
