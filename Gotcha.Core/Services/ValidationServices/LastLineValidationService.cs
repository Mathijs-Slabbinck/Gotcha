using Gotcha.Core.Entities;
using Microsoft.Extensions.Configuration;
using Gotcha.Core.Services.ResultModel;

namespace Gotcha.Core.Services.ValidationServices
{
    public static class LastLineValidationService
    {
        internal static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        internal static bool IsReservedUsername(string username)
        {
            UserNameValidationHelper usernameValidationService = new UserNameValidationHelper(new ConfigurationBuilder()
                                                                                                        .AddJsonFile("appsettings.json")
                                                                                                        .Build());

            return usernameValidationService.IsReservedUsername(username.Trim());
        }

        internal static bool IsAllowedImageUrl(string? url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return true;
            }

            // Validate URL format
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
                return false;

            // Only allow http/https
            if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
            {
                return false;
            }

            /*
            // Optional: Block certain domains
            string[] blockedDomains = { "example-malicious-site.com" };
            if (blockedDomains.Any(d => uri.Host.Contains(d, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }
            */

            return true;
        }

        public static bool IsValidIP(string ip)
        {
            if (string.IsNullOrEmpty(ip))
                return false;

            string[] ipAsArray = ip.Split(".");

            if (ipAsArray.Length != 4)
                return false;

            foreach (string ipBlock in ipAsArray)
            {
                if (string.IsNullOrEmpty(ipBlock) || ipBlock.Length > 3 || !int.TryParse(ipBlock, out int num) || num < 0 || num > 255)
                    return false;

                // Reject leading zeros unless the octet is exactly "0"
                if (ipBlock.Length > 1 && ipBlock.StartsWith("0"))
                    return false;
            }

            return true;
        }
        
        /*
        public static Task<ResultModel<Attacker>> LogAttacker(string trigger, string input, HttpRequest httpRequest)
        {
        */
            /* !!! TO DO !!! */
            // check if the attacked is logged in
            // if logged in set the UserId in the Attacker entity's UserId property

        /*

            Guid UserId = Guid.Empty; // get the UserId from the session or authentication context

            if (string.IsNullOrEmpty(trigger))
                trigger = "Unknown Trigger";

            if (string.IsNullOrEmpty(input))
                input = "Unknown Input";


            Attacker attacker = new Attacker()
            {
                IpAdress = httpRequest.HttpContext.Connection.RemoteIpAddress?.ToString(),
                UserAgent = httpRequest.Headers["User-Agent"].ToString(),
                Referer = httpRequest.Headers["Referer"].ToString(),
                TimeStamp = DateTime.UtcNow,
                Path = httpRequest.Path,
                InvalidInput = input,
                SessionId = httpRequest.HttpContext.Session.Id,
                UserId = UserId,
            };
        }
        */
    }
}
