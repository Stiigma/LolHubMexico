using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.DTOs.Players
{
    public class PlayerDTO
    {
        public int IdPlayer { get; set; }
        public int IdUser { get; set; }

        public string MainRole { get; set; }

        public string SummonerName { get; set; }

        public int Level { get; set; }

        public string ProfilePicture { get; set; }

        public string Puuid { get; set; }

    }
}
