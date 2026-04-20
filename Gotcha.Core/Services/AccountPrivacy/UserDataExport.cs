using System.Text.Json.Serialization;
using Gotcha.Core.Enums;

namespace Gotcha.Core.Services.AccountPrivacy
{
    public class UserDataExport
    {
        public DateTime ExportedAt { get; init; } = DateTime.UtcNow;
        public required UserProfileExport Profile { get; init; }
        public required List<GameExportRecord> Games { get; init; }
        public required List<KillExportRecord> Kills { get; init; }
        public required List<KillExportRecord> Deaths { get; init; }
    }

    public class UserProfileExport
    {
        public Guid Id { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string UserName { get; init; }
        public required string Email { get; init; }
        public required string Gender { get; init; }
        public DateTime BirthDate { get; init; }
        public DateTime AccountCreationDate { get; init; }
        public string? PhoneNumber { get; init; }
        public string? GuardianEmail { get; init; }
        public bool HasGuardianConsent { get; init; }
        public DateTime? GuardianConsentDate { get; init; }
    }

    public class GameExportRecord
    {
        [JsonIgnore]
        public Guid PlayerId { get; init; }

        public required string GameName { get; init; }
        public DateTime CreationDate { get; init; }
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public bool HasStarted { get; init; }
        public bool IsFinished { get; init; }
        public string? MyUsernameInGame { get; init; }
        public bool IsAlive { get; init; }
        public bool IsAdmin { get; init; }
        public bool IsSpectator { get; init; }
    }

    public class KillExportRecord
    {
        public DateTime Moment { get; init; }
        public string? Weapon { get; init; }
        public bool IsValid { get; init; }
        public KillRole Role { get; init; }
    }
}
