using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gotcha.Core.Enums;

namespace Gotcha.Core.Entities
{
    public class TargetAssignment
    {
        private Guid _id;
        private readonly Guid _hunterId;
        private readonly Player _hunter;
        private readonly Guid _targetId;
        private readonly Player _target;
        private readonly DateTime _targetAssigned;
        private Kill? kill;
        private readonly string? _weapon;
        private AssignmentStatus assignmentStatus;

        /* Constructor without weapon and reason
         * Used when creating a new Kill object without the 'CustomWeapons' rule during gameplay */
        public TargetAssignment(Player hunter, Player target)
        {
            _id = Guid.NewGuid();
            _hunterId = hunter.Id;
            _hunter = hunter;
            _targetId = target.Id;
            _target = target;
            _targetAssigned = DateTime.UtcNow;
            assignmentStatus = AssignmentStatus.Ongoing;
        }

        /* Constructor without weapon and reason
         * Used when creating a new Kill object with the 'CustomWeapons' rule during gameplay */
        public TargetAssignment(Player hunter, Player target, string? weapon) : this (hunter, target)
        {
            _weapon = weapon;
        }

        /* Full constructor
         * Used when retrieving TargetAssignment data from the database */
        public TargetAssignment(Guid id, Guid hunterId, Player hunter, Guid targetId, Player target, DateTime targetAssigned, Kill? kill, string? weapon, AssignmentStatus assignmentStatus)
        {
            _id = id;
            _hunterId = hunterId;
            _hunter = hunter;
            _targetId = targetId;
            _target = target;
            _targetAssigned = targetAssigned;
            Kill = kill;
            _weapon = weapon;
            AssignmentStatus = assignmentStatus;
        }

        public Guid Id
        {
            get { return _id; }
        }

        public Guid HunterId
        {
            get { return _hunterId; }
        }

        public Player Hunter
        {
            get { return _hunter; }
        }

        public Guid TargetId
        {
            get { return _targetId; }
        }

        public Player Target
        {
            get { return _target; }
        }

        public DateTime TargetAssigned
        {
            get { return _targetAssigned; }
        }

        public Kill? Kill
        {
            get { return kill; }
            set { kill = value; }
        }

        public string? Weapon
        {
            get { return _weapon; }
        }

        public AssignmentStatus AssignmentStatus
        {
            get { return assignmentStatus; }
            set { assignmentStatus = value; }
        }
    }
}
