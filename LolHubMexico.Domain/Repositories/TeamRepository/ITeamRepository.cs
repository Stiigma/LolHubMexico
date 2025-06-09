using LolHubMexico.Domain.DTOs.Teams;
using LolHubMexico.Domain.Entities.Teams;

namespace LolHubMexico.Domain.Repositories.TeamRepository
{
    public interface  ITeamRepository
    {
        Task<Team> CreateTeamAsync(Team newTeam);

        Task<List<TeamSearchDTO>> SearchTeamsByNameAsync(string query);
        Task<Team> GetTeamByTeamName(string teamName);
        Task<Team?> GetTeamById(int IdTeam);

        Task<Team> GetTeamByIdUser(int IdUser);
        Task<TeamMember?> GetTeamMemberByIdUser(int IdUser);

        Task<List<TeamMember>> GetAllTeam(int idTeam);


        Task<bool> IsExistTeamName(string teamName);

        Task<TeamMember> AddMember(TeamMember newMember);
        Task<bool> IsUserInAnyTeam(int userId);

        Task<bool> ExistsCapitanAsync(int idCapitan);

        Task<Team> UpdateTeam(Team team);

        Task<List<Team>> GetTeams();


    }
}
