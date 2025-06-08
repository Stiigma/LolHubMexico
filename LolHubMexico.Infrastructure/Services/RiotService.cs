using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Application.Interfaces;
using LolHubMexico.Domain.DTOs.Players;
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
            var url = $"https://la1.api.riotgames.com/lol/summoner/v4/summoners/by-puuid/{puiid}?api_key={_apiKey}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(content);

            return new RiotSummonerDTO
            {
                SummonerId = data.id,
                Puuid = data.puuid,
                AccountId = data.accountId,
                ProfileIconId = data.profileIconId,
                SummonerLevel = data.summonerLevel,
            };

        }
    }

}
