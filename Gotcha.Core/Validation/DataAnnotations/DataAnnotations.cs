using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;

namespace Gotcha.Core.Validation.DataAnnotations
{
    public class NotReservedUsernameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string username)
                return ValidationResult.Success!;

            UsernameValidationService validationService = new UsernameValidationService(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build());
            if (validationService.IsReservedUsername(username))
            {
                return new ValidationResult(
                    ErrorMessage ?? "This username is not allowed. Please choose another."
                );
            }

            return ValidationResult.Success!;
        }
    }
}