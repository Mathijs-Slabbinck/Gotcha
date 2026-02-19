namespace Gotcha.Core.Services.ValidationServices
{
    public static class LastLineValidationService
    {
        private static readonly HashSet<string> ReservedUsernames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "admin",
            "system",
            "root",
            "null",
            "undefined",
            "void",
            "nan",
            "[object object]",
            "gotcha",
            "moderator"
        };

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
            if (string.IsNullOrWhiteSpace(username))
                return false;

            return ReservedUsernames.Contains(username.Trim());
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
    }
}
