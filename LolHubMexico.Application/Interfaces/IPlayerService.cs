using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.DTOs.Players;

namespace LolHubMexico.Application.Interfaces
{
    public interface IPlayerService
    {
        Task<RspPlayerLinkDTO> LinkSummonerAsync(int userId, string summonerName, string region, string MainRole);
        public Task<PlayerDTO> GetPlayerByIdUser(int idUser);
    }
}
