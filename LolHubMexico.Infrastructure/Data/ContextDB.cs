using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LolHubMexico.Domain.Entities.Users;
using LolHubMexico.Domain.Entities.Notifications;

using LolHubMexico.Domain.Entities.Teams;
using LolHubMexico.Domain.Entities.Scrims;
using LolHubMexico.Domain.Entities.Players;
using LolHubMexico.Domain.Entities.DatailsScrims;

namespace LolHubMexico.Infrastructure.Data
{
    public class ContextDB : DbContext
    {
        public ContextDB(DbContextOptions<ContextDB> options)
            : base(options)
        {
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }

        public DbSet<TeamMember> TeamMembers { get; set; }

        public DbSet<TeamInvitation> TeamInvitations {  get; set; }

        public DbSet<Scrim> Scrims { get; set; }

        public DbSet<Player> Players { get; set; }


        public DbSet<DetailsScrim> DetailsScrim { get; set; }
    }
}
