using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.Entities.Players;

namespace LolHubMexico.Domain.Repositories.PlayerRepository
{
    public interface IPlayerRepository
    {
        Task<Player> CreatePlayer(Player player);
        public Task<Player?> GetPlayerByIdUser(int idUser);
    }
}
