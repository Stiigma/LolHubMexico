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
        public string tittle {  get; set; }
        //public string idMatch { get; set; } = string.Empty;

        public string description { get; set; }

        public int idTeam1 { get; set; }

        public int idTeam2 { get; set; }

        public DateTime scheduled_date { get; set; }

        public int status { get; set; }

        public string result { get; set; } = string.Empty;

        public DateTime created_at { get; set; }

        public bool? team1_result_reported { get; set; }
        public bool? team2_result_reported { get; set; }
        public DateTime? team1_reported_at { get; set; }
        public DateTime? team2_reported_at { get; set; }
        public string result_verification { get; set; } // pending, agreed, disputed, api-verified

        public string? imagePath1 { get; set; } = string.Empty;

        public string? imagePath2 { get; set; } = string.Empty;

        public string? idMatch1 { get; set; } 

        public string? idMatch2 { get; set; }
    }
}
