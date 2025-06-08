using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.Entities.DatailsScrims
{
    public class DetailsScrim
    {
        [Key]
        public int idDetailsScrim { get; set; }

        public int idScrim { get; set; }

        public int idPlayer { get; set; }
        public int idUser { get; set; }

        public int idTeam {  get; set; }

        public string puuid { get; set; }

        public string idMatch { get; set; } = string.Empty;


        public string carril { get; set; } = string.Empty;

        public string teamDamagePercentage { get; set; } = string.Empty;

        public int kills { get; set; } = 0;

        public int deaths { get; set; } = 0;

        public int assists { get; set; } = 0;

        public int goldEarned { get; set; } = 0;

        public int farm { get; set; } = 0;

        public int visionScore { get; set; } = 0;


        public string championName { get; set; } = string.Empty;

        public int nivel { get; set; } = 0;

        public string items { get; set; } = string.Empty;
    }
}
