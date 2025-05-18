using LolHubMexico.Application.TeamService;
using LolHubMexico.Domain.DTOs.Notificactions;
using Microsoft.AspNetCore.Mvc;

namespace LolHubMexico.API.Controllers.TeamController
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamInvitationController : ControllerBase
    {
        private readonly TeamInvitationService _invitationService;

        public TeamInvitationController(TeamInvitationService invitationService)
        {
            _invitationService = invitationService;
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
    }

}
