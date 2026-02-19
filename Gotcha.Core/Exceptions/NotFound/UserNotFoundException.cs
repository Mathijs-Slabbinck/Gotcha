namespace Gotcha.Core.Exceptions.NotFound
{
    public class UserNotFoundException : GotchaException
    {
        public Guid UserId { get; }
        public UserNotFoundException(Guid userId) : base($"User with ID {userId} not found.")
        {
            UserId = userId;
        }
    }
}
