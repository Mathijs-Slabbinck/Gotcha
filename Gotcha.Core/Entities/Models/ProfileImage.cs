namespace Gotcha.Core.Entities.Models
{
    public class ProfileImage
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public byte[] ImageData { get; set; }
        public string MimeType { get; set; }
    }
}
