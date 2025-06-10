using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.Entities.DatailsScrims;

namespace LolHubMexico.Domain.Repositories.ScrimRepository
{
    public interface IDetailsScrimRepository
    {
        Task<DetailsScrim> CreateDetailScrim(DetailsScrim detailsScrim);
        Task<List<DetailsScrim>> GetDetailsById(int idDetails);

        Task<List<DetailsScrim>> GetDetailsByIdAndTeam(int idDetails, int idTeam);
        
        Task<List<DetailsScrim>> GetDetailsByIdScrim(int idscrim);

        Task<DetailsScrim> UpdateDetailsScrimAsync(DetailsScrim updatedDetail);
    }
}
