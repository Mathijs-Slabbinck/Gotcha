using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gotcha.Core.Entities.Logging
{
    // This entity keeps track of data by people who have likely tried to bypass security (but failed cuz I'm great)
    public class Attacker
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string? IpAdress { get; set; }
        public string? UserAgent { get; set; }
        // page they came from
        public string? Referer { get; set; }
        public DateTime TimeStamp { get; set; }
        // target EndPoint
        public string Path { get; set; } = string.Empty;
        public string? InvalidInput { get; set; }
        public string? SessionId { get; set; }
        // in case the attacker was logged in (and they are absolute morons)
        public string? MacAdress { get; set; }
        public Guid UserId { get; set; } = Guid.Empty;
    }
}
