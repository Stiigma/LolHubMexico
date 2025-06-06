using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.Entities.Players;
using LolHubMexico.Domain.Repositories.PlayerRepository;
using LolHubMexico.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LolHubMexico.Infrastructure.Repositories.PlayerRepository
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly ContextDB _context;
        public PlayerRepository(ContextDB context)
        {
            _context = context;
        }

        public async Task<Player> CreatePlayer(Player player)
        {
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            return player;
        }

        public async  Task<Player?> GetPlayerByIdUser(int idUser)
        {
            return await _context.Players.FirstOrDefaultAsync(p => p.IdUser == idUser);

            

        }
    }
}
