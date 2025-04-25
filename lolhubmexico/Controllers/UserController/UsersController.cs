using LolHubMexico.Application.UserService;
using LolHubMexico.Domain.Entities.Users;
using Microsoft.AspNetCore.Mvc;

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
    }
}
