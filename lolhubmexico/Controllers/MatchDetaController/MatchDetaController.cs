using LolHubMexico.Application.dessingPatterns;
using LolHubMexico.Application.Exceptions;
using LolHubMexico.Application.Interfaces;
using LolHubMexico.Application.PlayerService;
using LolHubMexico.Application.ScrimLogService;
using LolHubMexico.Application.ScrimProcessing;
using LolHubMexico.Application.ServicesMatchDetails;
using LolHubMexico.Domain.DTOs.Players;
using LolHubMexico.Domain.Entities.MatchDetails;
using LolHubMexico.Domain.Entities.ScrimLog;
using LolHubMexico.Domain.Repositories.ScrimRepository;
using Microsoft.AspNetCore.Mvc;

namespace LolHubMexico.Controllers.MatchDetaController
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchDetaController : Controller
    {

        private readonly MatchDetailService _mDetailsService;
        private readonly IMatchAnalysisFacade _matchAnalysisFacade;
        private readonly SlogService _slogService;

        public MatchDetaController(MatchDetailService mDetailsService, IMatchAnalysisFacade matchAnalysisFacade, SlogService slogService)
        {
            _mDetailsService = mDetailsService;
            _matchAnalysisFacade = matchAnalysisFacade;
            _slogService = slogService;


        }

        [HttpGet("by-user")]
        public async Task<ActionResult<MatchDetail>> GetMDetailByIdScrim([FromQuery] int idScrim)
        {
            try
            {
                var match = await _mDetailsService.GetMatchDetailById(idScrim);
                return Ok(match);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", detail = ex.Message });
            }
        }


        [HttpGet("gemini")]
        public async Task<ActionResult<MatchDetail>> GetMDetailByIdScrim([FromQuery] string idmatch)
        {
            try
            {
                var match = await _matchAnalysisFacade.GetGeminiMatchAnalysisJsonAsync(idmatch);
                return Ok(match);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", detail = ex.Message });
            }
        }

        [HttpGet("/scrim/log/{idScrim}")]
        public async Task<ActionResult<ScrimLog>> GetLogScirm(int idScrim)
        {
            try
            {
                var log = await _slogService.GetLogByIdScrim(idScrim);
                return Ok(log);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", detail = ex.Message });
            }
        }



    }
}
