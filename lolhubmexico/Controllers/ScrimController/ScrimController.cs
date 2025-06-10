using LolHubMexico.Application;
using LolHubMexico.Application.Exceptions;
using LolHubMexico.Application.Interfaces;
using LolHubMexico.Application.ScrimServices;
using LolHubMexico.Application.ScrimDetailsService;
using LolHubMexico.Domain.DTOs.Players;
using LolHubMexico.Domain.DTOs.Scrims;
using LolHubMexico.Domain.DTOs.Teams;
using Microsoft.AspNetCore.Mvc;
using LolHubMexico.Domain.Entities.DatailsScrims;
using LolHubMexico.Domain.Entities.Scrims;

namespace LolHubMexico.Controllers.ScrimController
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScrimController : ControllerBase
    {
        private readonly ScrimService _scrimPlayer;
        private readonly ScrimDetailServices _scrimDetailServices;

        public ScrimController(ScrimService scrimPlayer, ScrimDetailServices scrimDetailServices)
        {
            _scrimPlayer = scrimPlayer;
            _scrimDetailServices = scrimDetailServices;
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



        [HttpPost("result-scrim")]

        public async Task<ActionResult> CreateTeamAsync(ScrimResultReportDTO resultScrim)
        {
            try
            {

                var createdScrim = await _scrimPlayer.InsertResultMatchByTeam(resultScrim);
                var created = true;
                if (!createdScrim)
                    created = false;
                return Ok(new { created });
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

        [HttpGet("by-id")]

        public async Task<ActionResult<ScrimPDTO>> GetScrimById(int idScrim)
        {
            try
            {

                var scrim = await _scrimPlayer.GetScrimById(idScrim);

                return Ok(scrim);
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

        [HttpGet("team/by-id")]

        public async Task<ActionResult<List<ScrimPDTO>>> GetScrimbyTeamid(int idTeam)
        {
            try
            {

                var scrim = await _scrimPlayer.GetScrimActiveTeam(idTeam);

                return Ok(scrim);
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
        [HttpGet("details/by-id")]

        public async Task<ActionResult<List<DetailsScrim>>> GetScrimDetailsById(int idScrim)
        {
            try
            {

                var scrim = await _scrimDetailServices.GetDetailsScrimById(idScrim);

                return Ok(scrim);
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

        [HttpPost("accept-scrim")]

        public async Task<ActionResult<bool>> AcceptScrim([FromBody] RivalDTO rival)
        {
            try
            {

                var IsAcccept = await _scrimPlayer.AcceptScrim(rival);

                return Ok(IsAcccept);
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


        [HttpGet("/active/{idUser}")]
        public async Task<IActionResult> GetActiveScrimsByUser(int idUser)
        {
          
            try
            {

                var scrims = await _scrimPlayer.GetScrimsByIdUserActives(idUser);
                return Ok(scrims);
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

        [HttpPut("update-scrim")] 

        public async Task<ActionResult<ScrimPDTO>> updateScrim([FromBody] ScrimPDTO scrim)
        {
            try
            {

                var updateScrim = await _scrimPlayer.updateScrim(scrim);

                return Ok(updateScrim);
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

        [HttpGet("get-activas")]

        public async Task<ActionResult<ScrimPDTO>> GetActivas(int idUser)
        {
            try
            {

                var activeScrims = await _scrimPlayer.GetScrimsPending();

                return Ok(activeScrims);
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

        [HttpGet("get-Summoners-pending")]

        public async Task<ActionResult<List<UserLinkDTO>>> GetUserDetailsPending(int idScrim, int idTeam)
        {
            try
            {

                var UsersLink = await _scrimDetailServices.GetDetailByIdAndTeam(idScrim, idTeam);

                return Ok(UsersLink);
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
