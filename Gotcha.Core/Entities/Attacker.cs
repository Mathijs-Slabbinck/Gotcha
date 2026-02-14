using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gotcha.Core.Entities
{
    // This entity keeps track of data by people who have likely tried to bypass security (but failed cuz I'm great)
    public class Attacker
    {
        public int Id { get; set; }
        public string? IpAdress { get; set; }
        public string? UserAgent { get; set; }
        // page they came from (tried the attack on)
        public string? Referer { get; set; }
        public DateTime TimeStamp { get; set; }
        // target EndPoint
        public string Path { get; set; } = string.Empty;
        public string InvalidInput { get; set; }
        public string? SessionId { get; set; }
        // in case the attacker was logged in (and they are absolute morons)
        public Guid UserId { get; set; } = new Guid();
    }
}
