using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gotcha.Core.Exceptions
{
    public class ValidationException : GotchaException
    {
        public string FieldName { get; }
        public ValidationException(string fieldName, string message)
            : base(message)
        {
            FieldName = fieldName;
        }
    }
}
