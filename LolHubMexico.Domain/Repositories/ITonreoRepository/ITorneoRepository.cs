using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.Entities.Torneos;

namespace LolHubMexico.Domain.Repositories.ITonreoRepository
{
    public interface ITorneoRepository
    {
        Task<Torneo> CrearTorneoAsync(Torneo torneo);
        Task<Torneo?> ObtenerTorneoPorIdAsync(int idTorneo);

        Task<List<Torneo>?> GetTorneosConEstado(int status);
        Task<bool> EditarTorneoAsync(Torneo torneo);
        Task<bool> EliminarTorneoAsync(int idTorneo);

        Task<TorneoScrim> CreateTorneoScrimAsync(TorneoScrim torneoScrim);

        // Gets a specific association by its primary key
        Task<TorneoScrim?> GetTorneoScrimByIdAsync(int idTorneoScrim);

        // Gets all scrims associated with a specific tournament
        Task<List<TorneoScrim>> GetTorneoScrimsByTorneoIdAsync(int idTorneo);

        // Gets the tournament association for a specific scrim
        Task<TorneoScrim?> GetTorneoScrimByScrimIdAsync(int idScrim);

        // Deletes an association
        Task<bool> DeleteTorneoScrimAsync(int idTorneoScrim);

        Task<TorneoEquipo> CreateTorneoEquipoAsync(TorneoEquipo torneoEquipo);
        Task<TorneoEquipo?> GetTorneoEquipoByIdAsync(int idTorneEquipo);
        Task<List<TorneoEquipo>> GetTorneoEquiposByTorneoIdAsync(int idTorneo);
        Task<TorneoEquipo?> GetTorneoEquipoByEquipoIdAsync(int idEquipo);
        Task<bool> DeleteTorneoEquipoAsync(int idTorneEquipo);

        Task<List<TorneoScrim>> ScrimsActivasByID(int idScrim);

        Task<List<TorneoEquipo>> EquipoActivasByID(int idteam);

        Task<List<Torneo>> TomarTorneoEstado(int idTorneom, int status);


        Task<List<TorneoEquipo>> TorneoEquipoByIdTeam(int idteam);
    }
}
