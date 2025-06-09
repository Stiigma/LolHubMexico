using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.DTOs.Scrims
{
    public class DetailScrimDTO
    {
        public int idDetailsScrim { get; set; }


        public int idTeam { get; set; }

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
