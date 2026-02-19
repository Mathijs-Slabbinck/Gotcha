namespace Gotcha.Core.Exceptions.NotFound
{
    public class RulesNotFoundException : GotchaException
    {
        public Guid RulesId { get; }
        public RulesNotFoundException(Guid rulesId) : base($"Rules with ID {rulesId} not found.")
        {
            RulesId = rulesId;
        }
    }
}
