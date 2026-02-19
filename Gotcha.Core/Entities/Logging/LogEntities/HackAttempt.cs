using Gotcha.Core.Entities.Logging.LogEntities.Base;
using Gotcha.Core.Enums;
namespace Gotcha.Core.Entities.Logging.LogEntities
{
    public class HackAttempt : LogTypesBase
    {
        public HackAttempt(LogSubTypes logSubType, string message) : base(logSubType, message)
        {
        }

        public HackAttempt(LogSubTypes logSubType, string message, string extraInfo) : base(logSubType, message, extraInfo)
        {
        }
    }
}
