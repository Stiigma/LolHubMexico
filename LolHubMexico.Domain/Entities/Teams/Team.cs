using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.Entities.Teams
{
    public class Team
    {
        public int IdTeam { get; set; }
        public string TeamName { get; set; }
        public string TeamLogo { get; set; }

        public DateTime CreationDate { get; set; }

        public int IdCapitan {  get; set; }

        public int StatusTeam {  get; set; }

        public string DescripcionTeam { get; set; }
    }
}
