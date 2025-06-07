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

        public string puuid { get; set; }

        public string idMatch { get; set; }


        public string carril { get; set; }

        public int totalDamageDealt { get; set; }

        public int kills { get; set; }

        public int deaths { get; set; }

        public int assists { get; set; }

        public int goldEarned { get; set; }

        public int farm { get; set; }

        public int visionScore { get; set; }


        public string championName { get; set; }

        public int nivel { get; set; }

        public string items { get; set; }
    }
}
