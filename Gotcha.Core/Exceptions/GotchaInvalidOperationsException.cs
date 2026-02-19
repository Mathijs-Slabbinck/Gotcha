namespace Gotcha.Core.Exceptions
{
    public class GotchaInvalidOperationException : GotchaException
    {
        public GotchaInvalidOperationException(string message) : base(message) { }
        public GotchaInvalidOperationException(string message, Exception inner) : base(message, inner) { }
    }
}
