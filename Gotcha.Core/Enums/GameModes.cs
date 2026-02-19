using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gotcha.Core.Enums
{
    public enum GameModes
    {
        Gotcha, // standard Gotcha
        Assassin, // players can kill people who they suspect are their killer. If they are right the killer dies, if they are wrong they die
    }
}
