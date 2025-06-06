using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.Entities.Scrims;
using LolHubMexico.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LolHubMexico.Domain.Repositories.ScrimRepository
{
    public class ScrimRepository : IScrimRepository
    {
        private readonly ContextDB _context;
        public ScrimRepository(ContextDB context) {
            _context = context;
        }

        public async Task<Scrim> CreateScrim(Scrim scrim)
        {
            _context.Scrims.Add(scrim);
            await _context.SaveChangesAsync();
            return scrim;
        }


        public async Task<Scrim?> GetScrimById(int idScrim)
        {
            var scrim = await _context.Scrims.FirstOrDefaultAsync(u => u.idScrim == idScrim);

            return scrim;
        }

        public async Task<Scrim> UpdateScrim(Scrim scrim)
        {
            _context.Scrims.Update(scrim);
            await _context.SaveChangesAsync();
            return scrim;
        }

        public async Task<List<Scrim>> GetAllScrims()
        {

            var lstScrims = new List<Scrim>();
            lstScrims = await _context.Scrims.ToListAsync();

            return lstScrims;
        }

        public async Task<List<Scrim>> GetScrimsByTeam1(int idTeam)
        {
            return await _context.Scrims
                .Where(s => s.idTeam1 == idTeam && s.status != 2)
                .ToListAsync();
        }

        public async Task<List<Scrim>> GetScrimsByTeam2(int idTeam)
        {
            return await _context.Scrims
                .Where(s => s.idTeam2 == idTeam && s.status != 2)
                .ToListAsync();
        }
    }
}
