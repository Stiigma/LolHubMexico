using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.DTOs.Teams
{
    public class CreateTeamDTO
    {
        [Required]
        [MaxLength(100, ErrorMessage = "El nombre del equipo no puede tener más de 100 caracteres.")]
        public string TeamName { get; set; }

        //public string TeamLogo { get; set; }

        [Required]
        public int IdCapitan { get; set; }

        //[MaxLength(255, ErrorMessage = "La descripción no puede tener más de 255 caracteres.")]

        //public string DescripcionTeam { get; set; }
    }
}
