namespace Gotcha.Maui.Models.Items
{
    public class AdminPlayerItem
    {
        public Guid PlayerId { get; init; }
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public bool HasImage { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsSpectator { get; set; }
    }
}
