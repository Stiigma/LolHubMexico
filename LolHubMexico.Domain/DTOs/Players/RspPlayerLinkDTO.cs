using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.DTOs.Players
{
    public class RspPlayerLinkDTO
    {
        public int IdUser { get; set; }

        public int IdPlayer { get; set; }
        public string SummonerName { get; set; }

        public string Region { get; set; }

        public bool Create { get; set; } = false;
    }
}
