namespace Gotcha.Maui.Services.Mock
{
    public class MockContactService : IContactService
    {
        public Task<bool> SubmitAsync(string reason, string message)
        {
            return Task.FromResult(true);
        }
    }
}
