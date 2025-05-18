using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace LolHubMexico.Domain.Entities.Notifications
{
    public class TeamInvitation
    {
        [Key]
        public int IdInvitation {  get; set; }

        public int IdTeam {  get; set; }

        public int IdUser { get; set; }

        public int InvitedBy { get; set; }

        public int Status { get; set; }

        public DateTime SentDate { get; set; }

        public DateTime RespondedDate { get; set; }

        public string Message {  get; set; }


        public static TeamInvitation Create(int idTeam, int idUser, int invitedBy, string? message)
        {
            return new TeamInvitation
            {
                IdTeam = idTeam,
                IdUser = idUser,
                InvitedBy = invitedBy,
                Status = 0, // 0 = pendiente
                SentDate = DateTime.Now,
                Message = message
            };
        }

    }
}
