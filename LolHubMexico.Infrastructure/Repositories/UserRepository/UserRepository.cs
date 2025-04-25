using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.Repositories.UserRepository;
using LolHubMexico.Infrastructure.Data;
using LolHubMexico.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
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
            return await _context.Users.ToListAsync();
        }
    }
}
