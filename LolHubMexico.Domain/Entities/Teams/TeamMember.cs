using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.Entities.Teams
{
    public class TeamMember
    {
        [Key]
        public int IdTeamMembers { get; set; }

        public int IdTeam {  get; set; }

        public int IdUser { get; set; }

        public DateTime Join_date { get; set; }

        public int Status {  get; set; }

        public string Role { get; set; }
    }
}
