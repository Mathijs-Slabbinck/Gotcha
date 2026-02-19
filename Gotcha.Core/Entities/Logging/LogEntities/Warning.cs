using Gotcha.Core.Entities.Logging.LogEntities.Base;
using Gotcha.Core.Enums;
namespace Gotcha.Core.Entities.Logging.LogEntities
{
    public class Warning : LogTypesBase
    {
        public Warning(LogSubTypes logSubType, string message) : base(logSubType, message)
        {
        }

        public Warning(LogSubTypes logSubType, string message, string extraInfo) : base(logSubType, message, extraInfo)
        {
        }
    }
}
