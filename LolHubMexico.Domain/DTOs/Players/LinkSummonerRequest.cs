using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.DTOs.Players
{
    public class LinkSummonerRequest
    {
        public int UserId { get; set; }
        public string SummonerName { get; set; }
       // public string Region { get; set; }

        public string MainRole { get; set; }
    }

}
