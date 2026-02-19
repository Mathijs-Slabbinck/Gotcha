namespace Gotcha.Core.Exceptions.NotFound
{
    public class GameNotFoundException : GotchaException
    {
        public Guid GameId { get; }
        public GameNotFoundException(Guid gameId) : base($"Game with ID {gameId} not found.")
        {
            GameId = gameId;
        }
    }
}
