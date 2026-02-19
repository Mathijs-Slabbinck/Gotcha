namespace Gotcha.Core.Exceptions
{
    public class InsufficientPlayersException : GameStateException
    {
        public int CurrentPlayers { get; }
        public int RequiredPlayers { get; }

        public InsufficientPlayersException(int current, int required)
            : base($"Insufficient players: {current}/{required}")
        {
            CurrentPlayers = current;
            RequiredPlayers = required;
        }
    }
}
