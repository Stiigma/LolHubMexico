using LolHubMexico.Application.UserService;
using LolHubMexico.Domain.Entities.Users;
using LolHubMexico.Application.DTOs.Users;
using Microsoft.AspNetCore.Mvc;
using LolHubMexico.Application.Exceptions;

namespace LolHubMexico.API.Controllers.UserController
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUserAsync(CreateUserDTO DTO)
        {
            try
            {
                var user = await _userService.CreateUserAsync(DTO);
                return Ok(user);
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
