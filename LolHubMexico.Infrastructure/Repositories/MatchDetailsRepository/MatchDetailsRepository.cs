using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Google;
using LolHubMexico.Domain.Entities.MatchDetails;
using LolHubMexico.Domain.Repositories.MatchRepository;
using LolHubMexico.Infrastructure.Data;

namespace LolHubMexico.Infrastructure.Repositories.MatchDetailsRepository
{
    public class MatchDetailsRepository : IMatchDetailsRepository
    {
        private readonly ContextDB _context;

        public MatchDetailsRepository(ContextDB context)
        {
            _context = context;
        }

        public async Task<MatchDetail> CreateAsync(MatchDetail matchDetails)
        {
            _context.MatchDetails.Add(matchDetails);
            await _context.SaveChangesAsync();
            return matchDetails;
        }

        public async Task<MatchDetail?> GetByScrimIdAsync(int idScrim)
        {
            return await _context.MatchDetails
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.IdScrim == idScrim);
        }

        public async Task<MatchDetail> UpdateAsync(MatchDetail matchDetails)
        {
            var existing = await _context.MatchDetails
                .FirstOrDefaultAsync(m => m.IdScrim == matchDetails.IdScrim);

            if (existing == null)
                throw new InvalidOperationException("No se encontró MatchDetails para actualizar.");

            existing.GameDuration = matchDetails.GameDuration;
            existing.GameMode = matchDetails.GameMode;
            existing.GameVersion = matchDetails.GameVersion;
            existing.TowersTeam1 = matchDetails.TowersTeam1;
            existing.TowersTeam2 = matchDetails.TowersTeam2;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteByScrimIdAsync(int idScrim)
        {
            var existing = await _context.MatchDetails
                .FirstOrDefaultAsync(m => m.IdScrim == idScrim);

            if (existing == null)
                return false;

            _context.MatchDetails.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
