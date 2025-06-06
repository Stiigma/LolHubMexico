using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.DTOs.Teams
{
    public class MemberTeamDTO
    {
        public int IdTeamMembers { get; set; }

        public int IdTeam { get; set; }
        public int IdUser { get; set; }
    }
}
