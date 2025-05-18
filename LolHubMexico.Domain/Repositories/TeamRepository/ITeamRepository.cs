using LolHubMexico.Domain.Entities.Teams;

namespace LolHubMexico.Domain.Repositories.TeamRepository
{
    public interface  ITeamRepository
    {
        Task<Team> CreateTeamAsync(Team newTeam);

        Task<Team> GetTeamByTeamName(string teamName);
        Task<Team?> GetTeamById(int IdTeam);

        Task<Team> GetTeamByIdUser(int IdUser);
        Task<TeamMember?> GetTeamMemberByIdUser(int IdUser);


        Task<bool> IsExistTeamName(string teamName);

        Task<TeamMember> AddMember(TeamMember newMember);
        Task<bool> IsUserInAnyTeam(int userId);

    }
}
