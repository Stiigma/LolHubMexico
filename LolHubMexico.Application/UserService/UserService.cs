using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.Entities.Users;
using LolHubMexico.Domain.Repositories.UserRepository;
using Microsoft.Extensions.Logging;
using LolHubMexico.Application.DTOs.Users;
using LolHubMexico.Application.Exceptions;
using System.Globalization;


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

        public async Task<User> CreateUserAsync(CreateUserDTO DTO)
        {
            if (DTO == null)
                throw new AppException("El DTO fue NULO");

            var existsByEmail = await _userRepository.ExistsByEmailAsync(DTO.Email);

            if (existsByEmail)
                throw new AppException("El correo ya está en uso");

            var existsByUsername = await _userRepository.ExistsByUserNameAsync(DTO.UserName);

            if(existsByUsername)
                throw new AppException("El UserName ya está en uso");
            
            var existsByPhoneNumber = await _userRepository.ExistsByPhoneNumberAsync(DTO.PhoneNumber);

            if (existsByPhoneNumber)
                throw new AppException("El numero de celular ya está en uso");

            User user = new User
            {
                Email = DTO.Email,
                UserName = DTO.UserName,
                PhoneNumber = DTO.PhoneNumber,
                FullName = DTO.FullName,
                PasswordHash = DTO.PasswordHash,
                Nacionality = DTO.Nacionality,
                Role = 2,
                Registration_date = DateTime.Now,
                Status = 1
            };

            return await _userRepository.CreateAsync(user);

        }

        public async Task<User> UpdateUserAsync(UserDTO DTO)
        {
            if (DTO == null)
                throw new AppException("El DTO fue NULO");

            var user = await _userRepository.GetUserById(DTO.IdUser);

            if (user == null)
                throw new AppException("El ID es inexistente");

            var existsByEmail = await _userRepository.ExistsByEmailAsync(DTO.Email);

            if (existsByEmail && user.Email != DTO.Email)
                throw new AppException("El correo ya está en uso");

            var existsByUsername = await _userRepository.ExistsByUserNameAsync(DTO.UserName);

            if (existsByUsername && user.UserName != DTO.UserName)
                throw new AppException("El UserName ya está en uso");

            var existsByPhoneNumber = await _userRepository.ExistsByPhoneNumberAsync(DTO.PhoneNumber);

            if (existsByPhoneNumber && user.PhoneNumber != DTO.PhoneNumber)
                throw new AppException("El numero de celular ya está en uso");

            User userUpdate = new User
            {
                IdUser = user.IdUser,
                Email = DTO.Email,
                UserName = DTO.UserName,
                PhoneNumber = DTO.PhoneNumber,
                FullName = DTO.FullName,
                Nacionality = DTO.Nacionality,
                Role = user.Role,
                Registration_date = user.Registration_date,
                Status = user.Status
            };

            return await _userRepository.UpdateAsync(userUpdate);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetUserById(id);

            if (user == null)
                throw new AppException("El id del usuario no existe");

            var updateUser = _userRepository.DeleteAsync(id);

            return true;

        }


    }
}
