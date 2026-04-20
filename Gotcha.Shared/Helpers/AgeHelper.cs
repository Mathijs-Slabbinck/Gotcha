namespace Gotcha.Shared.Helpers
{
    public static class AgeHelper
    {
        // Shared so the MAUI client and the API controller compute "under 16" the same way
        // — the guardian-consent flow depends on both sides agreeing on this boundary.
        public static bool IsUnder16(DateTime birthDate)
        {
            int age = DateTime.Today.Year - birthDate.Year;
            if (birthDate > DateTime.Today.AddYears(-age))
            {
                age--;
            }
            return age < 16;
        }
    }
}
