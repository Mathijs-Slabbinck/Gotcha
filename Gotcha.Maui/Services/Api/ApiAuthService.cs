using System.Net;
using System.Net.Http.Json;
using Gotcha.Maui.Extensions;
using Gotcha.Maui.Models;

namespace Gotcha.Maui.Services.Api
{
    public class ApiAuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public ApiAuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("GotchaApi");
        }

        public async Task<(Guid? UserId, string? ErrorMessage)> SignInAsync(string usernameOrEmail, string password)
        {
            try
            {
                object requestBody = new { UsernameOrEmail = usernameOrEmail, Password = password };

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/auth/signin", requestBody);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return (null, "Invalid username or password.");
                }

                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    string reasonCode = (await response.Content.ReadAsStringAsync()).Trim('"');

                    if (reasonCode == "guardian_consent")
                    {
                        return (null, "Guardian consent is required before you can sign in.");
                    }

                    return (null, "Please confirm your email before signing in.");
                }

                if (!response.IsSuccessStatusCode)
                {
                    return (null, "Something went wrong. Please try again.");
                }

                SignInResponse? result = await response.Content.ReadFromJsonAsync<SignInResponse>();

                if (result == null)
                {
                    return (null, "Something went wrong. Please try again.");
                }

                return (result.UserId, null);
            }
            catch
            {
                return (null, "Could not connect to the server. Please try again.");
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> SignUpAsync(SignUpData data)
        {
            try
            {
                object requestBody = new
                {
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    UserName = data.Username,
                    Email = data.Email,
                    Password = data.Password,
                    Gender = data.Gender,
                    BirthDate = data.Birthday,
                    GuardianEmail = data.GuardianEmail,
                };

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/auth/signup", requestBody);

                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }

                string? serverMessage = await response.Content.ReadJsonStringAsync();

                if (serverMessage == null)
                {
                    // Body wasn't a plain JSON string (e.g. ModelState dictionary on 400 validation failures).
                    // Log it for debugging but don't leak the raw object to the UI.
                    string rawBody = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"ApiAuthService sign-up non-string body: {rawBody}");
                }

                return (false, serverMessage ?? "Sign-up failed. Please check your details and try again.");
            }
            catch
            {
                return (false, "Could not connect to the server. Please try again.");
            }
        }

        private class SignInResponse
        {
            public Guid UserId { get; set; }
        }
    }
}
