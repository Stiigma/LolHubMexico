using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.DTOs.Players
{
    public class RiotSummonerDTO
    {
        public string Puuid { get; set; }
        public string SummonerId { get; set; }

        public string AccountId { get; set; }
        // public string SummonerName { get; set; }
        public int SummonerLevel { get; set; }
        public int ProfileIconId { get; set; }
        //public string Region { get; set; }
    }
}
