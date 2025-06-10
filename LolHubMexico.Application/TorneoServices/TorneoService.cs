using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Application.Interfaces;
using LolHubMexico.Domain.Entities.Torneos;
using LolHubMexico.Domain.Repositories.ITonreoRepository;
using LolHubMexico.Domain.Repositories.UserRepository;
using LolHubMexico.Application.Exceptions;
using LolHubMexico.Domain.DTOs.Torneo;
using LolHubMexico.Domain.Repositories.TeamRepository;
using Microsoft.VisualBasic;
using LolHubMexico.Domain.Entities.Scrims;
using LolHubMexico.Domain.Repositories.ScrimRepository;

namespace LolHubMexico.Application.TorneoServices
{
    public class TorneoService : ITorneoService
    {
        private readonly ITorneoRepository _torneoRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IScrimRepository _scrimRepository;

        public TorneoService(ITorneoRepository torneoRepository, IUserRepository userRepository, ITeamRepository teamRepository, IScrimRepository scrimRepository )
        {
            _torneoRepository = torneoRepository;
            _userRepository = userRepository;
            _teamRepository = teamRepository;
            _scrimRepository = scrimRepository;
        }

        public async Task<TorneoDTO> CrearTorneoAsync(TorneoDTO torneo)
        {
            var ensenadaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var nowInEnsenada = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, ensenadaTimeZone);
            // Validaciones de negocio básicas (puedes expandirlas)
            if (string.IsNullOrWhiteSpace(torneo.Nombre))
                throw new ArgumentException("El nombre del torneo no puede estar vacío.");

            if (torneo.FechaInicio > torneo.FechaFin)
                throw new ArgumentException("La fecha de inicio no puede ser posterior a la fecha de fin.");

            var user = await _userRepository.GetUserById(torneo.IdCreador);

            if (user == null)
                throw new AppException("El usuario no existe");

            if(user.Role != 0)
                throw new AppException("El usuario no es administrador");


            var newTorneo = new Torneo
            {
                IdCreador = torneo.IdCreador,
                FechaInicio = torneo.FechaInicio,
                Descripcion = torneo.Descripcion,
                Estado = 0,
                FechaCreacion = nowInEnsenada,
                FechaFin = torneo.FechaFin,
                Nombre = torneo.Nombre,

            };
            await _torneoRepository.CrearTorneoAsync(newTorneo);
            
            return torneo;
        }

        public async Task<List<Torneo>> getTodosTorneoEstado(int status)
        {
            var lst = await  _torneoRepository.GetTorneosConEstado(status);

            if(lst == null)
                return new List<Torneo>();

            return lst;
        }

        public async Task<List<Torneo>> TomarMisTorneos(int miTeam)
        {
            var TeamEquipo = await _torneoRepository.TorneoEquipoByIdTeam(miTeam);

            if(TeamEquipo == null)
                return new List<Torneo>();

            var lts = new List<Torneo>();
            foreach(var t in TeamEquipo)
            {
                var torneo = await _torneoRepository.ObtenerTorneoPorIdAsync(t.IdTorneo);
                if(torneo == null)
                    continue;
                lts.Add(torneo);
            }

            return lts;
        }

        public async Task<Torneo> TomarTorneoPorId(int idTorneo)
        {
            var torneo = await _torneoRepository.ObtenerTorneoPorIdAsync(idTorneo);

            if(torneo == null)
                throw new AppException("No existe ese torneo");

            return torneo;

        }

        public async Task<TorneoEquipo> UnirseATorneoAsync(int idTorneo, int idEquipo, int idUser)
        {
            // Verificar si ya existe la relación
            var torneo = await _torneoRepository.ObtenerTorneoPorIdAsync(idTorneo);

            if (torneo != null)
                throw new AppException("torneo no existente");

            if(torneo.Estado != 0)
                throw new AppException("El Torneo ya esta en proceso");

            var lstJugadores = await _torneoRepository.GetTorneoEquiposByTorneoIdAsync(idTorneo);

            if(torneo.Estado != 1)
            {
                if (lstJugadores.Count > 4)
                {
                    torneo.Estado = 1;
                    await _torneoRepository.EditarTorneoAsync(torneo);
                    throw new AppException("Torneo LLeno");
                }
            }                   

            var team = await _teamRepository.GetTeamById(idTorneo);

            if(team != null)
                throw new AppException("Team no existente");

            if (idUser != team.IdCapitan)
                throw new AppException("Solo capitanes pueden Inscribirse a torneos");


            var TorneosInscritosIds = await _torneoRepository.EquipoActivasByID(idEquipo);

            if (TorneosInscritosIds.Count > 4)
                throw new AppException("Solo puedes estar inscrito en 4 torneos a la vez");

            foreach (var inscrito in TorneosInscritosIds)
            {
                var torneoInscrito = await _torneoRepository.ObtenerTorneoPorIdAsync(inscrito.IdTorneo);
                if (torneoInscrito == null) continue;

                bool hayConflicto = torneo.FechaInicio < torneoInscrito.FechaFin &&
                                    torneo.FechaFin > torneoInscrito.FechaInicio;

                if (hayConflicto)                
                    throw new AppException("Ya estás inscrito en un torneo que se cruza en horario.");
                
            }



            // Crear relación
            var nuevaRelacion = new TorneoEquipo
            {
                IdTorneo = idTorneo,
                IdEquipo = idEquipo,
                status = true
            };

            return await _torneoRepository.CreateTorneoEquipoAsync(nuevaRelacion);
        }


        public async Task<Scrim> CreacionScrimOnly(int idTorneo,Scrim scrim)
        {

            if (scrim == null)
                throw new AppException("Es null");

            var newScrim = await _scrimRepository.CreateScrim(scrim);

            // Crear relación
            var nuevaRelacion = new TorneoScrim
            {
                IdTorneo = idTorneo,
                IdScrim = newScrim.idScrim,
                status = true
            };

            await _torneoRepository.CreateTorneoScrimAsync(nuevaRelacion);
            return newScrim;
        }

        public async Task<List<Torneo>> TorneosConStatus(int idTorneo, int status)
        {
            var ltsList = new List<Torneo>();

            ltsList = await _torneoRepository.TomarTorneoEstado(idTorneo, status);

            return ltsList;
        }

        public async Task<Torneo> EditarTorneo(Torneo torneo)
        {
            if(torneo == null)
                throw new AppException("Torneo no existe");
            var tornecoCreated = await _torneoRepository.ObtenerTorneoPorIdAsync(torneo.IdTorneo);

            if(tornecoCreated == null)
                throw new AppException("Torneo no existe");
            tornecoCreated = torneo;

            await _torneoRepository.EditarTorneoAsync(tornecoCreated);

            return tornecoCreated;

        }

        public async Task<bool> CambiarEstado(int idTorneo, int status)
        {
            if (idTorneo == null)
                throw new AppException("Torneo no existe");
            var tornecoCreated = await _torneoRepository.ObtenerTorneoPorIdAsync(idTorneo);

            if (tornecoCreated == null)
                throw new AppException("Torneo no existe");
            tornecoCreated.Estado = status;

            return await _torneoRepository.EditarTorneoAsync(tornecoCreated);            

        }
    }
}
