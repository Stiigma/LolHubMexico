using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.Entities.Users;
using LolHubMexico.Domain.Repositories.UserRepository;
using Microsoft.Extensions.Logging;


namespace LolHubMexico.Application.UserService
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<List<User>?> GetAllAsync()
        {
            try
            {
                return await _userRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuarios");
                return new List<User>();
            }
        }
    }
}
