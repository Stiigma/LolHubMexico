using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.Entities.DatailsScrims;
using LolHubMexico.Domain.Repositories.ScrimRepository;
using LolHubMexico.Infrastructure.Data;

namespace LolHubMexico.Infrastructure.Repositories.ScrimRepository
{
    public class DetailsScrimRepository : IDetailsScrimRepository
    {
        private readonly ContextDB _context;
        public DetailsScrimRepository(ContextDB context) {
            _context = context;
        }

        public async Task<DetailsScrim> CreateDetailScrim(DetailsScrim detailsScrim)
        {
            var newDetails =  _context.DetailsScrim.Add(detailsScrim);

            await _context.SaveChangesAsync();

            return newDetails.Entity;

        }
    }
}
