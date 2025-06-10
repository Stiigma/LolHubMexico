using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.Entities.Torneos;
using LolHubMexico.Domain.Repositories.ITonreoRepository;
using LolHubMexico.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LolHubMexico.Infrastructure.Repositories.TonreoRepository
{
    public class TorneoRepository : ITorneoRepository
    {
        private readonly ContextDB _context; // Your DbContext instance

        public TorneoRepository(ContextDB context)
        {
            _context = context;
        }

       
        public async Task<Torneo> CrearTorneoAsync(Torneo torneo)
        {
            if (torneo == null)
            {
                throw new ArgumentNullException(nameof(torneo), "El torneo a crear no puede ser nulo.");
            }

            try
            {
                _context.Torneos.Add(torneo); 
                await _context.SaveChangesAsync(); 
                return torneo; 
            }
            catch (DbUpdateException ex)
            {
                
                Console.Error.WriteLine($"Error al intentar guardar el torneo en la base de datos: {ex.Message}");
                
                throw new Exception("Ocurrió un error al crear el torneo. Por favor, intente de nuevo.", ex);
            }
            catch (Exception ex)
            {
                
                Console.Error.WriteLine($"Error inesperado al crear el torneo: {ex.Message}");
                throw new Exception("Ocurrió un error inesperado al crear el torneo.", ex);
            }
        }

        public async Task<List<TorneoEquipo>> TorneoEquipoByIdTeam(int idTeam)
        {
            return await _context.TorneoEquipos
                .Where(te => te.IdEquipo == idTeam)
                .ToListAsync();
        }

        public async Task<TorneoScrim> CreateTorneoScrimAsync(TorneoScrim torneo)
        {
            if (torneo == null)
            {
                throw new ArgumentNullException(nameof(torneo), "El torneo a crear no puede ser nulo.");
            }

            
            
                _context.TorneoScrims.Add(torneo);
                await _context.SaveChangesAsync();
                return torneo;
          
        }

        public async Task<List<Torneo>?> GetTorneosConEstado(int status)
        {
            return await _context.Torneos
                .Where(ts => ts.Estado == status)
                .ToListAsync();
        }

        public async Task<Torneo?> ObtenerTorneoPorIdAsync(int idTorneo)
        {
            try
            {
                
                return await _context.Torneos.FindAsync(idTorneo);
                
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error al obtener el torneo con ID {idTorneo}: {ex.Message}");
                throw new Exception($"Ocurrió un error al obtener el torneo con ID {idTorneo}.", ex);
            }
        }

        public async Task<List<Torneo>> TomarTorneoEstado(int idTorneom, int status)
        {
            return await _context.Torneos.Where(t => t.IdTorneo == idTorneom && t.Estado == status).ToListAsync();
        }


        public async Task<bool> EditarTorneoAsync(Torneo torneo)
        {
            if (torneo == null)
            {
                throw new ArgumentNullException(nameof(torneo), "El torneo a editar no puede ser nulo.");
            }

            var torneoExistente = await _context.Torneos.FindAsync(torneo.IdTorneo);
            if (torneoExistente == null)
            {
                return false;
            }

            // Actualizar manualmente los campos
            torneoExistente.Nombre = torneo.Nombre;
            torneoExistente.FechaInicio = torneo.FechaInicio;
            torneoExistente.FechaFin = torneo.FechaFin;
            torneoExistente.Descripcion = torneo.Descripcion;
            torneoExistente.Estado = torneo.Estado;
            // Agrega aquí cualquier otro campo necesario...

            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }


        public async Task<bool> EliminarTorneoAsync(int idTorneo)
        {
     
                var torneoToDelete = await _context.Torneos.FindAsync(idTorneo);

                if (torneoToDelete == null)
                {
                    return false;
                }

                _context.Torneos.Remove(torneoToDelete);
                int rowsAffected = await _context.SaveChangesAsync(); 

                return rowsAffected > 0;
           
        }

        public async Task<TorneoScrim?> GetTorneoScrimByIdAsync(int idTorneoScrim)
        {
            return await _context.TorneoScrims.FindAsync(idTorneoScrim);
        }

        public async Task<List<TorneoScrim>> GetTorneoScrimsByTorneoIdAsync(int idTorneo)
        {
            return await _context.TorneoScrims
                .Where(ts => ts.IdTorneo == idTorneo)
                .ToListAsync();
        }

        public async Task<List<TorneoScrim>> ScrimsActivasByID(int idScrim)
        {
            return await _context.TorneoScrims
                .Where(ts => ts.IdScrim == idScrim && ts.status == true)
                .ToListAsync();
        }

        public async Task<List<TorneoEquipo>> EquipoActivasByID(int idteam)
        {
            return await _context.TorneoEquipos
                .Where(ts => ts.IdEquipo == idteam && ts.status == true)
                .ToListAsync();
        }

        public async Task<TorneoScrim?> GetTorneoScrimByScrimIdAsync(int idScrim)
        {
            return await _context.TorneoScrims
                .FirstOrDefaultAsync(ts => ts.IdScrim == idScrim);
        }

        public async Task<bool> DeleteTorneoScrimAsync(int idTorneoScrim)
        {
            var torneoScrim = await _context.TorneoScrims.FindAsync(idTorneoScrim);
            if (torneoScrim == null) return false;

            _context.TorneoScrims.Remove(torneoScrim);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TorneoEquipo> CreateTorneoEquipoAsync(TorneoEquipo torneoEquipo)
        {
            _context.TorneoEquipos.Add(torneoEquipo);
            await _context.SaveChangesAsync();
            return torneoEquipo;
        }

        public async Task<TorneoEquipo?> GetTorneoEquipoByIdAsync(int idTorneEquipo)
        {
            return await _context.TorneoEquipos.FindAsync(idTorneEquipo);
        }

        public async Task<List<TorneoEquipo>> GetTorneoEquiposByTorneoIdAsync(int idTorneo)
        {
            return await _context.TorneoEquipos
                .Where(te => te.IdTorneo == idTorneo)
                .ToListAsync();
        }

        public async Task<TorneoEquipo?> GetTorneoEquipoByEquipoIdAsync(int idEquipo)
        {
            return await _context.TorneoEquipos
                .FirstOrDefaultAsync(te => te.IdEquipo == idEquipo);
        }

        public async Task<bool> DeleteTorneoEquipoAsync(int idTorneEquipo)
        {
            var entity = await _context.TorneoEquipos.FindAsync(idTorneEquipo);
            if (entity == null) return false;

            _context.TorneoEquipos.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
