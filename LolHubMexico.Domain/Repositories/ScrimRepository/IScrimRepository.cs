using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.Entities.Scrims;

namespace LolHubMexico.Domain.Repositories.ScrimRepository
{
    public interface IScrimRepository
    {

        public Task<Scrim> CreateScrim(Scrim scrim);

        public Task<Scrim> UpdateScrim(Scrim scrim);

        public Task<Scrim?> GetScrimById(int idScrim);


        public Task<List<Scrim>> GetAllScrims();

        public Task<List<Scrim>> GetScrimsByTeam1(int idTeam);
        public Task<List<Scrim>> GetScrimsByTeam2(int idTeam);

        public Task DeleteScrim(Scrim scrim);

    }
}
