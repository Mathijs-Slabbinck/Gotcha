using System;
using System.Collections.Generic;
using System.Text;
using Gotcha.Core.Enums;

namespace Gotcha.Core.Entities.Logging.LogEntities.Base
{
    public abstract class LogTypesBase
    {
        private string _message;
        private LogSubTypes _logSubType;
        private string? _extraInfo;

        public LogTypesBase(LogSubTypes logSubType, string message, string? extraInfo = null)
        {
            _logSubType = logSubType;
            _message = message;
            _extraInfo = extraInfo;
        }

        public string Message
        {
            get { return _message; }
        }

        public LogSubTypes LogSubType
        {
            get { return _logSubType; }
        }

        public string? ExtraInfo
        {
            get { return _extraInfo; }
        }
    }
}
