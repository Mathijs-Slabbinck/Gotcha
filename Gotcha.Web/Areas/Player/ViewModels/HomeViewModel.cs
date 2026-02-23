using Gotcha.Core.Enums;

namespace Gotcha.Web.Areas.Player.ViewModels
{
    public class HomeViewModel
    {
        public string? Username { get; set; }
        public string? FullName { get; set; } // e.g., "John Doe" (FirstName + " " + LastName)
        public string? ProfileImgSource { get; set; }
        public DateTime? AssignmentExpirationDate { get; set; }
        public Genders? Gender { get; set; }
        public string? Weapon { get; set; }
    }
}
