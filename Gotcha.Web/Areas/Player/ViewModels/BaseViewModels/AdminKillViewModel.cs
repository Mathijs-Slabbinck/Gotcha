namespace Gotcha.Web.Areas.Player.ViewModels.BaseViewModels
{
    public class AdminKillViewModel
    {
        public Guid KillId { get; set; }
        public string KillerName { get; set; } = string.Empty;
        public string VictimName { get; set; } = string.Empty;
        public string? Weapon { get; set; }
        public DateTime Moment { get; set; }
    }
}
