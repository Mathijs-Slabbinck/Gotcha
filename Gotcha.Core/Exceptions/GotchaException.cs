using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gotcha.Core.Exceptions
{
    public class GotchaException : Exception
    {
        public GotchaException(string message) : base(message) { }
        public GotchaException(string message, Exception inner) : base(message, inner) { }
    }
}
