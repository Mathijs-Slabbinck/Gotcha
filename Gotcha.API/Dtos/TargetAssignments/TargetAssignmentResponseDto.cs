namespace Gotcha.API.Dtos.TargetAssignments
{
    public class TargetAssignmentResponseDto
    {
        public Guid Id { get; set; }
        public Guid HunterId { get; set; }
        public Guid TargetId { get; set; }
        public DateTime TargetAssigned { get; set; }
        public DateTime? AssignmentFinished { get; set; }
        public DateTime? AssignmentExpirationDate { get; set; }
        public Guid? KillId { get; set; }
        public string? Weapon { get; set; }
        public required string AssignmentStatus { get; set; }
    }
}
