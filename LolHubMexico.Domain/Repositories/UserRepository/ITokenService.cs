using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Domain.DTOs.Users;

namespace LolHubMexico.Domain.Repositories.UserRepository
{
    public interface ITokenService
    {
        string GenerateJwtToken(UserTokenDTO user);
    }
}
