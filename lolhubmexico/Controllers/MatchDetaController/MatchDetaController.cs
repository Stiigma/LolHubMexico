using LolHubMexico.Application.Exceptions;
using LolHubMexico.Application.Interfaces;
using LolHubMexico.Application.PlayerService;
using LolHubMexico.Application.ScrimProcessing;
using LolHubMexico.Application.ServicesMatchDetails;
using LolHubMexico.Domain.DTOs.Players;
using LolHubMexico.Domain.Entities.MatchDetails;
using LolHubMexico.Domain.Repositories.ScrimRepository;
using Microsoft.AspNetCore.Mvc;

namespace LolHubMexico.Controllers.MatchDetaController
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchDetaController : Controller
    {

        private readonly MatchDetailService _mDetailsService;
    
        public MatchDetaController(MatchDetailService mDetailsService)
        {
            _mDetailsService = mDetailsService;
          
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

    }
}
