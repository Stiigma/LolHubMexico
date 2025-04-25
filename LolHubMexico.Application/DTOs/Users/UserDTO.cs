using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Application.DTOs.Users
{
    public class UserDTO
    {
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime FechaRegistro { get; set; }

        public string Nacionality { get; set; }

    }
}
