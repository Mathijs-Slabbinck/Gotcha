using System;
using System.Collections.Generic;
using System.Text;
using Gotcha.Core.Entities.Logging.LogEntities.Base;
using Gotcha.Core.Enums;
using Gotcha.Core.Interfaces;

namespace Gotcha.Core.Entities.Logging.LogEntities
{
    public class OtherLogType : LogTypesBase, ILogEntity
    {
        public OtherLogType(LogSubTypes logSubType, string message) : base(logSubType, message)
        {
        }

        public OtherLogType(string exceptionInfo, LogSubTypes logSubType, string message) : base(logSubType, message)
        {
        }

        public OtherLogType(LogSubTypes logSubType, string message, string extraInfo) : base(logSubType, message, extraInfo)
        {
        }

        public OtherLogType(string exceptionInfo, LogSubTypes logSubType, string message, string extraInfo) : base(logSubType, message, extraInfo)
        {
        }
    }
}
