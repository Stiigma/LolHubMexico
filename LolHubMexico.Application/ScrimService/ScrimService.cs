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

            bool IsCapitan = await _teamService.IsCapitan(newDto.created_by);

            if(!IsCapitan)
                throw new AppException("No es capitan de Equipo");

            var team = await _teamService.GetTeamByUserId(newDto.created_by);

            if(newDto.created_by != team.IdCapitan)
                throw new AppException("No es capitan de Equipo");

            var newScrim = new Scrim
            {
                idTeam1 = team.IdTeam,
                scheduled_date = newDto.scheduled_date,
                created_by = newDto.created_by,
                created_at = DateTime.Now,
                status = 0
            };

            await _scrimRepository.CreateScrim(newScrim);



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

            var scrim = await _scrimRepository.GetScrimById(rivalDTO.idScrim);

            if(scrim == null)
                throw new AppException("La Scrim no existe");


            scrim.idTeam2 = team.IdTeam;
            scrim.status = 1;



            return true;

        }
    }
}
