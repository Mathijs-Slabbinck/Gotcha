using Gotcha.Core.Entities.Logging.LogEntities.Base;
using Gotcha.Core.Enums;
namespace Gotcha.Core.Entities.Logging.LogEntities
{
    public class OtherLogType : LogTypesBase
    {
        public OtherLogType(LogSubTypes logSubType, string message) : base(logSubType, message)
        {
        }

        public OtherLogType(LogSubTypes logSubType, string message, string extraInfo) : base(logSubType, message, extraInfo)
        {
        }
    }
}
