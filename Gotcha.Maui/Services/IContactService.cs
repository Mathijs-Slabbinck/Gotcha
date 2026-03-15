namespace Gotcha.Maui.Services
{
    public interface IContactService
    {
        Task<bool> SubmitAsync(string reason, string message);
    }
}
