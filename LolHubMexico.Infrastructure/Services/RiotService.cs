using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
         
            return matchData;
        }


        public async Task<TimelineRiotDto?> GetMatchTimelineAsync(string matchId, string region)
        {
            // Ajusta la URL base según la región (por ejemplo, "americas", "europe", "asia")
            // La API de timeline está en la región "regional" (americas, europe, asia), no en "platform" (na1, euw1, etc.)
            string baseUrl = $"https://americas.api.riotgames.com";
            string requestUri = $"https://americas.api.riotgames.com/lol/match/v5/matches/{matchId}/timeline?api_key={_apiKey}";            

            try
            {
                var response = await _httpClient.GetAsync(requestUri);
                response.EnsureSuccessStatusCode(); // Lanza una excepción para errores HTTP

                var jsonString = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Esto es crucial para que coincida con camelCase de JSON y PascalCase de C#
                };

                return System.Text.Json.JsonSerializer.Deserialize<TimelineRiotDto>(jsonString, options);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error al obtener la línea de tiempo del match: {e.Message}");
                // Puedes manejar el error más detalladamente aquí
                return null;
            }
            catch (System.Text.Json.JsonException e)
            {
                Console.WriteLine($"Error de deserialización JSON: {e.Message}");
                return null;
            }
        }
    }

}
