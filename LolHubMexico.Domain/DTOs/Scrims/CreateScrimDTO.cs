using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.DTOs.Scrims
{
    public class CreateScrimDTO
    {
        public int created_by { get; set; }

        public int idTeam1 { get; set; }

        public DateTime scheduled_date { get; set; }

    }
}
