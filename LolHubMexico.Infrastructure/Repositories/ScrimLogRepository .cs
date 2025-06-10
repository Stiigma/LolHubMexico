using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google;
using LolHubMexico.Domain.Entities.ScrimLog;
using LolHubMexico.Domain.Repositories;
using LolHubMexico.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LolHubMexico.Infrastructure.Repositories
{
    public class ScrimLogRepository : IScrimLogRepository
    {
        private readonly ContextDB _context;

        public ScrimLogRepository(ContextDB context)
        {
            _context = context;
        }

        public async Task AddScrimLogAsync(ScrimLog scrimLog)
        {
            if (scrimLog == null)
            {
                throw new ArgumentNullException(nameof(scrimLog));
            }
            _context.ScrimLogs.Add(scrimLog); // Añade la entidad al DbSet
            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos
        }

        
        public async Task<ScrimLog?> GetScrimLogByIdAsync(int idLogScrim)
        {
          
            return await _context.ScrimLogs.FindAsync(idLogScrim);
        }

       
        public async Task<ScrimLog?> GetScrimLogsByIdScrimAsync(int idScrim)
        {
            // Usa LINQ para filtrar y ToListAsync para ejecutar la consulta de forma asíncrona
            return await _context.ScrimLogs.
                                 FirstOrDefaultAsync(sl => sl.IdScrim == idScrim);

        }

     
        public async Task<ScrimLog?> GetScrimLogByMatchIdAsync(string matchId)
        {
            // Usa LINQ para encontrar el primero que coincida, o null si no hay ninguno
            return await _context.ScrimLogs
                                 .FirstOrDefaultAsync(sl => sl.MatchId == matchId);
        }

      
        public async Task UpdateScrimLogAsync(ScrimLog scrimLog)
        {
            if (scrimLog == null)
            {
                throw new ArgumentNullException(nameof(scrimLog));
            }

           
            _context.Entry(scrimLog).State = EntityState.Modified; // Marca la entidad como modificada
            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos
        }

        public async Task DeleteScrimLogAsync(int idLogScrim)
        {
            var scrimLogToDelete = await _context.ScrimLogs.FindAsync(idLogScrim); // Encuentra la entidad
            if (scrimLogToDelete != null)
            {
                _context.ScrimLogs.Remove(scrimLogToDelete); // Marca la entidad para eliminación
                await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos
            }
          
        }
    }
}
