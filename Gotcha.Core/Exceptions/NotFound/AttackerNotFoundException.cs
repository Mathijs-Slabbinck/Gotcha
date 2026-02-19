namespace Gotcha.Core.Exceptions.NotFound
{
    public class AttackerNotFoundException : GotchaException
    {
        public Guid AttackerId { get; }
        public AttackerNotFoundException(Guid attackerId) : base($"Attacker with ID {attackerId} not found.")
        {
            AttackerId = attackerId;
        }
    }
}
