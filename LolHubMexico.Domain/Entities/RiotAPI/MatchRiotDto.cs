using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LolHubMexico.Domain.Entities.RiotAPI
{
    public class MatchRiotDto
    {
        public Metadata metadata { get; set; } = new Metadata();
        public Info info { get; set; } = new Info();
    }

    public class Metadata
    {
        public string matchId { get; set; } = "";
        public List<string> participants { get; set; } = new();
    }

    public class Info
    {
        public long gameCreation { get; set; }
        public int gameDuration { get; set; }
        public string gameMode { get; set; } = "";
        public string gameVersion { get; set; } = "";
        public List<Participant> participants { get; set; } = new();
        public List<TeamAPI> teams { get; set; } = new();
    }

    public class Participant
    {
        public string puuid { get; set; } = "";
        public string summonerName { get; set; } = "";
        public string championName { get; set; } = "";
        public string teamPosition { get; set; } = "";
        public int kills { get; set; }
        public int totalDamageDealtToChampions { get; set; }

        public int deaths { get; set; }
        public int assists { get; set; }
        public int goldEarned { get; set; }
        public int totalMinionsKilled { get; set; }
        public int visionScore { get; set; }
        public int champLevel { get; set; }
        public List<int> itemIds { get; set; } = new();
        public int teamId { get; set; }
    }

    public class TeamAPI
    {
        public int teamId { get; set; }
        public bool win { get; set; }
        public Objectives objectives { get; set; } = new Objectives();
    }

    public class Objectives
    {
        public Objective dragon { get; set; } = new Objective();
        public Objective baron { get; set; } = new Objective();
        public Objective herald { get; set; } = new Objective();
        public Objective tower { get; set; } = new Objective();
        public Objective inhibitor { get; set; } = new Objective();
    }

    public class Objective
    {
        public int kills { get; set; }
    }

}
