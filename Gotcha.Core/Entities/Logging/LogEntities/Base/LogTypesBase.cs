using Gotcha.Core.Enums;

namespace Gotcha.Core.Entities.Logging.LogEntities.Base
{
    public abstract class LogTypesBase
    {
        public LogSubTypes LogSubType { get; }
        public string Message { get; }
        public string? ExtraInfo { get; }

        public LogTypesBase(LogSubTypes logSubType, string message, string? extraInfo = null)
        {
            LogSubType = logSubType;
            Message = message;
            ExtraInfo = extraInfo;
        }
    }
}
