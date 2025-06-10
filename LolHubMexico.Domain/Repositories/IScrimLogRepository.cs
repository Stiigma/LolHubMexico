using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.Entities.ScrimLog;

namespace LolHubMexico.Domain.Repositories
{
    public interface IScrimLogRepository
    {

        Task AddScrimLogAsync(ScrimLog scrimLog);

        Task<ScrimLog?> GetScrimLogsByIdScrimAsync(int idScrim);
        Task<ScrimLog?> GetScrimLogByMatchIdAsync(string matchId);
        Task<ScrimLog?> GetScrimLogByIdAsync(int idLogScrim);
        Task UpdateScrimLogAsync(ScrimLog scrimLog);

        Task DeleteScrimLogAsync(int idLogScrim);
    }
}
