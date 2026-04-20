using System.Net.Http.Json;

namespace Gotcha.Maui.Extensions
{
    public static class HttpContentExtensions
    {
        // ASP.NET's BadRequest(string) JSON-encodes the body (e.g. "\"Game not started\"").
        // This reads and unwraps it via a real JSON parse so escape sequences are handled safely.
        // Returns null when the body isn't a JSON string (e.g. ModelState dictionary on 400 validation failures).
        public static async Task<string?> ReadJsonStringAsync(this HttpContent content)
        {
            try
            {
                return await content.ReadFromJsonAsync<string>();
            }
            catch
            {
                return null;
            }
        }
    }
}
