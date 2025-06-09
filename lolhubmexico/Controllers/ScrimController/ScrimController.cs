using LolHubMexico.Application;
using LolHubMexico.Application.Exceptions;
using LolHubMexico.Application.Interfaces;
using LolHubMexico.Application.ScrimService;
using LolHubMexico.Domain.DTOs.Players;
using LolHubMexico.Domain.DTOs.Scrims;
using LolHubMexico.Domain.DTOs.Teams;
using Microsoft.AspNetCore.Mvc;

namespace LolHubMexico.Controllers.ScrimController
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScrimController : ControllerBase
    {
        private readonly ScrimService _scrimPlayer;

        public ScrimController(ScrimService scrimPlayer)
        {
            _scrimPlayer = scrimPlayer;
        }

        [HttpPost("create-scrim")]

        public async Task<ActionResult> CreateTeamAsync(CreateScrimDTO newScrim)
        {
            try
            {

                var createdScrim = await _scrimPlayer.CreateScrim(newScrim);
                var created = true;
                if (!createdScrim)
                    created = false;
                return Ok(new { created, createdScrim });
            }
            catch (AppException ex)
            {
                // Error personalizado que lanzas desde el servicio
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Otro tipo de error no controlado
                return StatusCode(500, new { message = "Error interno del servidor", detail = ex.Message });
            }
        }

        [HttpGet("get-pending")]

        public async Task<ActionResult<ScrimPDTO>> GetPending()
        {
            try
            {

                var createdScrim = await _scrimPlayer.GetScrimsPending();
                
                return Ok( createdScrim);
            }
            catch (AppException ex)
            {
                // Error personalizado que lanzas desde el servicio
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Otro tipo de error no controlado
                return StatusCode(500, new { message = "Error interno del servidor", detail = ex.Message });
            }
        }


    }
}
