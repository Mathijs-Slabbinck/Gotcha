using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gotcha.Core.Exceptions
{
    public class GotchaInvalidOperationsExceptions : GotchaException
    {
        public GotchaInvalidOperationsExceptions(string message) : base(message) { }
        public GotchaInvalidOperationsExceptions(string message, Exception inner) : base(message, inner) { }
    }
}
