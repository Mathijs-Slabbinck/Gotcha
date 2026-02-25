namespace Gotcha.Web.Areas.User.ViewModels
{
    public class GamesViewModel
    {
        public List<GameItemViewModel> ActiveGames { get; set; } = new List<GameItemViewModel>();
        public List<GameItemViewModel> EndedGames { get; set; } = new List<GameItemViewModel>();
    }
}
