namespace Gotcha.API.Dtos.TargetAssignments
{
    public class CreateTargetAssignmentDto
    {
        public Guid HunterId { get; set; }
        public Guid TargetId { get; set; }
        public string? Weapon { get; set; }
        public DateTime? AssignmentExpirationDate { get; set; }
    }
}
