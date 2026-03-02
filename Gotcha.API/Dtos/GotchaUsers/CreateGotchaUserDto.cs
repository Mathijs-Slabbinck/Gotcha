namespace Gotcha.API.Dtos.GotchaUsers
{
    public class CreateGotchaUserDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? ProfileImageSource { get; set; }
        public required string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string? GuardianEmail { get; set; }
    }
}
