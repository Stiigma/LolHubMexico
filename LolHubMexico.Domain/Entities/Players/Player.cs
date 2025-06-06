using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.Entities.Players
{
    public class Player
    {
        [Key]
        public int IdPlayer { get; set; }
        public int IdUser { get; set; }

        public string SummonerName { get; set; }
        public string Region { get; set; }
        public int Level { get; set; }
       // public string Ranking { get; set; }
        public string MainRole { get; set; }
        public string ProfilePicture { get; set; }
        //public string Bio { get; set; }
        public int Status { get; set; }

        // Nuevos campos importantes
        public string Puuid { get; set; }         // Global Riot ID
        public string SummonerId { get; set; }    // Summoner internal ID
        public bool Verified { get; set; } = false;
    }
}
