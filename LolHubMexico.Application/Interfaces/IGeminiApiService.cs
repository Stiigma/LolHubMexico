using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.Entities.RiotAPI;

namespace LolHubMexico.Application.Interfaces
{
    public interface IGeminiApiService
    {
        Task<string?> ProcessMatchEventsWithGemini(TimelineRiotDto timelineDto, MatchRiotDto matchDto);


    }
}
