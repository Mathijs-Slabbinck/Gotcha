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
