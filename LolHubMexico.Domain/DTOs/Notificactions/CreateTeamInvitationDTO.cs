using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.DTOs.Notificactions
{
    public class CreateTeamInvitationDTO
    {
        public int IdTeam { get; set; }
        public int IdUser { get; set; }        // Usuario al que se invita
        public int InvitedBy { get; set; }     // Capitán o quien invita
        public string? Message { get; set; }
    }

}
