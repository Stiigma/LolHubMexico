using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.Entities.MatchDetails;

namespace LolHubMexico.Domain.Repositories.MatchRepository
{
    public interface IMatchDetailsRepository
    {
        Task<MatchDetail> CreateAsync(MatchDetail matchDetails);
        Task<MatchDetail?> GetByScrimIdAsync(int idScrim);
        Task<MatchDetail> UpdateAsync(MatchDetail matchDetails);
        Task<bool> DeleteByScrimIdAsync(int idScrim);
    }

}
