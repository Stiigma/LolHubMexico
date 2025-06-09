using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.DTOs.Teams
{
    public class TeamSearchDTO
    {
        public int IdTeam { get; set; }
        public string TeamName { get; set; }
        public string? TeamLogo { get; set; } // Opcional si quieres mostrar el logo
    }
}
