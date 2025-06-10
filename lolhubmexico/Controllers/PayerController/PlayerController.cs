using LolHubMexico.Application.Exceptions;
using LolHubMexico.Application.Interfaces;
using LolHubMexico.Application.ScrimProcessing;
using LolHubMexico.Application.ScrimService;
using LolHubMexico.Domain.DTOs.Players;
using LolHubMexico.Domain.Entities.Scrims;
using LolHubMexico.Domain.Repositories.ScrimRepository;
using Microsoft.AspNetCore.Mvc;

namespace LolHubMexico.Controllers.PayerController
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;
        private readonly IScrimRepository _iotService;
        private readonly IScrimProcessor _scrimService;

        public PlayerController(IPlayerService playerService, IScrimRepository scrimRepository, IScrimProcessor scrimService)
        {
            _playerService = playerService;
            _iotService = scrimRepository;
            _scrimService = scrimService;
        }

        [HttpPost("procesar/{idScrim}")]
        public async Task<IActionResult> ProcesarScrim(int idScrim, string idmatch)
        {
            try
            {
                var scrim = await _iotService.GetScrimById(idScrim);
                if (scrim == null)
                    return NotFound(new { message = $"Scrim con id {idScrim} no encontrada." });

                await _scrimService.ProcessAsync(scrim, idmatch);

                return Ok(new { message = "Scrim procesada correctamente." });
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno", detail = ex.Message });
            }
        }

        [HttpPost("link")]
        public async Task<IActionResult> LinkSummoner([FromBody] LinkSummonerRequest request)
        {
            try
            {
                var result = await _playerService.LinkSummonerAsync(request.UserId, request.SummonerName, request.tagName, request.MainRole);
                return Ok(result);
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
        

        [HttpGet("by-user")]
        public async Task<ActionResult<PlayerDTO>> GetPlayerByIdUser([FromQuery] int idUser)
        {
            try
            {
                var player = await _playerService.GetPlayerByIdUser(idUser);
                return Ok(player);
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
