using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.Entities.Users;

namespace LolHubMexico.Domain.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);

        Task<User> UpdateAsync(User user);

        Task<User> DeleteAsync(int id);

        Task<User?> GetUserById(int id);

        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByPhoneNumberAsync(string phoneNumber);
        Task<List<User>?> GetAllAsync();

        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByUserNameAsync(string username);

        Task<bool> ExistsByPhoneNumberAsync(string phoneNumber);
    }
}
