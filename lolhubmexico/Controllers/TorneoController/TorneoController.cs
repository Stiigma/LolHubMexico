using LolHubMexico.Application.Exceptions;
using LolHubMexico.Application;
using LolHubMexico.Domain.DTOs.Teams;
using Microsoft.AspNetCore.Mvc;
using LolHubMexico.Application.TorneoServices;
using LolHubMexico.Domain.DTOs.Torneo;
using Microsoft.AspNetCore.Http.HttpResults;
using LolHubMexico.Domain.Entities.Torneos;


namespace LolHubMexico.Controllers.TorneoController
{

    [ApiController]
    [Route("api/[controller]")]
    public class TorneoController : Controller
    {

        private readonly TorneoService _torneoService;

        public TorneoController(TorneoService teamService)
        {
            _torneoService = teamService;
        }


        [HttpPost("create-torneo")]

        public async Task<ActionResult> CreateTeamAsync(TorneoDTO newTeam)
        {
            try
            {

                var createdTeam = await _torneoService.CrearTorneoAsync(newTeam);
                var created = true;
                return Ok(new { created, createdTeam });
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

        [HttpPost("Entrar-Torneo")]

        public async Task<ActionResult> entrarTorneo(int idTorneo, int idEquipo, int iUser)
        {
            try
            {

                var createdTeam = await _torneoService.UnirseATorneoAsync(idTorneo, idEquipo, iUser);
                var created = true;
                return Ok(new { created, createdTeam });
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

        [HttpGet("pendientes")]

        public async Task<ActionResult<List<Torneo>>> TorneosPendientes()
        {
            try
            {

                var createdTeam = await _torneoService.getTodosTorneoEstado(0);
                var created = true;
                return Ok(new { created, createdTeam });
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

        [HttpGet("mi-torneo/{idTeam}")]
        public async Task<ActionResult<List<Torneo>>> misTorneos(int idTeam)
        {
            try
            {

                var createdTeam = await _torneoService.TomarMisTorneos(idTeam);
                var created = true;
                return Ok(new { created, createdTeam });
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

        [HttpGet("by-id/{idTorneo}")]
        public async Task<ActionResult<List<Torneo>>> TorneoById(int idTorneo)
        {
            try
            {

                var createdTeam = await _torneoService.TomarTorneoPorId(idTorneo);;
                var created = true;
                return Ok(new { created, createdTeam });
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
