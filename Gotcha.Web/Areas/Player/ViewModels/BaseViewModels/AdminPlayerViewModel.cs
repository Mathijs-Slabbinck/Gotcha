namespace Gotcha.Web.Areas.Player.ViewModels.BaseViewModels
{
    public class AdminPlayerViewModel
    {
        public Guid PlayerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Username { get; set; }
        public bool HasImage { get; set; }
    }
}
