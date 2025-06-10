using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.Entities.ScrimLog;
using LolHubMexico.Domain.Repositories;
using LolHubMexico.Application.Exceptions;

namespace LolHubMexico.Application.ScrimLogService
{
    public class SlogService

    {
        private readonly IScrimLogRepository _repository;
        public SlogService(IScrimLogRepository repository) { 
            _repository = repository;
        }

        public async Task <ScrimLog> GetLogByIdScrim( int idScrim)
        {
            if (idScrim == 0)
                throw new AppException("id no valido");
            var scrimLog = await _repository.GetScrimLogsByIdScrimAsync(idScrim);

            if(scrimLog == null)
                throw new AppException("No Hay log, contactanos...");

            return scrimLog;
        }

    }
}
