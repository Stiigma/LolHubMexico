using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.Entities.Torneos
{
    public class TorneoScrim
    {
        [Key]
        public int idTorneoScrim { get; set; }
        public int IdTorneo { get; set; }
        public int IdScrim { get; set; }

        public bool status { get; set; }
    }
}
