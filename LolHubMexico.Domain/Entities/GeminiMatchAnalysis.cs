using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.Entities
{
    public class GeminiMatchAnalysis
    {
        public KeyEvents keyEvents { get; set; }
        public List<DragonEvent> allDragons { get; set; }  // ✅ NUEVO
        public List<MajorFight> majorFights { get; set; }
    }

    public class KeyEvents
    {
        public FirstDragon? firstDragon { get; set; }
        public FirstBaron? firstBaron { get; set; }
        public FirstHerald? firstHerald { get; set; }
        public FirstTower? firstTower { get; set; }
    }

    public class FirstDragon
    {
        public string time { get; set; }
        public int? team { get; set; }  // <- Cambiado a nullable
        public string type { get; set; }
    }

    public class FirstBaron
    {
        public string time { get; set; }
        public int? team { get; set; }  // <- Cambiado a nullable
    }

    public class FirstHerald
    {
        public string time { get; set; }
        public int? team { get; set; }  // <- Cambiado a nullable
    }

    public class FirstTower
    {
        public string time { get; set; }
        public int? team { get; set; }  // <- Cambiado a nullable
        public string lane { get; set; }
    }

    public class DragonEvent
    {
        public string time { get; set; }
        public int? team { get; set; }  // <- Cambiado a nullable
        public string type { get; set; }
    }

    public class MajorFight
    {
        public string id { get; set; }
        public long timestampMillisStart { get; set; }
        public long timestampMillisEnd { get; set; }
        public string timeStart { get; set; }
        public string timeEnd { get; set; }
        public string location { get; set; }
        public int totalKills { get; set; }
        public List<KillDetail> killsDetails { get; set; }
    }

    public class KillDetail
    {
        public int killerId { get; set; }
        public string killerPuuid { get; set; }
        public int victimId { get; set; }
        public string victimPuuid { get; set; }
        public List<string> assists { get; set; }
        public VictimPosition victimPosition { get; set; }
    }


    public class VictimPosition
    {
        public int x { get; set; }
        public int y { get; set; }
    }

}
