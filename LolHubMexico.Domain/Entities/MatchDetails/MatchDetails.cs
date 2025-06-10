using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.Entities.MatchDetails
{
    public class MatchDetail
    {
        [Key]
        public int IdMatchDetails { get; set; }      // Clave primaria
        public int IdScrim { get; set; }             // Relación con la Scrim

        public int GameDuration { get; set; }        // En segundos
        public string GameMode { get; set; } = string.Empty;
        public string GameVersion { get; set; } = string.Empty;

        public int TowersTeam1 { get; set; }
        public int TowersTeam2 { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
