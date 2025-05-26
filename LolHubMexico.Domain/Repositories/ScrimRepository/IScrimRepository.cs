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

    }
}
