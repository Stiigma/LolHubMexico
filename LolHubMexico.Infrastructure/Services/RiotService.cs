using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Application.Interfaces;
using LolHubMexico.Domain.DTOs.Players;
using LolHubMexico.Domain.Entities.RiotAPI;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LolHubMexico.Infrastructure.Services
{
    public class RiotService : IRiotService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public RiotService(IConfiguration config)
        {
            _httpClient = new HttpClient();
            _apiKey = config["RiotApiKey"]; // Desde appsettings.Development.json
        }

        public async Task<RiotAccountDTO> GetSummonerByNameAsync(string region, string summonerName)
        {
            var url = $"https://americas.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{summonerName}/{region}?api_key={_apiKey}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(content); // Usa Newtonsoft.Json

            return new RiotAccountDTO
            {
                Puuid = data.puuid,
                GameName = data.gameName,
                TagLine = data.tagLine,
            };
        }

        public async Task<RiotSummonerDTO> GetSummonerByPuiid(string puiid)
        {
            var url = $"https://americas.api.riotgames.com/riot/account/v1/accounts/by-puuid/{puiid}?api_key={_apiKey}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(content);

            return new RiotSummonerDTO
            {
                SummonerId = data.id ?? "",
                Puuid = data.puuid ?? "",
                AccountId = data.accountId ?? "",
                ProfileIconId = data.profileIconId ?? 0,       // int
                SummonerLevel = data.summonerLevel ?? 0         // int
            };

        }

        public async Task<MatchRiotDto?> GetStatsByMatchIdAsync(string matchId)
        {
            var url = $"https://americas.api.riotgames.com/lol/match/v5/matches/{matchId}?api_key={_apiKey}";
            var response = await _httpClient.GetAsync(url);


            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"❌ Error al obtener match {matchId}: {response.StatusCode}");
                return new MatchRiotDto(); // o null si prefieres
            }

            var content = await response.Content.ReadAsStringAsync();

            var matchData = JsonConvert.DeserializeObject<MatchRiotDto>(content);

            //// Mostrar datos principales
            //Console.WriteLine($"✅ Match ID: {matchData.metadata.matchId}");
            //Console.WriteLine($"Duración: {matchData.info.gameDuration} segundos");
            //Console.WriteLine($"Modo: {matchData.info.gameMode}");
            //Console.WriteLine($"Versión: {matchData.info.gameVersion}");

            //foreach (var team in matchData.info.teams)
            //{
            //    Console.WriteLine($" Dragones: {team.objectives.dragon?.kills ?? 0}");
            //    Console.WriteLine($" Barones: {team.objectives.baron?.kills ?? 0}");
            //    Console.WriteLine($" Heraldos: {team.objectives.herald?.kills ?? 0}");
            //    Console.WriteLine($" Torres: {team.objectives.tower?.kills ?? 0}");
            //}

            //foreach (var p in matchData.info.participants)
            //{
            //    Console.WriteLine($"👤 {p.summonerName} ({p.championName})");
            //    Console.WriteLine($"  KDA: {p.kills}/{p.deaths}/{p.assists}");
            //    Console.WriteLine($"  Oro: {p.goldEarned} | Farm: {p.totalMinionsKilled} | Visión: {p.visionScore}");
            //}

            return matchData;
        }
    }

}
