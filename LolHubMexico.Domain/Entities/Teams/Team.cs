using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using LolHubMexico.Domain.Entities.Users;



namespace LolHubMexico.Domain.Entities.Teams
{
    public class Team
    {
        [Key]
        public int IdTeam { get; set; }
        public string TeamName { get; set; }
        public string TeamLogo { get; set; }

        public DateTime CreationDate { get; set; }

        public int IdCapitan {  get; set; }

        public int Status {  get; set; }

        public string DescripcionTeam { get; set; }


        public Team()
        {
            IdTeam = 0;
            TeamName = string.Empty;
            TeamLogo = string.Empty;
            CreationDate = DateTime.Now;
            IdCapitan = 0;
            Status = -1;
            DescripcionTeam = string.Empty;

        }

        //public const int MaxIntegrants = 8;

        //public List<TeamMember> Members { get; set; } = new();


        //public void AddMember(User user)
        //{
        //    if (IsFull()) throw new Exception("Equipo lleno.");
        //    if (HasMember(user.IdUser)) throw new Exception("Ya pertenece.");

        //    TeamMember newMember = new TeamMember
        //    {
        //        IdTeam = this.IdTeam,
        //        Join_date = DateTime.Now,
        //        Role = "flex-rol",
        //        IdUser = user.IdUser,
        //        Status = 1
        //    };

        //    Members.Add(newMember);
        //}

        //public bool HasMember(int userId) => Members.Any(m => m.IdUser == userId);
        //public bool IsFull() => Members.Count >= MaxIntegrants;



    }
}
