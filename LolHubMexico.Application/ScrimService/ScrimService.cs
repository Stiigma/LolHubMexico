using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Application;
using LolHubMexico.Application.Exceptions;
using LolHubMexico.Domain.DTOs.Scrims;
using LolHubMexico.Domain.Entities.Scrims;
using LolHubMexico.Domain.Repositories.ScrimRepository;

namespace LolHubMexico.Application.ScrimService
{
    public class ScrimService
    {
        private readonly IScrimRepository _scrimRepository;
        private readonly TeamService _teamService;
        public ScrimService(IScrimRepository scrimRepository, TeamService scrimService ) { 
            _scrimRepository = scrimRepository;
            _teamService = scrimService;
        }

        public async Task<bool> CreateScrim(CreateScrimDTO newDto)
        {
            if(newDto == null) return false;

            if(!IsValidScheduledDate(newDto.scheduled_date))
                throw new AppException("La Fecha es invalida'");

            bool IsCapitan = await _teamService.IsCapitan(newDto.created_by);

            if(!IsCapitan)
                throw new AppException("No es capitan de Equipo");

            var team = await _teamService.GetTeamByUserId(newDto.created_by);

            var isComplete = await _teamService.TeamMemberComplete(team.IdTeam);

            if(!isComplete)
                throw new AppException("El equipo debe de tener 5 miembros");



            if (newDto.created_by != team.IdCapitan)
                throw new AppException("No es capitan de Equipo");


            if(newDto.idTeam2 == 0)
            {
                var newScrim = new Scrim
                {
                    idTeam1 = team.IdTeam,
                    scheduled_date = newDto.scheduled_date,
                    created_by = newDto.created_by,
                    created_at = DateTime.Now,
                    status = 0
                };

                await _scrimRepository.CreateScrim(newScrim);

            }
            else
            {

                var IsCompleteRival = await _teamService.TeamMemberComplete(newDto.idTeam2);

                if (!IsCompleteRival)
                    throw new AppException("Team Rival no completo");

                var newScrim = new Scrim
                {
                    idTeam1 = team.IdTeam,
                    idTeam2 = newDto.idTeam2,
                    scheduled_date = newDto.scheduled_date,
                    created_by = newDto.created_by,
                    created_at = DateTime.Now,
                    status = 1
                };

                await _scrimRepository.CreateScrim(newScrim);
            }

            return true;
        }

        public async Task<bool> AcceptScrim(RivalDTO rivalDTO)
        {
            if(rivalDTO == null) return false;

            if (!rivalDTO.IsAccept)
                return false;

            bool IsCapitan = await _teamService.IsCapitan(rivalDTO.idrival);

            if (!IsCapitan)
                throw new AppException("No es capitan de Equipo");



            var team = await _teamService.GetTeamByUserId(rivalDTO.idrival);


            var isComplete = await _teamService.TeamMemberComplete(team.IdTeam);

            if (!isComplete)
                throw new AppException("El equipo debe de tener 5 miembros");

            var scrim = await _scrimRepository.GetScrimById(rivalDTO.idScrim);

            if(scrim == null)
                throw new AppException("La Scrim no existe");


            scrim.idTeam2 = team.IdTeam;
            scrim.status = 1;



            return true;

        }

        public async Task<List<ScrimPDTO>> GetScrimsPending()
        {
            var lstScrims = await _scrimRepository.GetAllScrims();

            var pendingScrims = lstScrims
                .Where(s => s.status == 0)
                .Select(s => new ScrimPDTO
                {
                    idScrim = s.idScrim,
                    idTeam1 = s.idTeam1,
                    scheduled_date = s.scheduled_date,
                    status = s.status
                })
                .ToList();

            return pendingScrims;
        }

        public async Task<List<ScrimPDTO>> GetScrimsByIdUser(int idUser)
        {
            var teamMember = await _teamService.GetTeamByUserId(idUser);

            var scrimsTeam1 = await _scrimRepository.GetScrimsByTeam1(teamMember.IdTeam);
            var scrimsTeam2 = await _scrimRepository.GetScrimsByTeam2(teamMember.IdTeam);

            var allScrims = scrimsTeam1
                .Concat(scrimsTeam2)
                //.Where(s => s.Status == 0) // opcional si ya lo filtras en el repo
                .DistinctBy(s => s.idScrim) // evitar duplicados si aplica
                .ToList();

            var scrimDTOs = allScrims.Select(s => new ScrimPDTO
            {
                idScrim = s.idScrim,
                idTeam1 = s.idTeam1,
                idTeam2 = s.idTeam2,
                scheduled_date = s.scheduled_date,
                status = s.status
            }).ToList();

            return scrimDTOs;
        }


        private bool IsValidScheduledDate(DateTime scheduledDate)
        {
            var now = DateTime.Now;

            // Verificar que no sea de un día anterior
            if (scheduledDate.Date < now.Date)
                return false;

            // Verificar que sea al menos 10 minutos después de ahora
            if (scheduledDate < now.AddMinutes(10))
                return false;

            return true;
        }
    }
}
