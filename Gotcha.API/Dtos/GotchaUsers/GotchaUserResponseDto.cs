namespace Gotcha.API.Dtos.GotchaUsers
{
    public class GotchaUserResponseDto
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public string? ProfileImageSource { get; set; }
        public required string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime AccountCreationDate { get; set; }
        public string? GuardianEmail { get; set; }
        public bool HasGuardianConsent { get; set; }
        public DateTime? GuardianConsentDate { get; set; }
    }
}
