namespace Gotcha.Core.Exceptions.NotFound
{
    public class TargetAssignmentNotFoundException : GotchaException
    {
        public Guid HunterId { get; }
        public Guid VictimId { get; }
        public TargetAssignmentNotFoundException(Guid hunterId, Guid victimId) : base($"TargetAssignment with killer with id {hunterId} and with victim with id {victimId} not found.")
        {
            HunterId = hunterId;
            VictimId = victimId;
        }
    }
}
