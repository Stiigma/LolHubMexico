using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.DTOs.Users
{
    public class UserDTO
    {
        public int IdUser { get; set; }
        public string UserName { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime FechaRegistro { get; set; }

        public string PhoneNumber { get; set; }

        public string Nacionality { get; set; }

        public int Role { get; set; }

        


    }
}
