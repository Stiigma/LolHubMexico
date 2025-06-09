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

        public int idTeam2 { get; set; } = 0;

        public DateTime scheduled_date { get; set; }

        public List<int> idsUsers  { get; set; } = new List<int>();

        public string description { get; set; }

        public string tittle { get; set; }

    }
}
