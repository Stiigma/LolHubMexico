using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using LolHubMexico.Domain.Entities.Notifications;
using LolHubMexico.Domain.Repositories.TeamRepository;
using LolHubMexico.Infrastructure.Data;

namespace LolHubMexico.Infrastructure.Repositories.TeamRepository
{
    public class TeamInvitationRepository : ITeamInvitationRepository
    {
        private readonly ContextDB _context;

        public TeamInvitationRepository(ContextDB context)
        {
            _context = context;
        }

        public async Task<TeamInvitation?> GetByIdAsync(int id)
        {
            return await _context.TeamInvitations.FindAsync(id);
        }

        public async Task AddAsync(TeamInvitation invitation)
        {
            _context.TeamInvitations.Add(invitation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TeamInvitation invitation)
        {
            _context.TeamInvitations.Update(invitation);
        }

        public async Task<bool> ExistsPendingInvitationAsync(int idTeam, int idUser)
        {
            return await _context.TeamInvitations.AnyAsync(i =>
                i.IdTeam == idTeam &&
                i.IdUser == idUser &&
                i.Status == 0);
        }

        public async Task<List<TeamInvitation>> GetPendingInvitationsForUserAsync(int idUser)
        {
            return await _context.TeamInvitations
                .Where(i => i.IdUser == idUser && i.Status == 0)
                .ToListAsync();
        }
    }

}
