using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.Entities.Scrims
{
    public class Scrim
    {
        [Key]
        public int idScrim {  get; set; }

        public int created_by { get; set; }

        public int idTeam1 { get; set; }

        public int idTeam2 { get; set; }

        public DateTime scheduled_date { get; set; }

        public int status { get; set; }

        public string result { get; set; }

        public DateTime created_at { get; set; }
    }
}
