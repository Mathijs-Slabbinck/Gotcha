namespace Gotcha.Core.Exceptions
{
    public class GotchaException : Exception
    {
        public GotchaException(string message) : base(message) { }
        public GotchaException(string message, Exception inner) : base(message, inner) { }
    }
}
