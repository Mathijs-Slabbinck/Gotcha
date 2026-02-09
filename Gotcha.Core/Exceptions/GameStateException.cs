using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gotcha.Core.Exceptions
{
    public class GameStateException : GotchaException
    {
        public GameStateException(string message) : base(message) { }
        public GameStateException(string message, Exception inner) : base(message, inner) { }
    }
}
