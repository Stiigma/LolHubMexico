using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.Repositories.TeamRepository;
using LolHubMexico.Infrastructure.Data;
using LolHubMexico.Domain.Entities.Teams;
using Microsoft.EntityFrameworkCore;
using LolHubMexico.Application.Exceptions;
using System.Collections;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using LolHubMexico.Domain.Entities.Users;

namespace LolHubMexico.Infrastructure.Repositories.TeamRepository
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ContextDB _context;

        public TeamRepository(ContextDB context)
        {
            _context = context;
        }

        public async Task<Team>  CreateTeamAsync(Team  newTeam)
        {

            _context.Teams.Add(newTeam);
            await _context.SaveChangesAsync();
            return newTeam;
        }



        public async Task<Team> GetTeamByTeamName(string teamName)
        {


            var team = await _context.Teams.FirstOrDefaultAsync(u => u.TeamName == teamName);
            if (team == null)
            {
                throw new AppException("Error Base de datos");
                
            }
            
            
            return team;

        }

        public async Task<Team> GetTeamByIdUser(int IdUser)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(u => u.IdCapitan == IdUser);
            //var teamMember = await _context.TeamMembers.FirstOrDefaultAsync(u => u.IdUser == IdUser);
            if (team == null)
                return new Team();

            return team;
        }


        public async Task<TeamMember?> GetTeamMemberByIdUser(int IdUser)
        {
            var teamMem = await _context.TeamMembers.FirstOrDefaultAsync(u => u.IdUser == IdUser);
            //var teamMember = await _context.TeamMembers.FirstOrDefaultAsync(u => u.IdUser == IdUser);
            if (teamMem == null)
                return null;

            return teamMem;
        }

        public async Task<Team?> GetTeamById(int IdTeam)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(u => u.IdTeam == IdTeam);

            if (team == null)
                return null;

            return team;

        }

        public async Task<bool> IsExistTeamName(string teamName)
        {

            var team = await _context.Teams.FirstOrDefaultAsync(u => u.TeamName == teamName);
            //bool exist = false;



            if (team == null)
                return false;


            return true;

        }

        public async Task<TeamMember> AddMember(TeamMember newMember)
        {
            _context.TeamMembers.AddAsync(newMember);
            
            await _context.SaveChangesAsync();
            return newMember;
        }

        public async Task<bool> IsUserInAnyTeam(int userId)
        {
            return await _context.TeamMembers
                .AnyAsync(m => m.IdUser == userId && m.Status == 1); // o el campo que indique si está activo
        }

        public async Task<bool> ExistsCapitanAsync(int idCapitan)
        {
            return await _context.Teams.AnyAsync(t => t.IdCapitan == idCapitan);
        }

        

    }
}
