namespace Gotcha.API.Dtos.Players
{
    public class UpdatePlayerDto
    {
        public string? UserName { get; set; }
        public string? ProfileImageSource { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? IsSpectator { get; set; }
        public string? Notes { get; set; }
    }
}
