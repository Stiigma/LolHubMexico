using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.DTOs.Notificactions;
using LolHubMexico.Domain.Entities.Notifications;
using LolHubMexico.Domain.Notifications;
using LolHubMexico.Domain.Repositories.TeamRepository;

namespace LolHubMexico.Application.TeamService
{
    public class TeamInvitationService
    {
        private readonly ITeamInvitationRepository _invitationRepo;
        private readonly INotifier _notifier;

        public TeamInvitationService(ITeamInvitationRepository invitationRepo, INotifier notifier)
        {
            _invitationRepo = invitationRepo;
            _notifier = notifier;
        }

        public async Task<TeamInvitation> CreateInvitationAsync(CreateTeamInvitationDTO dto)
        {
            bool exists = await _invitationRepo.ExistsPendingInvitationAsync(dto.IdTeam, dto.IdUser);
            if (exists)
                throw new InvalidOperationException("Ya existe una invitación pendiente para este usuario.");

            var invitation = TeamInvitation.Create(dto.IdTeam, dto.IdUser, dto.InvitedBy, dto.Message);
            await _invitationRepo.AddAsync(invitation);

            // Aquí se hace la notificación en tiempo real 👇
            await _notifier.NotifyAsync(dto.IdUser, new
            {
                invitation.IdTeam,
                invitation.Message,
                From = invitation.InvitedBy
            });

            return invitation;
        }


        public async Task<List<TeamInvitation>?> GetInvitationById(int idUser)
        {

            var listTI = await _invitationRepo.GetPendingInvitationsForUserAsync(idUser);

            if(listTI == null)
                throw new InvalidOperationException("No Existen Invitaciones");

            return listTI;

        }


    }

}
