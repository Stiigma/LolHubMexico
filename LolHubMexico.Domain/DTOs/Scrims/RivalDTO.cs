using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.DTOs.Scrims
{
    public class RivalDTO
    {
        public int idScrim { get; set; }
        public int idrival { get; set; }
        public bool IsAccept {  get; set; } = false;
    }
}
