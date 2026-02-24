namespace Gotcha.Web.Areas.Player.ViewModels.BaseViewModels
{
    public class PlayerBaseViewModel
    {
        public string Name { get; set; }
        public string? OtherName { get; set; }
        public bool IsAlive { get; set; } = true;
    }
}
