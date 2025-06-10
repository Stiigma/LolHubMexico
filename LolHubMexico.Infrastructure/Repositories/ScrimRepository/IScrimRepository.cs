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

        public async Task<List<Scrim>> GetScrimsPorEstadoAsync(int status)
        {
            return await _context.Scrims
                .Where(s => s.status == status)
                .ToListAsync();
        }
        public async Task<Scrim?> UpdateScrim(Scrim scrim)
        {
            var existingScrim = await _context.Scrims.FirstOrDefaultAsync(s => s.idScrim == scrim.idScrim);
            if (existingScrim == null)
                return null;

            // Campos que siempre deben actualizarse
            existingScrim.idTeam1 = scrim.idTeam1;
            existingScrim.idTeam2 = scrim.idTeam2;
            existingScrim.status = scrim.status;
            existingScrim.scheduled_date = scrim.scheduled_date;
            existingScrim.created_at = scrim.created_at;
            existingScrim.description = scrim.description;

            // Campos opcionales que pueden ser null
            existingScrim.team1_result_reported = scrim.team1_result_reported;
            existingScrim.team2_result_reported = scrim.team2_result_reported;
            existingScrim.team1_reported_at = scrim.team1_reported_at;
            existingScrim.team2_reported_at = scrim.team2_reported_at;
            existingScrim.result_verification = scrim.result_verification;
            existingScrim.imagePath1 = scrim.imagePath1;
            existingScrim.imagePath2 = scrim.imagePath2;
            existingScrim.idMatch1 = scrim.idMatch1;
            existingScrim.idMatch2 = scrim.idMatch2;

            await _context.SaveChangesAsync();
            return existingScrim;
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
                .Where(s => s.idTeam1 == idTeam)
                .ToListAsync();
        }

        public async Task<List<Scrim>> GetScrimsByTeam2(int idTeam)
        {
            return await _context.Scrims
                .Where(s => s.idTeam2 == idTeam)
                .ToListAsync();
        }

        public async Task DeleteScrim(Scrim scrim)
        {
            _context.Scrims.Remove(scrim);
            await _context.SaveChangesAsync();

        }
    }
}
