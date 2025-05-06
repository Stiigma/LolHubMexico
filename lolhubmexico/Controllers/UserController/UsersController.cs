using LolHubMexico.Application.UserService;
using LolHubMexico.Domain.Entities.Users;
using LolHubMexico.Domain.DTOs.Users;
using Microsoft.AspNetCore.Mvc;
using LolHubMexico.Application.Exceptions;
using LolHubMexico.Domain.Repositories.UserRepository;

namespace LolHubMexico.API.Controllers.UserController
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly UserService _userService;
        private readonly ITokenService _tokenService;

        public UsersController(UserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpPost("register")]
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

        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync(LoginUserDTO loginUserDTO)
        {
            try
            {
                var user = await _userService.LoginAsync(loginUserDTO.credencial,loginUserDTO.password);

                var dtoToToken = new UserTokenDTO
                {
                    IdUser = user.IdUser,
                    Email = user.Email,
                    UserName = user.UserName,
                    Role = user.Role
                };

                var token = _tokenService.GenerateJwtToken(dtoToToken);

                return Ok(new { token, user });
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
