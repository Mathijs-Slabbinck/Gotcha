namespace Gotcha.Core.Services.ValidationServices
{
    public static class UserValidationService
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

        private static readonly HashSet<string> ReservedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "test",
            "user",
            "player",
            "unknown",
            "anonymous",
            "nobody",
            "deleted",
            "removed",
            "banned"
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

        public static bool IsReservedUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;

            return ReservedUsernames.Contains(username.Trim());
        }

        public static bool IsReservedName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            string trimmedName = name.Trim();

            if (ReservedUsernames.Contains(trimmedName))
                return true;

            if (ReservedNames.Contains(trimmedName))
                return true;

            return false;
        }

        private const int MinimumAge = 13;
        private const int MaximumAge = 150;

        internal static bool IsValidBirthDay(DateTime birthDate)
        {
            // Reject default DateTime values
            if (birthDate == DateTime.MinValue || birthDate == DateTime.MaxValue)
                return false;

            DateTime today = DateTime.UtcNow.Date;

            // Can't be born in the future
            if (birthDate.Date > today)
                return false;

            int age = today.Year - birthDate.Year;

            // Adjust if birthday hasn't happened yet this year
            if (birthDate.Date > today.AddYears(-age))
                age--;

            // Must be at least 13 (COPPA) and not unreasonably old
            if (age < MinimumAge || age > MaximumAge)
                return false;

            return true;
        }
    }
}
