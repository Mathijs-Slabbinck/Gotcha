namespace Gotcha.Core.Exceptions
{
    public class ValidationException : GotchaException
    {
        public string FieldName { get; }
        public string File { get; }

        public ValidationException(string fieldName, string file, string message) : base(message)
        {
            FieldName = fieldName;
            File = file;
        }
    }
}
