using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gotcha.Core.Enums;

namespace Gotcha.Core.Entities
{
    public class Rules
    {
        public bool CustomWeapons { get; set; } = false;
        public bool RandomTargetAssignment { get; set; } = false;
        public GameModes GameModes { get; set; }
    }
}
