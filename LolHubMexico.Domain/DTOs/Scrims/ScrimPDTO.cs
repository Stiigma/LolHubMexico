using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.DTOs.Scrims
{
    public class ScrimPDTO
    {
        public int idScrim {  get; set; }

        public int idTeam1 { get; set; }


        public int idTeam2 { get; set; } = 0;
        public DateTime scheduled_date { get; set; }

        public int status { get; set; }
    }
}
