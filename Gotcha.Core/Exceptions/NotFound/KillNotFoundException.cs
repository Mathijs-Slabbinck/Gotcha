namespace Gotcha.Core.Exceptions.NotFound
{
    public class KillNotFoundException : GotchaException
    {
        public Guid KillId { get; }
        public KillNotFoundException(Guid killId) : base($"Kill with ID {killId} not found.")
        {
            KillId = killId;
        }
    }
}