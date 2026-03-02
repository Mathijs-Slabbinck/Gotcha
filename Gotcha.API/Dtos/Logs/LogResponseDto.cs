namespace Gotcha.API.Dtos.Logs
{
    public class LogResponseDto
    {
        public Guid Id { get; set; }
        public Guid LogGroupId { get; set; }
        public DateTime TimeStamp { get; set; }
        public required string LogType { get; set; }
        public required string LogSubType { get; set; }
        public string? Message { get; set; }
        public string? ExtraInfo { get; set; }
        public string? ExceptionMessage { get; set; }
        public Guid AttackerId { get; set; }
    }
}
