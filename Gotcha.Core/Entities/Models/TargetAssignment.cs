using Gotcha.Core.Enums;

namespace Gotcha.Core.Entities.Models
{
    public class TargetAssignment
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public Guid HunterId { get; init; }
        public Player Hunter { get; init; }
        public Guid TargetId { get; init; }
        public Player Target { get; init; }
        public DateTime TargetAssigned { get; init; } = DateTime.UtcNow;
        public DateTime? AssignmentFinished { get; set; }
        public DateTime? AssignmentExpirationDate { get; set; }
        public Kill? Kill { get; set; }
        public string? Weapon { get; init; }
        public AssignmentStatus AssignmentStatus { get; set; } = AssignmentStatus.Ongoing;
    }
}
