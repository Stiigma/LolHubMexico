using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.Entities.Users;
using LolHubMexico.Domain.Repositories.UserRepository;
using Microsoft.Extensions.Logging;
using LolHubMexico.Domain.DTOs.Users;
using LolHubMexico.Application.Exceptions;
using System.Globalization;
using LolHubMexico.Domain.Repositories.TeamRepository;


namespace LolHubMexico.Application.UserService
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger, ITeamRepository teamRepository)
        {
            _userRepository = userRepository;
            _logger = logger;
            _teamRepository = teamRepository;
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

            if (DTO.PasswordHash != DTO.ConfirmPassword)
                throw new AppException("Las contraseñas no coinciden");

            var existsByEmail = await _userRepository.ExistsByEmailAsync(DTO.Email);

            if (existsByEmail)
                throw new AppException("El correo ya está en uso");

            var existsByUsername = await _userRepository.ExistsByUserNameAsync(DTO.UserName);

            if(existsByUsername)
                throw new AppException("El UserName ya está en uso");
            
            var existsByPhoneNumber = await _userRepository.ExistsByPhoneNumberAsync(DTO.PhoneNumber);

            if (existsByPhoneNumber)
                throw new AppException("El numero de celular ya está en uso");

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(DTO.PasswordHash);

            User user = new User
            {
                Email = DTO.Email,
                UserName = DTO.UserName,
                PhoneNumber = DTO.PhoneNumber,
                FullName = DTO.FullName,
                PasswordHash = hashedPassword,
                Nacionality = DTO.Nacionality,
                Role = 2,
                Registration_date = DateTime.Now,
                Status = 1
            };

            return await _userRepository.CreateAsync(user);

        }



        public async Task<List<UserSearchDTO>> SearchUsersByNameAsync(string query, int requesterId)
        {
            var userList = await _userRepository.SearchUsersByNameAsync(query);


            var filteredList = userList
                .Where(u => u.IdUser != requesterId) // ← aquí filtramos al usuario logueado
                .ToList();

            foreach (var user in filteredList)
            {
                user.status = await _teamRepository.IsUserInAnyTeam(user.IdUser) ? 1 : 0;
            }

            return userList;
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

        public async Task<UserDTO> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                throw new AppException("No se encontró una cuenta con ese correo.");

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new AppException("La contraseña es incorrecta.");

            var userDTO = new UserDTO
            {
                Email = user.Email,
                IdUser = user.IdUser,
                FechaRegistro = user.Registration_date,
                FullName = user.FullName,
                Nacionality = user.Nacionality,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                UserName = user.UserName,
            };

            return userDTO; // O puedes mapear un DTO de respuesta si lo prefieres
        }
    }
}
