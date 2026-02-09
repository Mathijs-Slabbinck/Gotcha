using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gotcha.Core.Exceptions
{
    public class InvalidTargetAssignmentException : GotchaException
    {
        public Guid HunterId { get; }
        public Guid TargetId { get; }

        public InvalidTargetAssignmentException(Guid hunterId, Guid targetId, string message)
            : base(message)
        {
            HunterId = hunterId;
            TargetId = targetId;
        }
    }
}
