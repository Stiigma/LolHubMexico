using LolHubMexico.Application;
using LolHubMexico.Application.Exceptions;
using LolHubMexico.Domain.DTOs.Notificactions;
using LolHubMexico.Domain.DTOs.Teams;
using Microsoft.AspNetCore.Mvc;

namespace LolHubMexico.Controllers.TeamController
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamInvitationController : ControllerBase
    {
        private readonly TeamInvitationService _invitationService;
        private readonly TeamService _teamService;

        public TeamInvitationController(TeamInvitationService invitationService, TeamService teamService)
        {
            _invitationService = invitationService;
            _teamService = teamService;
        }

        [HttpPost("invite")]
        public async Task<IActionResult> InviteUser([FromBody] CreateTeamInvitationDTO dto)
        {
            try
            {
                var invitation = await _invitationService.CreateInvitationAsync(dto);
                return Ok(new { success = true, invitation });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error al enviar la invitación.", error = ex.Message });
            }
        }

        [HttpGet("my-invite")]

        public async Task<IActionResult> MyInvites([FromQuery] int idUser)
        {
            try
            {
                var invitation = await _invitationService.GetInvitationById(idUser);
                return Ok(new { success = true, invitation });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error al enviar la invitación.", error = ex.Message });
            }
        }

        [HttpPost("joinTeam")]
        public async Task<ActionResult> AcceptInvitation([FromBody] JoinTeamDTO responseInvitation)
        {
            try
            {
                var updatedTeam = await _teamService.JoinTeam(responseInvitation);
                return Ok(updatedTeam);
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
