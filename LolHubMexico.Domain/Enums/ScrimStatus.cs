using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.Enums
{
    public enum ScrimStatus
    {
        Open = 0,           // Scrim is open and waiting for invitations
        Pending = 1,        // Invitation sent, waiting for confirmation
        Confirmed = 2,      // Both teams have confirmed
        InProgress = 3,     // Match has started
        Completed = 4,      // Match has been processed
        Cancelled = -1      // Match was cancelled (timeout or error)
    }
}
