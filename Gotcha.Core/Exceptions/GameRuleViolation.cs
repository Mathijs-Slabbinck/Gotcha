using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gotcha.Core.Exceptions
{
    public class GameRuleViolationException : GotchaException
    {
        public string RuleName { get; }
        public GameRuleViolationException(string ruleName, string message)
            : base(message)
        {
            RuleName = ruleName;
        }
    }
}
