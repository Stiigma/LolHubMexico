using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.DTOs.Scrims
{
    public class ScrimResultReportDTO
    {
        public int IdScrim { get; set; }
        public int IdTeam { get; set; }
        public int IdUser { get; set; }
        public string IdMatch { get; set; } = string.Empty;
        public bool Win { get; set; } // El ID del equipo que ganó
        public string? ImagePath { get; set; } // Opcional, puede ser null o string.Empty
    }
}
