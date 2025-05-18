using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.Repositories.TeamRepository;
using LolHubMexico.Domain.DTOs.Teams;
using LolHubMexico.Application.Exceptions;
using LolHubMexico.Domain.Entities.Teams;
using LolHubMexico.Domain.Repositories.UserRepository;

namespace LolHubMexico.Application.TeamService
{
    public class TeamService 
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUserRepository _userRepository;

        public TeamService(ITeamRepository teamRepository, IUserRepository userRepository)
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository;
        }

        public async Task<Team> CreateTeamAsync(CreateTeamDTO newTeam)
        {
            if (newTeam == null)
                throw new AppException("team null");

            if(await _teamRepository.IsExistTeamName(newTeam.TeamName))
                throw new AppException("Nombre de equipo ya en uso");

            if (await _teamRepository.IsUserInAnyTeam(newTeam.IdCapitan))
                throw new AppException("El capitán ya pertenece a un equipo.");

            Team newTeamInsert = new Team
            {
                TeamName = newTeam.TeamName,
                IdCapitan = newTeam.IdCapitan,
                Status = 1,
                CreationDate = DateTime.Now
            };

            


            var createdTeam = await _teamRepository.CreateTeamAsync(newTeamInsert);

            TeamMember teamMember = new TeamMember
            {
                IdTeam = createdTeam.IdTeam,
                IdUser = newTeam.IdCapitan,
                Join_date = DateTime.Now,
                Status = 1,
                Role = "Flex-Rol",
            };

            await _teamRepository.AddMember(teamMember);

            return createdTeam;
        }


        public async Task<Team> GetTeamByUserId(int? idUser)
        {
            if(idUser == null)
                throw new AppException("523");

            


            if (await _teamRepository.IsUserInAnyTeam((int)idUser))
            {
                var team = await _teamRepository.GetTeamByIdUser((int)idUser);
                if(team != null)
                    return team;

                var MemberT = await _teamRepository.GetTeamMemberByIdUser((int)idUser);



                var tryTeam = await _teamRepository.GetTeamById(MemberT!.IdTeam);
                if(tryTeam == null)
                    throw new AppException("No tienes equipo");

                return tryTeam;
            }
            else
            {
                throw new AppException("El capitán ya pertenece a un equipo.");
            }
                


        }
    }
}
