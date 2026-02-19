namespace Gotcha.Core.Exceptions.NotFound
{
    public class LogNotFoundException : GotchaException
    {
        public Guid LogId { get; }
        public LogNotFoundException(Guid logId) : base($"Log with ID {logId} not found.")
        {
            LogId = logId;
        }
    }
}
