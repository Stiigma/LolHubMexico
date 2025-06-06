using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.DTOs.Users
{
    public class CreateUserDTO
    {
        [Required]
        [MaxLength(30, ErrorMessage = "El usuario no puede tener más de 30 caracteres.")]
        public string UserName { get; set; }

        //[Required]
        //[MaxLength(320, ErrorMessage = "El FirebaseUid no puede tener más de 320 caracteres.")]
        //public string FirebaseUid { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(320, ErrorMessage = "El Email no puede tener más de 320 caracteres.")]
        public string Email { get; set; }
        
        [Required]
        [MaxLength(50, ErrorMessage = "El nombre completo no puede tener más de 50 caracteres.")]
        public string FullName { get; set; }

        [Required]
        [MaxLength(18, ErrorMessage = "El numero telefonico no puede tener más de 18 caracteres.")]
        public string PhoneNumber {  get; set; }

        [Required]
        [MaxLength(40, ErrorMessage = "La nacionalidad no puede tener más de 40 caracteres.")]
        public string Nacionality { get; set; }

        [Required]
        [MaxLength(40, ErrorMessage = "La nacionalidad no puede tener más de 40 caracteres.")]
        public string PasswordHash { get; set; }
        [Required]
        [MaxLength(40, ErrorMessage = "La nacionalidad no puede tener más de 40 caracteres.")]
        public string ConfirmPassword { get; set; }

    }
}
