using System.ComponentModel.DataAnnotations;

namespace Gotcha.Core.Validation.DataAnnotations
{
    /// <summary>
    /// Makes the annotated field required when the user is under the guardian consent age (16).
    /// Requires the name of the DateTime property that holds the birth date.
    /// Usage: [GuardianRequired("BirthDay")]
    /// </summary>
    public class GuardianRequired : ValidationAttribute
    {
        private const int GuardianConsentAge = 16;

        private readonly string _birthDatePropertyName;

        public GuardianRequired(string birthDatePropertyName)
        {
            _birthDatePropertyName = birthDatePropertyName;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            // Get the birth date property from the model
            var birthDateProperty = validationContext.ObjectType.GetProperty(_birthDatePropertyName);

            if (birthDateProperty == null)
                return new ValidationResult($"Property '{_birthDatePropertyName}' not found on the model.");

            object? birthDateValue = birthDateProperty.GetValue(validationContext.ObjectInstance);

            if (birthDateValue is not DateTime birthDate)
                return ValidationResult.Success!; // we return null on purpose (hence the !) [validation passed]

            // Calculate age
            DateTime today = DateTime.UtcNow.Date;
            int age = today.Year - birthDate.Year;

            // Adjust if birthday hasn't happened yet this year
            if (birthDate.Date > today.AddYears(-age))
                age--;

            // If old enough, guardian email is not required
            if (age >= GuardianConsentAge)
                return ValidationResult.Success!;

            // Under 16 — guardian email is required
            string? guardianEmail = value as string;

            if (string.IsNullOrWhiteSpace(guardianEmail))
            {
                if (ErrorMessage == null)
                    ErrorMessage = "A parent or guardian email is required for users under 16.";

                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success!; // we return null on purpose (hence the !) [validation passed]
        }
    }
}
