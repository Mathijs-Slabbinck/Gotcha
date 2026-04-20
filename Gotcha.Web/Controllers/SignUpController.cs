using Gotcha.Core.Entities.Models;
using Gotcha.Core.Interfaces;
using Gotcha.Core.Services.ValidationServices;
using Gotcha.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gotcha.Web.Controllers
{
    public class SignUpController : Controller
    {
        private readonly UserManager<GotchaUser> _userManager;
        private readonly IEmailService _emailService;

        public SignUpController(UserManager<GotchaUser> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpViewModel signUpViewModel)
        {
            if (!ModelState.IsValid)
                return View("Index", signUpViewModel);

            // Honeypot: check all text inputs for attack patterns (SQL injection, XSS, etc.)
            // and reserved names/usernames that bypass client-side validation
            string? suspiciousValue = SecurityValidationService.FindSuspiciousInput(
                signUpViewModel.FirstName,
                signUpViewModel.LastName,
                signUpViewModel.UserName,
                signUpViewModel.Email,
                signUpViewModel.GuardianEmail
            );

            bool hasReservedName = UserValidationService.IsReservedName(signUpViewModel.FirstName)
                                || UserValidationService.IsReservedName(signUpViewModel.LastName)
                                || UserValidationService.IsReservedUsername(signUpViewModel.UserName);

            if (suspiciousValue != null || hasReservedName)
            {
                TempData["GotchaOriginalPath"] = "/SignUp/SignUp";
                TempData["GotchaInvalidInput"] = suspiciousValue ?? $"Reserved: FirstName={signUpViewModel.FirstName}, LastName={signUpViewModel.LastName}, UserName={signUpViewModel.UserName}";
                return RedirectToAction("Index", "Gotcha");
            }

            // Validate and process the uploaded profile image (if provided)
            ProfileImage? profileImage = null;

            if (signUpViewModel.ProfileImage != null && signUpViewModel.ProfileImage.Length > 0)
            {
                IFormFile file = signUpViewModel.ProfileImage;

                // File size check
                if (!ImageValidationService.IsAllowedFileSize(file.Length))
                {
                    ModelState.AddModelError("ProfileImage", "This image is too large. The maximum file size is 8 MB.");
                    return View("Index", signUpViewModel);
                }

                // MIME type check
                if (!ImageValidationService.IsAllowedMimeType(file.ContentType))
                {
                    ModelState.AddModelError("ProfileImage", "This file type is not supported. Please upload a JPG, PNG, or WebP image.");
                    return View("Index", signUpViewModel);
                }

                // File extension check
                if (!ImageValidationService.IsAllowedFileExtension(file.FileName))
                {
                    ModelState.AddModelError("ProfileImage", "This file type is not supported. Please upload a JPG, PNG, or WebP image.");
                    return View("Index", signUpViewModel);
                }

                using Stream fileStream = file.OpenReadStream();

                // Magic bytes check — is this actually an image?
                byte[] header = new byte[12];
                await fileStream.ReadExactlyAsync(header, 0, 12);
                fileStream.Position = 0;

                if (!ImageValidationService.HasValidImageSignature(header, file.FileName))
                {
                    ModelState.AddModelError("ProfileImage", "This file does not appear to be a valid image.");
                    return View("Index", signUpViewModel);
                }

                // Dimension check
                var (isValidSize, width, height) = ImageValidationService.ValidateImageDimensions(fileStream);
                fileStream.Position = 0;

                if (!isValidSize)
                {
                    ModelState.AddModelError("ProfileImage",
                        $"Image dimensions must be between {ImageValidationService.MinImageDimension}x{ImageValidationService.MinImageDimension} " +
                        $"and {ImageValidationService.MaxImageDimension}x{ImageValidationService.MaxImageDimension} pixels. " +
                        $"Your image is {width}x{height}.");
                    return View("Index", signUpViewModel);
                }

                // Resize to 1500x1500 and re-encode as JPEG (strips metadata, proves it's a real image)
                using MemoryStream processedImage = ImageValidationService.ResizeAndReEncodeImage(fileStream);

                profileImage = new ProfileImage
                {
                    ImageData = processedImage.ToArray(),
                    MimeType = "image/jpeg",
                };
            }

            GotchaUser gotchaUser = new GotchaUser
            {
                FirstName = signUpViewModel.FirstName,
                LastName = signUpViewModel.LastName,
                Email = signUpViewModel.Email,
                BirthDate = signUpViewModel.BirthDay.Value,
                Gender = signUpViewModel.Gender.Value,
                GuardianEmail = signUpViewModel.GuardianEmail,
                UserName = signUpViewModel.UserName,
                ProfileImage = profileImage,
            };

            // CreateAsync hashes the password and saves the user + VipSettings to the DB
            IdentityResult result = await _userManager.CreateAsync(gotchaUser, signUpViewModel.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    string field = GetFieldForIdentityError(error.Code);
                    ModelState.AddModelError(field, error.Description);
                }

                return View("Index", signUpViewModel);
            }

            // Send email confirmation link
            string emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(gotchaUser);

            string? confirmationUrl = Url.Action(
                "ConfirmEmail",
                "Accounts",
                new { userId = gotchaUser.Id, token = emailToken },
                Request.Scheme
            );

            await _emailService.SendEmailAsync(
                gotchaUser.Email,
                "Confirm your Gotcha account",
                $"<h2>Welcome to Gotcha!</h2><p>Click the link below to confirm your email address:</p><p><a href=\"{confirmationUrl}\">Confirm Email</a></p>"
            );

            // Send guardian consent email if user is under 16
            if (gotchaUser.NeedsGuardianConsent())
            {
                string guardianToken = Guid.NewGuid().ToString();
                gotchaUser.GuardianConsentToken = guardianToken;
                await _userManager.UpdateAsync(gotchaUser);

                string? consentUrl = Url.Action(
                    "ConfirmGuardianConsent",
                    "Accounts",
                    new { userId = gotchaUser.Id, token = guardianToken },
                    Request.Scheme
                );

                await _emailService.SendEmailAsync(
                    gotchaUser.GuardianEmail!,
                    "Guardian consent required for Gotcha account",
                    $"<h2>Guardian Consent Required</h2><p>{gotchaUser.FirstName} {gotchaUser.LastName} has signed up for Gotcha and listed you as their parent or guardian.</p><p>Click the link below to give your consent:</p><p><a href=\"{consentUrl}\">Give Consent</a></p>"
                );
            }

            return RedirectToAction("CheckEmail");
        }

        public IActionResult CheckEmail()
        {
            return View();
        }

        private static string GetFieldForIdentityError(string errorCode)
        {
            if (errorCode.Contains("Password"))
                return "Password";

            if (errorCode.Contains("Email"))
                return "Email";

            if (errorCode.Contains("UserName"))
                return "UserName";

            return string.Empty;
        }
    }
}
