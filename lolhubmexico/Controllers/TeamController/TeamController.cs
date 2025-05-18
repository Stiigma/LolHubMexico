using Microsoft.AspNetCore.Mvc;
using LolHubMexico.Application.TeamService;
using LolHubMexico.Domain.DTOs.Teams;
using LolHubMexico.Application.Exceptions;
using LolHubMexico.Domain.DTOs.Notificactions;
using LolHubMexico.Domain.Entities.Teams;

namespace LolHubMexico.API.Controllers.TeamController
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        

        private readonly TeamService _teamService;

        public TeamController(TeamService teamService)
        {
            _teamService = teamService;
        }


        [HttpPost("create-team")]

        public async Task<ActionResult> CreateTeamAsync(CreateTeamDTO newTeam) {
            try
            {

                var createdTeam = await _teamService.CreateTeamAsync(newTeam);
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
        [HttpGet("my-team")]
        public async Task<ActionResult<Team>> GetTeamByIdUser(int IdUser)
        {
            try
            {
                var teamRelease = await _teamService.GetTeamByUserId(IdUser);
                return Ok(teamRelease);
            }
            catch (AppException ex)
            {
                // Error personalizado que lanzas desde el servicio
                return BadRequest(new { message = ex.Message });
            }
        }

        //[HttpPost("invite")]
        //public async Task<IActionResult> Invite([FromBody] List<String> idsNewMembers)
        //{
        //    var result = await _teamInvitationService.CreateInvitationAsync(idsNewMembers);
        //    return Ok(result);
        //}




    }
}
