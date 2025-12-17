using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gotcha.Core.Exceptions
{
    public class PlayerNotFoundException : GotchaException
    {
        public Guid PlayerId { get; }
        public PlayerNotFoundException(Guid playerId)
            : base($"Player with ID {playerId} not found.")
        {
            PlayerId = playerId;
        }
    }
}
