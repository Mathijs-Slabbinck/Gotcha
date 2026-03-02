namespace Gotcha.API.Dtos.GotchaUsers
{
    public class UpdateGotchaUserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? ProfileImageSource { get; set; }
        public string? Gender { get; set; }
    }
}
