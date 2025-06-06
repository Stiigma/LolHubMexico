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
using LolHubMexico.Domain.Enums.TeamInvitation;

namespace LolHubMexico.Application
{
    public class TeamService 
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITeamInvitationRepository _teamInvitationRepository;

        public TeamService(ITeamRepository teamRepository, IUserRepository userRepository, ITeamInvitationRepository teamInvitationRepository)
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository;
            _teamInvitationRepository = teamInvitationRepository;
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

        public async Task<bool> IsUserWithTeam(int idUser)
        {
            if (idUser == null)
                throw new AppException("No se aceptan nulos");

            bool response = await _teamRepository.IsUserInAnyTeam(idUser);

            return response;
        }


        public async Task<Team> GetTeamByUserId(int? idUser)
        {
            if(idUser == null)
                throw new AppException("523");

            


            if (await _teamRepository.IsUserInAnyTeam((int)idUser))
            {
                var team = await _teamRepository.GetTeamByIdUser((int)idUser);
                if(team.Status != -1)
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

        public async Task<bool> JoinTeam(JoinTeamDTO dto)
        {
            if(dto == null)
                throw new AppException("dto vacio");

            var invitation = await _teamInvitationRepository.GetByIdAsync(dto.IdInvitation);
            if (invitation == null)
                throw new AppException("No Existe esta invitacion");
            if (!dto.response)
            {
                invitation.Status = (int)StatusInvTeam.Negative;
                return false;
            }
                

            TeamMember teamMember = new TeamMember
            {
                IdTeam = dto.IdTeam,
                IdUser = dto.IdUser,
                Join_date = DateTime.Now,
                Status = 1,
                Role = "Flex-Rol",
            };

            await _teamRepository.AddMember(teamMember);
            invitation.Status = (int)StatusInvTeam.Accept;

            await _teamInvitationRepository.UpdateAsync(invitation);

            return true;

        }


        public async Task<bool> IsCapitan(int IdCapitan)
        {
            if (IdCapitan == null)
                throw new AppException("Id Vacio");

            bool response = await _teamRepository.ExistsCapitanAsync(IdCapitan);

            return response;
        }

        public async Task<List<TeamMember>> GetAllTeam(int idTeam)
        {
            var lstTeamMembers = await _teamRepository.GetAllTeam(idTeam);

            if(lstTeamMembers == null)
                throw new AppException("No hay miembros de este id Team");

            if( lstTeamMembers.Count == 0)
                throw new AppException("No hay miembros de este id Team");

            return lstTeamMembers;
        }

        public async Task<bool> TeamMemberComplete(int IdTeam)
        {
            var lstTeamMembers = await GetAllTeam(IdTeam);

            
            if(lstTeamMembers.Count < 5)
                return false;

            return true;
        }

        public async Task<List<TeamMember>> GetTeamComplete(int idTeam)
        {
            var lstTeamMembers = await GetAllTeam(idTeam);

            if (lstTeamMembers.Count <= 0)
                throw new AppException("Este Team no tiene Integrantes");

            return lstTeamMembers;
        }

        public async Task<Team> Update(Team updateTeam)
        {
            //if (await _teamRepository.IsExistTeamName(updateTeam.TeamName))
            //    throw new AppException("Nombre de equipo ya en uso");

            var newTeamUpdate = await _teamRepository.UpdateTeam(updateTeam);

            return newTeamUpdate;

        }
    }
}
