using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.Entities;

namespace LolHubMexico.Application.dessingPatterns
{
    public interface IMatchAnalysisFacade
    {
        Task<GeminiMatchAnalysis?> GetGeminiMatchAnalysisJsonAsync(string matchId);
    }
}
