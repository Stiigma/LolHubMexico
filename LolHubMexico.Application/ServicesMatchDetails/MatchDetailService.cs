using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.Entities.DatailsScrims;
using LolHubMexico.Domain.Entities.MatchDetails;
using LolHubMexico.Application.Exceptions;
using LolHubMexico.Domain.Notifications;
using LolHubMexico.Domain.Repositories.MatchRepository;
using LolHubMexico.Domain.Repositories.TeamRepository;

namespace LolHubMexico.Application.ServicesMatchDetails
{
    public class MatchDetailService
    {
        private readonly IMatchDetailsRepository _repository;
        

        public MatchDetailService(IMatchDetailsRepository repository)
        {
            _repository = repository;
        
        }

        public async Task<MatchDetail> GetMatchDetailById(int idScrim)
        {
            if (idScrim == 0)
                throw new AppException("Id invalido");

            var match = await _repository.GetByScrimIdAsync(idScrim);

            if(match == null)
                throw new AppException($"No existe id: {idScrim}");

            return match;
        }
    }
}
