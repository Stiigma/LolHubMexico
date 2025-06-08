using LolHubMexico.Application.Exceptions;
using LolHubMexico.Application.Interfaces;
using LolHubMexico.Domain.DTOs.Players;
using Microsoft.AspNetCore.Mvc;

namespace LolHubMexico.Controllers.PayerController
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpPost("link")]
        public async Task<IActionResult> LinkSummoner([FromBody] LinkSummonerRequest request)
        {
            try
            {
                var result = await _playerService.LinkSummonerAsync(request.UserId, request.SummonerName, "LAN", request.MainRole);
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
