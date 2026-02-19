using System;
using System.Collections.Generic;
using System.Text;
using Gotcha.Core.Entities.Logging.LogEntities.Base;
using Gotcha.Core.Enums;
using Gotcha.Core.Interfaces;

namespace Gotcha.Core.Entities.Logging.LogEntities
{
    public class Warning : LogTypesBase, ILogEntity
    {
        public Warning(LogSubTypes logSubType, string message) : base(logSubType, message)
        {
        }

        public Warning(string exceptionInfo, LogSubTypes logSubType, string message) : base(logSubType, message)
        {
        }

        public Warning(LogSubTypes logSubType, string message, string extraInfo) : base(logSubType, message, extraInfo)
        {
        }

        public Warning(string exceptionInfo, LogSubTypes logSubType, string message, string extraInfo) : base(logSubType, message, extraInfo)
        {
        }
    }
}
