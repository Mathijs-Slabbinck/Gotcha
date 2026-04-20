using Gotcha.Core.Enums;

namespace Gotcha.Core.Entities.Logging.LogEntities.Base
{
    /* At first glance this class and it's subclasses may seem redundant, but they serve a crucial purpose in the architecture of the logging system.
     * 1) Error carries an extra field the others don't. (private Exception? _exception and an ExceptionInfo getter)
     * 2) Strong typing in BaseResultModel (declares List<Error> Errors and List<Warning> Warnings)
     * 3) Per-type constructor shapes (different parameters / overloads for each log type, e.g. Error requires an Exception while the others don't) */

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
