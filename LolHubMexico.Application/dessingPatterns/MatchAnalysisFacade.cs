using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LolHubMexico.Application.Interfaces;
using LolHubMexico.Domain.Entities;
using LolHubMexico.Domain.Entities.RiotAPI;

namespace LolHubMexico.Application.dessingPatterns
{
    public class MatchAnalysisFacade : IMatchAnalysisFacade
    {
        private readonly IRiotService _riotApiService;
        private readonly IGeminiApiService _geminiApiService;

        public MatchAnalysisFacade(IRiotService riotApiService, IGeminiApiService geminiApiService)
        {
            _riotApiService = riotApiService ?? throw new ArgumentNullException(nameof(riotApiService));
            _geminiApiService = geminiApiService ?? throw new ArgumentNullException(nameof(geminiApiService));
        }

        public async Task<GeminiMatchAnalysis?> GetGeminiMatchAnalysisJsonAsync(string matchId)
        {
            if (string.IsNullOrEmpty(matchId))
            {
                throw new ArgumentException("El ID de la partida no puede ser nulo o vacío.", nameof(matchId));
            }

            // Paso 1: Llamar al servicio de Riot para obtener la línea de tiempo
            // Asumiendo que GetMatchTimelineAsync y GetMatchDetailsAsync existen y funcionan
            TimelineRiotDto? timelineDto = await _riotApiService.GetMatchTimelineAsync(matchId, "americas");
            MatchRiotDto? matchDto = await _riotApiService.GetStatsByMatchIdAsync(matchId);

            if (timelineDto == null || matchDto == null)
            {
                // Manejo de error si no se pueden obtener los datos de Riot
                // Podrías lanzar una excepción específica o devolver null/un mensaje de error.
                return null;
            }

            // Paso 2: Llamar al servicio de Gemini para procesar los eventos
            string?
                geminiAnalysisJson = await _geminiApiService.ProcessMatchEventsWithGemini(timelineDto, matchDto);
            var cleanJson = CleanGeminiJson(geminiAnalysisJson);
          
            // Paso 3: Opcional - Post-procesamiento o validación de la respuesta de Gemini si es necesario
            // Paso 3: Deserializar el JSON de Gemini a un objeto C#
            try
            {
                // Asegúrate de que el JSON sea válido antes de intentar deserializar
                // El método ProcessMatchEventsWithGemini ya limpia "```json" y "```"
                var analysis = JsonSerializer.Deserialize<GeminiMatchAnalysis>(cleanJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return analysis;
            }
            catch (JsonException ex)
            {
                // Manejar errores de deserialización (ej. el JSON no tiene el formato esperado)
                Console.WriteLine($"Error al deserializar la respuesta de Gemini: {ex.Message}");
                // Loguear el JSON problemático para depuración si es necesario
                // Console.WriteLine($"JSON problemático: {geminiAnalysisJson}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado al procesar la respuesta de Gemini: {ex.Message}");
                return null;
            }         
        }

        private string CleanGeminiJsonlts(List<List<string>> parts)
        {
            if (parts == null || parts.Count != 3)
                throw new ArgumentException("El resultado de Gemini no contiene exactamente tres secciones.");

            string keyEvents = string.Join("\n", parts[0]);
            string allDragons = string.Join("\n", parts[1]);
            string majorFights = string.Join("\n", parts[2]);

                        string json = $@"
            {{
              ""keyEvents"": {keyEvents},
              ""allDragons"": {allDragons},
              ""majorFights"": {majorFights}
            }}";

                        return json;
        }
        private string CleanGeminiJson(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return "";

            // Elimina bloques que inicien con ```json y terminen en ```
            if (raw.StartsWith("```json") && raw.EndsWith("```"))
                return raw.Substring(7, raw.Length - 10).Trim();

            // Fallback adicional: elimina backticks sueltos
            return raw.Replace("```", "").Trim();
        }
    }
}
