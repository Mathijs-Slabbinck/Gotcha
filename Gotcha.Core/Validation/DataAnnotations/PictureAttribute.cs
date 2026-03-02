using System.ComponentModel.DataAnnotations;
using Gotcha.Core.Services.ValidationServices;
using Microsoft.AspNetCore.Http;

namespace Gotcha.Core.Validation.DataAnnotations
{
    public class Picture : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            // Null — that's fine, the image is optional
            if (value is null)
                return ValidationResult.Success!; // we return null on purpose (hence the !) [validation passed]

            // --- URL path (string) ---
            if (value is string url)
            {
                if (!ImageValidationService.IsAllowedImageUrl(url))
                {
                    if (ErrorMessage == null)
                        ErrorMessage = "Please enter a valid image URL.";

                    return new ValidationResult(ErrorMessage);
                }

                return ValidationResult.Success!;
            }

            // --- File upload path (IFormFile) ---
            if (value is IFormFile file)
            {
                // 1. Check file extension (.jpg, .jpeg, .png, .webp)
                if (!ImageValidationService.IsAllowedFileExtension(file.FileName))
                    return new ValidationResult("Only .jpg, .jpeg, .png, and .webp images are allowed.");

                // 2. Check MIME type (image/jpeg, image/png, image/webp)
                if (!ImageValidationService.IsAllowedMimeType(file.ContentType))
                    return new ValidationResult("The file type is not a supported image format.");

                // 3. Check file size (max 8 MB)
                if (!ImageValidationService.IsAllowedFileSize(file.Length))
                    return new ValidationResult("The image must be between 1 byte and 8 MB.");

                // 4. Check magic bytes — make sure the actual file content matches the extension
                //    A valid image is always larger than 12 bytes (the minimum header size we check)
                if (file.Length < 12)
                    return new ValidationResult("The file is too small to be a valid image.");

                byte[] fileHeader = new byte[12];

                using (Stream stream = file.OpenReadStream())
                {
                    stream.ReadExactly(fileHeader, 0, fileHeader.Length);
                }

                if (!ImageValidationService.HasValidImageSignature(fileHeader, file.FileName))
                    return new ValidationResult("The file content does not match the expected image format.");

                return ValidationResult.Success!;
            }

            // Unsupported type — reject
            return new ValidationResult("Invalid image value.");
        }
    }
}
