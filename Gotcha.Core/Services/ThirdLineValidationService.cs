using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gotcha.Core.Services
{
    public static class ThirdLineValidationService
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // More robust regex pattern
            string pattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
            if (!Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase))
            {
                return false;
            }

            // Additional validation using MailAddress to catch edge cases
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

        public static bool IsInputClean(string input)
        {
            if(input == null ||
               input.Equals("null", StringComparison.OrdinalIgnoreCase) ||
               input.Equals("void", StringComparison.OrdinalIgnoreCase) ||
               input.Equals("undefined", StringComparison.OrdinalIgnoreCase) ||
               input.Equals("NaN", StringComparison.OrdinalIgnoreCase) ||
               input.Equals("[Object Object]", StringComparison.OrdinalIgnoreCase)
               )
               {
                    return false;
               }

            // Common SQL injection keywords (case-insensitive)
            string[] sqlKeywords = {
                "DROP TABLE", "SELECT * FROM", "INSERT INTO", "DELETE FROM", "UPDATE SET",
                "ALTER TABLE", "CREATE TABLE", "TRUNCATE TABLE", "EXECUTE", "EXEC", "UNION ALL", "-"
            };

            foreach (string keyword in sqlKeywords)
            {
                if (input.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            string[] dangerousChars = { "<", ">", "&", "\"", "'", ";", "{", "}", "$", "%", "\\", "/", "|", "+" };

            foreach (string dangerousChar in dangerousChars)
            {
                if (input.Contains(dangerousChar))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsCleanProfileImageSource(string? profileImageSource)
        {
            // Null or empty is considered clean
            if (string.IsNullOrEmpty(profileImageSource))
            {
                return true; 
            }

            // Check for valid URL format (basic check)
            if (!Uri.TryCreate(profileImageSource, UriKind.Absolute, out var uriResult) ||
                uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps)
            {
                return false;
            }

            return IsInputClean(profileImageSource);
        }

        // Sanitize input for XSS and basic SQL injection
        public static string SaniziteInput(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            // HTML encode to prevent XSS
            // &copy; => ©; &amp; => &; &lt; => <; &gt; => > ...
            string sanitized = WebUtility.HtmlEncode(input);

            // If there are any remaining raw HTML tags, delete them
            sanitized = Regex.Replace(sanitized, @"<.*?>", string.Empty);

            if(!IsInputClean(sanitized))
            {
                throw new ArgumentException("Input still contains potentially dangerous content after sanitization!");
            }

            return sanitized;
        }

        public static bool IsValidLength(string input, int minLength, int maxLength)
        {
            if (string.IsNullOrEmpty(input))
                return false;
            return input.Length >= minLength && input.Length <= maxLength;
        }
    }
}
