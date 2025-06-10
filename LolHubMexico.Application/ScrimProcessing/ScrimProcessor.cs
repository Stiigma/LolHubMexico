using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Application.Interfaces;
using LolHubMexico.Domain.Entities.RiotAPI;
using LolHubMexico.Domain.Entities.Scrims;
using LolHubMexico.Application.Exceptions;
using LolHubMexico.Domain.Repositories.ScrimRepository;
using LolHubMexico.Domain.Entities.DatailsScrims;
using LolHubMexico.Domain.Enums;
using LolHubMexico.Domain.Entities.MatchDetails;
using LolHubMexico.Domain.Repositories.MatchRepository;

namespace LolHubMexico.Application.ScrimProcessing
{
    public class ScrimProcessor : IScrimProcessor
    {
        private readonly IRiotService _riotMatchService;
        private readonly IDetailsScrimRepository _detailsScrimRepository;
        private readonly IScrimRepository _scrimRepository;
        private readonly IMatchDetailsRepository _matchDetailsRepository;

        public ScrimProcessor(IRiotService riotMatchService, IDetailsScrimRepository detailsScrimRepository, IScrimRepository scrimRepository, IMatchDetailsRepository matchDetailsRepository)
        {
            _riotMatchService = riotMatchService;
            _detailsScrimRepository = detailsScrimRepository;
            _scrimRepository = scrimRepository;
            _matchDetailsRepository = matchDetailsRepository;
        }

        public async Task ProcessAsync(Scrim scrim, string idMatch)
        {
            if (string.IsNullOrEmpty(idMatch))
            {
                Console.WriteLine("❌ Scrim sin idMatch válido");
                return;
            }

            // Obtener partida desde API
            var match = await _riotMatchService.GetStatsByMatchIdAsync(idMatch);
            if (match == null)
                throw new AppException("No existe esta idMatch en la API de Riot");

            // Obtener detalles registrados de la scrim en BD
            var detallesBD = await _detailsScrimRepository.GetDetailsByIdScrim(scrim.idScrim);
            Console.WriteLine($"📋 Jugadores registrados en la scrim: {detallesBD.Count}");

            var detallesQueue = new Queue<DetailsScrim>(detallesBD);
            var participantesApi = match.info.participants;

            var validados = new HashSet<string>();
            var totalDamageByTeam = participantesApi
                .GroupBy(p => p.teamId)
                .ToDictionary(g => g.Key, g => g.Sum(p => p.totalDamageDealtToChampions));

            // Map para guardar puuid de referencia por equipo en DB
            var teamMap = new Dictionary<int, string>();

            // 🔁 Procesar participantes
            foreach (var participante in participantesApi)
            {
                bool encontrado = false;
                int intentos = detallesQueue.Count;

                while (intentos-- > 0)
                {
                    var detalle = detallesQueue.Dequeue();

                    if (detalle.puuid == participante.puuid)
                    {
                        int teamId = participante.teamId;
                        int damageTotalEquipo = totalDamageByTeam[teamId];

                        // Mapear un solo puuid por equipo (como referencia)
                        if (!teamMap.ContainsKey(detalle.idTeam))
                            teamMap[detalle.idTeam] = participante.puuid;

                        validados.Add(participante.puuid);
                        Console.WriteLine($"✔ Match encontrado: {participante.summonerName}");

                        detalle.idMatch = idMatch;
                        detalle.carril = participante.teamPosition;
                        detalle.teamDamagePercentage = damageTotalEquipo > 0
                            ? ((float)participante.totalDamageDealtToChampions / damageTotalEquipo * 100).ToString("0.00") + "%"
                            : "0.00%";
                        detalle.kills = participante.kills;
                        detalle.deaths = participante.deaths;
                        detalle.assists = participante.assists;
                        detalle.goldEarned = participante.goldEarned;
                        detalle.farm = participante.totalMinionsKilled;
                        detalle.visionScore = participante.visionScore;
                        detalle.championName = participante.championName;
                        detalle.nivel = participante.champLevel;
                        detalle.items = string.Join(",", participante.itemIds);

                        Console.WriteLine($"  → Datos cargados correctamente");
                        await _detailsScrimRepository.UpdateDetailsScrimAsync(detalle);
                        encontrado = true;
                        break;
                    }
                    else
                    {
                        detallesQueue.Enqueue(detalle);
                    }
                }

                if (!encontrado)
                {
                    Console.WriteLine($"✖ No se encontró en BD: {participante.summonerName} ({participante.puuid})");
                }
            }

            // ✅ Determinar equipo ganador usando puuid validado
            var participanteValido = participantesApi.FirstOrDefault(p => validados.Contains(p.puuid));
            var detalleValido = detallesBD.FirstOrDefault(d => d.puuid == participanteValido?.puuid);

            if (participanteValido != null && detalleValido != null)
            {
                int riotTeamIdValido = participanteValido.teamId;
                int dbTeamIdValido = detalleValido.idTeam;

                // Mapeo preciso Riot → DB
                var riotToDbMap = new Dictionary<int, int>
        {
            { riotTeamIdValido, dbTeamIdValido },
            { riotTeamIdValido == 100 ? 200 : 100, dbTeamIdValido == scrim.idTeam1 ? scrim.idTeam2 : scrim.idTeam1 }
        };

                var equipoGanador = match.info.teams.FirstOrDefault(t => t.win);
                if (equipoGanador != null)
                {
                    int ganadorDbId = riotToDbMap[equipoGanador.teamId];
                    scrim.result = ganadorDbId.ToString();
                    Console.WriteLine($"🏆 Ganó el equipo con ID en BD: {ganadorDbId} (Riot teamId: {equipoGanador.teamId})");
                }
                else
                {
                    scrim.result = "Empate o sin información";
                }
            }
            else
            {
                scrim.result = "No se pudo determinar el equipo ganador";
            }

            // 🧩 Procesar los que quedaron sin hacer match
            foreach (var detallePendiente in detallesQueue)
            {
                int teamAsignado = detallePendiente.idTeam;

                if (!teamMap.TryGetValue(teamAsignado, out string puuidReferencia))
                {
                    Console.WriteLine($"⚠ No se encontró puuid de referencia para equipo {teamAsignado}");
                    continue;
                }

                var jugadorReferencia = participantesApi.FirstOrDefault(p => p.puuid == puuidReferencia);
                if (jugadorReferencia == null)
                {
                    Console.WriteLine($"❌ El puuid de referencia no existe en la API: {puuidReferencia}");
                    continue;
                }

                Console.WriteLine($"🧩 Asignando datos aproximados al jugador sin puuid válido del equipo {teamAsignado}");

                detallePendiente.idMatch = idMatch;
                detallePendiente.carril = jugadorReferencia.teamPosition;
                detallePendiente.teamDamagePercentage = "0.00%";
                detallePendiente.kills = jugadorReferencia.kills;
                detallePendiente.deaths = jugadorReferencia.deaths;
                detallePendiente.assists = jugadorReferencia.assists;
                detallePendiente.goldEarned = jugadorReferencia.goldEarned;
                detallePendiente.farm = jugadorReferencia.totalMinionsKilled;
                detallePendiente.visionScore = jugadorReferencia.visionScore;
                detallePendiente.championName = jugadorReferencia.championName;
                detallePendiente.nivel = jugadorReferencia.champLevel;
                detallePendiente.items = string.Join(",", jugadorReferencia.itemIds);

                Console.WriteLine($"  → Datos aproximados asignados desde referencia");

                await _detailsScrimRepository.UpdateDetailsScrimAsync(detallePendiente);
            }
            scrim.status = (int)ScrimStatus.Completed;
            // Si quieres guardar cambios en batch aquí puedes hacerlo ahora
            await _scrimRepository.UpdateScrim(scrim);
            await SaveMatchDetailsAsync(scrim.idScrim, match);
            // foreach (...) await _detailsScrimRepository.UpdateDetailsScrimAsync(detalle);
        }

        private async Task SaveMatchDetailsAsync(int idScrim, MatchRiotDto match)
        {
            var team1 = match.info.teams.FirstOrDefault(t => t.teamId == 100);
            var team2 = match.info.teams.FirstOrDefault(t => t.teamId == 200);

            if (team1 == null || team2 == null)
            {
                Console.WriteLine("❌ No se encontraron ambos equipos en la partida");
                return;
            }

            var existing = await _matchDetailsRepository.GetByScrimIdAsync(idScrim);

            if (existing == null)
            {
                var newEntry = new MatchDetail
                {
                    IdScrim = idScrim,
                    GameDuration = match.info.gameDuration,
                    GameMode = match.info.gameMode,
                    GameVersion = match.info.gameVersion,
                    TowersTeam1 = team1.objectives.tower.kills,
                    TowersTeam2 = team2.objectives.tower.kills
                };

                await _matchDetailsRepository.CreateAsync(newEntry);
                Console.WriteLine("📝 MatchDetails creado");
            }
            else
            {
                // Actualizar campos sin tocar el ID
                existing.GameDuration = match.info.gameDuration;
                existing.GameMode = match.info.gameMode;
                existing.GameVersion = match.info.gameVersion;
                existing.TowersTeam1 = team1.objectives.tower.kills;
                existing.TowersTeam2 = team2.objectives.tower.kills;

                await _matchDetailsRepository.UpdateAsync(existing);
                Console.WriteLine("🔁 MatchDetails actualizado");
            }
        }


    }

}
