using Gotcha.Core.Entities.Logging.LogEntities.Base;
using Gotcha.Core.Enums;

namespace Gotcha.Core.Entities.Logging.LogEntities
{
    public class Error : LogTypesBase
    {
        private Exception? _exception;

        public Error(LogSubTypes logSubType, string message) : base(logSubType, message)
        {
        }

        public Error(Exception exception, LogSubTypes logSubType, string message) : base(logSubType, message)
        {
            _exception = exception;
        }

        public Error(LogSubTypes logSubType, string message, string? extraInfo) : base(logSubType, message, extraInfo)
        {
        }

        public Error(Exception exception, LogSubTypes logSubType, string message, string? extraInfo) : base(logSubType, message, extraInfo)
        {
            _exception = exception;
        }

        public Exception? ExceptionInfo
        {
            get { return _exception; }
        }
    }
}
