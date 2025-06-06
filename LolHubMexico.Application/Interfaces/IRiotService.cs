using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.DTOs.Players;

namespace LolHubMexico.Application.Interfaces
{
    public interface IRiotService
    {
        Task<RiotAccountDTO> GetSummonerByNameAsync(string region, string summonerName);

        Task<RiotSummonerDTO> GetSummonerByPuiid(string puiid);
    }
}

