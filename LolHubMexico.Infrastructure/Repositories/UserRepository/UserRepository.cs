using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.Repositories.UserRepository;
using LolHubMexico.Infrastructure.Data;
using LolHubMexico.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using LolHubMexico.Domain.DTOs.Users;
namespace LolHubMexico.Infrastructure.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly ContextDB _context;

        public UserRepository(ContextDB context)
        {
            _context = context;

        }

        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.IdUser == user.IdUser);
            if (userInDb == null) return null;

            userInDb.FullName = user.FullName;
            userInDb.Email = user.Email;
            userInDb.Nacionality = user.Nacionality;
            userInDb.PhoneNumber = user.PhoneNumber;
            userInDb.Status = user.Status;
            userInDb.Role = user.Role;
            userInDb.UserName = user.UserName;
            await _context.SaveChangesAsync();
            return userInDb;
        }
        public async Task<User> DeleteAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.IdUser == id);
            user.Status = 0;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }


        public async Task<List<UserSearchDTO>> SearchUsersByNameAsync(string query)
        {
            return await _context.Users
                .Where(u => u.UserName.Contains(query))
                .OrderBy(u => u.UserName)
                .Select(u => new UserSearchDTO
                {
                    IdUser = u.IdUser,
                    UserName = u.UserName,
                    //AvatarUrl = u.AvatarUrl
                })
                .Take(10)
                .ToListAsync();
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.IdUser == id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<User?> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task<List<User>?> GetAllAsync()
        {
            return await _context.Users
                                     .Where(u => u.Status == 1)
                                     .ToListAsync();
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null) 
                return false;

            return true;
        }

        public async Task<bool> ExistsByUserNameAsync(string username)
        {
            var userName = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName != null && u.UserName.ToLower() == username.ToLower());
            if (userName == null) return false;

            return true;
        }

        public async Task<bool> ExistsByPhoneNumberAsync(string phoneNumber)
        {
            var PhoneNumber = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            if (PhoneNumber == null) return false;

            return true;
        }

        public async Task<bool> ChangeStatus(int idUser, int newStatus)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.IdUser == idUser);

            if (user == null)
                return false;

            user.Status = newStatus;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangeRol(int idUser, int newRole)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.IdUser == idUser);

            if (user == null)
                return false;

            user.Role = newRole;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
