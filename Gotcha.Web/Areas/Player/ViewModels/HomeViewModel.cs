using Gotcha.Core.Enums;
using Gotcha.Web.Areas.Player.ViewModels.BaseViewModels;

namespace Gotcha.Web.Areas.Player.ViewModels
{
    public class HomeViewModel
    {
        public string? TargetUsername { get; set; }
        public string? TargetName { get; set; } // e.g., "John Doe" (FirstName + " " + LastName)
        public string? TargetProfileImgSource { get; set; }
        public string? PlayerProfileImgSource { get; set; }
        public DateTime? AssignmentExpirationDate { get; set; }
        public Genders? TargetGender { get; set; }
        public Genders? PlayerGender { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? KilledOnDate { get; set; }
        public string? KillerName { get; set; }
        public string? KillerOtherName { get; set; }

        /* e.g :
         * "TheLegend27" (if only show usernames)
         * "John Doe" (if only show real names)
         */
        public string? WinnerName { get; set; }
        /* In case both show usernames and show real names are turned on
         */
        public string? WinnerOtherName { get; set; }
        public List<KillBaseViewModel> Kills { get; set; } = new List<KillBaseViewModel>();
        public string? Weapon { get; set; } // both for current players weapon and the weapon they got killed by (since can't be both at same time)
        public List<PlayerBaseViewModel> Players { get; set; } = new List<PlayerBaseViewModel>();
    }
}
