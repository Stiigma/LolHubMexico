using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Application.DTOs.Users
{
    public class CreateUserDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber {  get; set; }

        [Required]
        public string Nacionality { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
