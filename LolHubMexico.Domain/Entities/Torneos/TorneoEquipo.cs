using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.Entities.Torneos
{
    public class TorneoEquipo
    {
        [Key]
        public int idTorneEquipo { get; set; }
        public int IdTorneo { get; set; }
        public int IdEquipo { get; set; }
        
        public bool status {  get; set; }
       
        
    }
}
