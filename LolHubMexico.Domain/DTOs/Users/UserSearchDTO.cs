using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.DTOs.Users
{
    public class UserSearchDTO
    {
        public int IdUser { get; set; }
        public string UserName { get; set; }

        public int status { get; set; }
    }
}
