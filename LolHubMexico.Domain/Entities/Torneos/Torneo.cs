using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.Entities.Torneos
{
    public class Torneo
    {
        [Key]
        public int IdTorneo { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public int Estado { get; set; }  // 0: Pendiente, 1: En curso, 2: Finalizado

        public int IdCreador { get; set; }  // FK a tabla de Usuarios o Admins

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
       
    }
}
