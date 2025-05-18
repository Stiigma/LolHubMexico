using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.Entities.Notifications;

namespace LolHubMexico.Domain.Repositories.TeamRepository
{
    public interface ITeamInvitationRepository
    {
        Task<TeamInvitation?> GetByIdAsync(int invitationId);

        Task AddAsync(TeamInvitation invitation);
        Task UpdateAsync(TeamInvitation invitation);
        Task<bool> ExistsPendingInvitationAsync(int idTeam, int idUser);
        Task<List<TeamInvitation>> GetPendingInvitationsForUserAsync(int idUser);
    }

}
