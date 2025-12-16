using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gotcha.Core.Entities
{
    public class TargetAssignment
    {
        Guid HunterId { get; set; }
        Player Hunter { get; set; }
        Guid TargetId { get; set; }
        Player Target { get; set; }
        DateTime TargetAssigned { get; set }
        Kill? Kill { get; set; }
        string? Weapon { get; set; }
        string AssignmentStatus { get; set; }
    }
}
