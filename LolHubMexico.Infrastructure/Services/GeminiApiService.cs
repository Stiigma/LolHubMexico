using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LolHubMexico.Application.Interfaces;
using LolHubMexico.Domain.Entities;
using LolHubMexico.Domain.Entities.RiotAPI;
using LolHubMexico.Domain.Entities.Teams;
using Microsoft.Extensions.Configuration;


namespace LolHubMexico.Infrastructure.Services
{
    public class GeminiApiService : IGeminiApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _geminiApiKey;
        private readonly string _modelName;

        public GeminiApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            _geminiApiKey = configuration["Gemini:ApiKey"]
                ?? throw new ArgumentNullException("ApiGemini no configurado en la configuración. Por favor, revisa appsettings.json.");
            _modelName = configuration["Gemini:ModelId"] ?? "gemini-2.0-flash"; // Usa gemini-2.0-flash por defecto como en Dart
        }

        public async Task<string?> ProcessMatchEventsWithGemini(TimelineRiotDto timelineDto, MatchRiotDto matchDto)
        {
            if (timelineDto == null || matchDto == null)
                throw new ArgumentNullException("timelineDto y matchDto no pueden ser null.");

            var participantIdToPuuidMap = matchDto.info.participants
                .GroupBy(p => p.summonerName)
                .ToDictionary(g => g.Key, g => g.First().puuid);

            var puuidToSummonerNameMap = matchDto.info.participants
                .GroupBy(p => p.puuid)
                .ToDictionary(g => g.Key, g => g.First().summonerName);

            string GetSummonerName(int? participantId)
            {
                if (!participantId.HasValue) return "Unknown";
                if (participantIdToPuuidMap.TryGetValue(participantId.Value.ToString(), out var puuid))
                {
                    if (puuidToSummonerNameMap.TryGetValue(puuid, out var summonerName))
                        return summonerName;
                }
                return $"Unknown (ID: {participantId.Value})";
            }

            List<string>? GetAssistNames(List<int>? assistIds)
            {
                if (assistIds == null) return null;
                return assistIds.Select(id => GetSummonerName(id)).ToList();
            }

            var relevantEvents = timelineDto.info.frames
                .SelectMany(frame => frame.events)
                .Where(e =>
                    e.type == "CHAMPION_KILL" ||
                    e.type == "ELITE_MONSTER_KILL" ||
                    e.type == "BUILDING_KILL"
                )
                .Select(e => new
                {
                    e.timestamp,
                    e.type,
                    e.killerId,
                    KillerName = GetSummonerName(e.killerId),
                    e.victimId,
                    VictimName = GetSummonerName(e.victimId),
                    Assists = GetAssistNames(e.assists),
                    e.position,
                    e.buildingType,
                    e.laneType,
                    e.towerType,
                    e.monsterType,
                    e.monsterSubType,
                    e.teamId
                })
                .ToList();

            var inputData = new
            {
                matchId = timelineDto.metadata.matchId,
                gameDurationMillis = matchDto.info.gameDuration,
                participantsPuuidToNameMapping = puuidToSummonerNameMap,
                events = relevantEvents
            };

            var inputJson = JsonSerializer.Serialize(inputData);

            string prompt = $@"
                Analiza los siguientes eventos de una partida de League of Legends.

                Aquí tienes el mapeo de PUUIDs a nombres de invocador para referencia:
                {JsonSerializer.Serialize(puuidToSummonerNameMap, new JsonSerializerOptions { WriteIndented = false })}.

                Aquí tienes el mapeo de IDs de participante (1-10) a PUUIDs:
                {JsonSerializer.Serialize(participantIdToPuuidMap, new JsonSerializerOptions { WriteIndented = false })}.

                Los eventos de la partida están en este JSON:
                {inputJson}

                Tu tarea es:
                1.  **Identificar eventos clave:** Primer dragón, primer Barón, primer Heraldo y primera torre.
                2.  **Para cada objetivo clave:** Determina el ""team"" (como número: 100 o 200) que tomó el objetivo. Utiliza el 'killerId' del evento y el mapeo de 'participantId' a PUUID para identificar al equipo. Si el evento tiene 'teamId' directamente, úsalo.
                3.  **Todos los dragones:** Incluye *todos* los dragones de la partida en una lista llamada `allDragons`, con su tiempo (`time`), tipo (`type`, ej. 'fire', 'water', 'cloud', 'ocean', 'elder'), y el equipo (`team` 100 o 200) que lo aseguró.
                4.  **Peleas grandes (Major Fights):** Agrupa los eventos de 'CHAMPION_KILL' que ocurran en una proximidad cercana (considera que las posiciones 'x' e 'y' con una diferencia de +/- 1000 unidades son 'cercanas') y dentro de un lapso de 15 segundos. Identifica solo las **4 peleas más importantes** (aquellas con el mayor número total de muertes).
                5.  **Detalles de las Muertes en Peleas:** En cada objeto de `killsDetails` dentro de `majorFights`, incluye el `killerId` (numérico), el `killerPuuid`, el `victimId` (numérico), el `victimPuuid`, y una lista de `assists` (una lista de PUUIDs). También incluye la `victimPosition`.

                **Formato de salida JSON estricto:**
                ❗ Solo responde con un JSON válido que siga exactamente este formato, sin texto adicional, explicaciones o ```json.
                ```json
                {{
                    ""keyEvents"": {{
                        ""firstDragon"": {{ ""time"": ""MM:SS"", ""team"": 100, ""type"": ""fire"" }},
                        ""firstBaron"": {{ ""time"": ""MM:SS"", ""team"": 200 }},
                        ""firstHerald"": {{ ""time"": ""MM:SS"", ""team"": 100 }},
                        ""firstTower"": {{ ""time"": ""MM:SS"", ""team"": 200, ""lane"": ""TOP_LANE"" }}
                    }},
                    ""allDragons"": [
                        {{
                            ""time"": ""MM:SS"",
                            ""team"": 100,
                            ""type"": ""ocean""
                        }},
                        {{
                            ""time"": ""MM:SS"",
                            ""team"": 200,
                            ""type"": ""cloud""
                        }}
                    ],
                    ""majorFights"": [
                        {{
                            ""id"": ""fight_1"",
                            ""timestampMillisStart"": 123456,
                            ""timestampMillisEnd"": 123999,
                            ""timeStart"": ""02:03"",
                            ""timeEnd"": ""02:18"",
                            ""location"": ""River near mid lane"",
                            ""totalKills"": 3,
                            ""killsDetails"": [
                                {{
                                    ""killerId"": 4,
                                    ""killerPuuid"": ""puuid_killer_1"",
                                    ""victimId"": 7,
                                    ""victimPuuid"": ""puuid_victim_1"",
                                    ""assists"": [""puuid_assist_1"", ""puuid_assist_2""],
                                    ""victimPosition"": {{ ""x"": 1234, ""y"": 5678 }}
                                }},
                                {{
                                    ""killerId"": 1,
                                    ""killerPuuid"": ""puuid_killer_2"",
                                    ""victimId"": 9,
                                    ""victimPuuid"": ""puuid_victim_2"",
                                    ""assists"": [""puuid_assist_3""],
                                    ""victimPosition"": {{ ""x"": 1250, ""y"": 5700 }}
                                }}
                            ]
                        }}
                        // Se pueden añadir hasta 4 objetos de peleas más importantes aquí
                    ]
             }}";



            var requestBody = new
            {
                contents = new[]
                {
                new
                {
                    parts = new[] { new { text = prompt } }
                }
            },
                generationConfig = new
                {
                    temperature = 0.2f,
                    maxOutputTokens = 2048
                }
            };


            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var fullUrl = $"https://generativelanguage.googleapis.com/v1beta/models/{_modelName}:generateContent?key={_geminiApiKey}";
            var response = await _httpClient.PostAsync(fullUrl, jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"❌ Gemini API error: {response.StatusCode} - {error}");
                return null;
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine("===== 🔄 Respuesta RAW de Gemini =====");
            Console.WriteLine(responseBody);
            Console.WriteLine("======================================");

            string rawText = null;
            using (JsonDocument doc = JsonDocument.Parse(responseBody))
            {
                if (doc.RootElement.TryGetProperty("candidates", out var candidatesElement) &&
                    candidatesElement.EnumerateArray().Any())
                {
                    var firstCandidate = candidatesElement.EnumerateArray().First();
                    if (firstCandidate.TryGetProperty("content", out var contentElement) &&
                        contentElement.TryGetProperty("parts", out var partsElement) &&
                        partsElement.EnumerateArray().Any())
                    {
                        var firstPart = partsElement.EnumerateArray().First();
                        if (firstPart.TryGetProperty("text", out var textElement))
                        {
                            rawText = textElement.GetString();
                        }
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(rawText))
                return null;

            // Limpiar ```json al inicio y ``` al final si lo incluye
            if (rawText.StartsWith("```json") && rawText.EndsWith("```"))
                rawText = rawText[7..^3].Trim();

            // ✅ Mostrar el JSON limpio antes de retornarlo
            Console.WriteLine("===== ✅ JSON limpio generado por Gemini =====");
            Console.WriteLine(rawText);
            Console.WriteLine("==============================================");

            return rawText;
        }

        private string FormatMillisecondsToMinutesSeconds(long milliseconds)
        {
            var time = TimeSpan.FromMilliseconds(milliseconds);
            return $"{(int)time.TotalMinutes:D2}:{time.Seconds:D2}";
        }
    }

}
