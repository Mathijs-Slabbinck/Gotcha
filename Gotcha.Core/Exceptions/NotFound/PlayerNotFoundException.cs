namespace Gotcha.Core.Exceptions.NotFound
{
    public class PlayerNotFoundException : GotchaException
    {
        public Guid PlayerId { get; }
        public PlayerNotFoundException(Guid playerId) : base($"Player with ID {playerId} not found.")
        {
            PlayerId = playerId;
        }
    }
}
