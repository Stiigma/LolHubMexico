using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.Entities.ScrimLog
{
    public class ScrimLog
    {
        [Key]
        public int IdLogScm { get; set; }

        public int IdScrim { get; set; }
        public string MatchId { get; set; } = string.Empty;

        public string GeminiAnalysisJson { get; set; } = string.Empty;

        public DateTime LogDate { get; set; } = DateTime.Now;
    }
}
