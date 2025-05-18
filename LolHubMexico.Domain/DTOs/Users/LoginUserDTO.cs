using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.DTOs.Users
{
    public class LoginUserDTO
    {

        [EmailAddress]
        [MaxLength(320, ErrorMessage = "El Email no puede tener más de 320 caracteres.")]
        public string credencial { get; set; }
        public string password { get; set; }
    }
}
