namespace Gotcha.Core.Exceptions
{
    public class GameStateException : GotchaException
    {
        public GameStateException(string message) : base(message) { }
        public GameStateException(string message, Exception inner) : base(message, inner) { }
    }
}
