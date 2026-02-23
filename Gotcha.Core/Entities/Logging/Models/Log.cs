using Gotcha.Core.Enums;

namespace Gotcha.Core.Entities.Logging.Models
{
    public class Log
    {
        #region Properties
        public Guid Id { get; init; } = Guid.NewGuid();
        public Guid LogGroupId { get; init; } = Guid.NewGuid();
        public DateTime TimeStamp { get; init; } = DateTime.UtcNow;
        public LogTypes LogType { get; init; }
        public LogSubTypes LogSubType { get; init; } = LogSubTypes.Unknown;
        public string? Message { get; init; }
        public string? ExtraInfo { get; init; }
        public Exception? Exception { get; init; }
        public Attacker? Attacker { get; init; }
        public Guid AttackerId { get; init; }
        #endregion
    }
}
