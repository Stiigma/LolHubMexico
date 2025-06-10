using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.DTOs.Torneo
{
    public class TorneoDTO
    {
       
        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }


        public int IdCreador { get; set; }  // FK a tabla de Usuarios o Admins
        
    }
}
