using System.Net;
using Gotcha.API.Dtos.Auth;
using Gotcha.Core.Entities.Models;
using Gotcha.Core.Enums;
using Gotcha.Core.Interfaces;
using Gotcha.Shared.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gotcha.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<GotchaUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<GotchaUser> userManager, IEmailService emailService, IConfiguration configuration)
        {
            _userManager = userManager;
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string normalizedInput = dto.UsernameOrEmail.ToUpperInvariant();

            GotchaUser? user = await _userManager.Users
                .FirstOrDefaultAsync(u => !u.IsDeleted
                    && (u.NormalizedEmail == normalizedInput || u.NormalizedUserName == normalizedInput));

            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            bool passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!passwordValid)
            {
                return Unauthorized("Invalid credentials.");
            }

            if (!user.EmailConfirmed)
            {
                return StatusCode(403, "email_unconfirmed");
            }

            if (user.NeedsGuardianConsent())
            {
                return StatusCode(403, "guardian_consent");
            }

            SignInResponseDto response = new SignInResponseDto
            {
                UserId = user.Id
            };

            return Ok(response);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!Enum.TryParse<Genders>(dto.Gender, ignoreCase: true, out Genders gender))
            {
                return BadRequest("Invalid gender value.");
            }

            // Under-16 users must provide a guardian email (COPPA/GDPR). The MAUI client checks this
            // too, but anyone calling the API directly would bypass it — enforce server-side as well.
            if (AgeHelper.IsUnder16(dto.BirthDate) && string.IsNullOrWhiteSpace(dto.GuardianEmail))
            {
                return BadRequest("Parent/guardian email is required for users under 16.");
            }

            GotchaUser gotchaUser = new GotchaUser
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                UserName = dto.UserName,
                Email = dto.Email,
                BirthDate = dto.BirthDate,
                Gender = gender,
                GuardianEmail = dto.GuardianEmail,
            };

            IdentityResult result = await _userManager.CreateAsync(gotchaUser, dto.Password);

            if (!result.Succeeded)
            {
                // Redact uniqueness errors ("Email is already taken") to prevent account enumeration.
                // Password-policy errors are safe to return since they're about the caller's input.
                bool hasUniquenessError = result.Errors.Any(e =>
                    e.Code.Contains("Duplicate", StringComparison.OrdinalIgnoreCase));

                if (hasUniquenessError)
                {
                    return BadRequest("Sign-up failed. Please check your details and try again.");
                }

                string allErrors = string.Join(" ", result.Errors.Select(e => e.Description));
                return BadRequest(allErrors);
            }

            string webAppBaseUrl = _configuration["WebAppBaseUrl"] ?? throw new InvalidOperationException("WebAppBaseUrl is not configured.");
            string emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(gotchaUser);
            string confirmationUrl = $"{webAppBaseUrl}/Accounts/ConfirmEmail?userId={gotchaUser.Id}&token={Uri.EscapeDataString(emailToken)}";

            Task confirmEmailTask = _emailService.SendEmailAsync(
                gotchaUser.Email,
                "Confirm your Gotcha account",
                $"<h2>Welcome to Gotcha!</h2><p>Click the link below to confirm your email address:</p><p><a href=\"{confirmationUrl}\">Confirm Email</a></p>"
            );

            if (gotchaUser.NeedsGuardianConsent())
            {
                string guardianToken = Guid.NewGuid().ToString();
                gotchaUser.GuardianConsentToken = guardianToken;
                await _userManager.UpdateAsync(gotchaUser);

                string consentUrl = $"{webAppBaseUrl}/Accounts/ConfirmGuardianConsent?userId={gotchaUser.Id}&token={Uri.EscapeDataString(guardianToken)}";

                // HTML-encode user-provided names so an injected "<img onerror=...>" can't run in the guardian's email client.
                string safeFirstName = WebUtility.HtmlEncode(gotchaUser.FirstName);
                string safeLastName = WebUtility.HtmlEncode(gotchaUser.LastName);

                Task guardianEmailTask = _emailService.SendEmailAsync(
                    gotchaUser.GuardianEmail!,
                    "Guardian consent required for Gotcha account",
                    $"<h2>Guardian Consent Required</h2><p>{safeFirstName} {safeLastName} has signed up for Gotcha and listed you as their parent or guardian.</p><p>Click the link below to give your consent:</p><p><a href=\"{consentUrl}\">Give Consent</a></p>"
                );

                await Task.WhenAll(confirmEmailTask, guardianEmailTask);
            }
            else
            {
                await confirmEmailTask;
            }

            return Ok();
        }

    }
}
