using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Application.ScrimProcessing;
using LolHubMexico.Domain.Enums;
using LolHubMexico.Domain.Repositories.ScrimRepository;

namespace LolHubMexico.Application.dessingPatterns
{
    public class ScrimObserver : IScrimObserver
    {
        private readonly IScrimRepository _scrimRepository;
        private readonly ScrimProcessingQueue _processingQueue;

        public ScrimObserver(IScrimRepository scrimRepository, ScrimProcessingQueue processingQueue)
        {
            _scrimRepository = scrimRepository;
            _processingQueue = processingQueue;
        }

        public async Task VerificarScrimsPendientesAsync()
        {
            var scrimsPorVerificar = await _scrimRepository.GetScrimsPorEstadoAsync((int)ScrimStatus.Confirmed);
            var ensenadaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var nowInEnsenada = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, ensenadaTimeZone);

            Console.WriteLine($"\n🕵 Verificando scrims pendientes a las {nowInEnsenada} (hora de Ensenada)\n");

            foreach (var scrim in scrimsPorVerificar)
            {
                Console.WriteLine($"🔎 Scrim ID {scrim.idScrim} | Fecha programada: {scrim.scheduled_date}");

                // ¿Ya pasó la hora programada?
                if (scrim.scheduled_date <= nowInEnsenada)
                {
                    Console.WriteLine($"✅ Scrim ID {scrim.idScrim} ya debería haber comenzado. Cambiando estado a InProgress...");

                    scrim.status = (int)ScrimStatus.InProgress;
                    await _scrimRepository.UpdateScrim(scrim);

                    Console.WriteLine($"🟢 Estado actualizado exitosamente.\n");
                }
                else
                {
                    Console.WriteLine($"🕓 Scrim ID {scrim.idScrim} aún no empieza. (Falta: {(scrim.scheduled_date - nowInEnsenada).TotalMinutes:N0} min)\n");
                }
            }

            Console.WriteLine("✅ Verificación completada.\n");
        }

        public async Task CancelarScrimsInactivasAsync()
        {
            var scrimsEnCurso = await _scrimRepository.GetScrimsPorEstadoAsync((int)ScrimStatus.InProgress);
            var ensenadaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var nowInEnsenada = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, ensenadaTimeZone);

            Console.WriteLine($"\n🕵 Verificando scrims activas a las {nowInEnsenada} (hora de Ensenada)\n");

            foreach (var scrim in scrimsEnCurso)
            {
                // Hora programada
                var fechaInicio = scrim.scheduled_date;

                // Cuánto tiempo ha pasado desde que debió iniciar
                var tiempoTranscurrido = nowInEnsenada - fechaInicio;

                Console.WriteLine($"🔎 Scrim ID {scrim.idScrim} programada para: {fechaInicio} | Han pasado: {tiempoTranscurrido.TotalMinutes:N0} min");

                if (tiempoTranscurrido.TotalHours >= 2)
                {
                    Console.WriteLine($"❌ Scrim ID {scrim.idScrim} lleva más de 2 horas sin procesarse. Será cancelada.");

                    scrim.status = (int)ScrimStatus.Cancelled; // Estado cancelada
                    await _scrimRepository.UpdateScrim(scrim);

                    Console.WriteLine($"🛑 Scrim ID {scrim.idScrim} actualizada a estado Cancelled\n");
                }
                else
                {
                    Console.WriteLine($"⏳ Scrim ID {scrim.idScrim} aún está dentro del límite de 2 horas.\n");
                }
            }

            Console.WriteLine("✅ Verificación de scrims activas completada.\n");
        }

        public async Task VerificarScrimsReportadasAsync()
        {
            var scrimsEnProceso = await _scrimRepository.GetScrimsPorEstadoAsync((int)ScrimStatus.InProgress);

            Console.WriteLine($"\n📋 Iniciando verificación de scrims reportadas (total: {scrimsEnProceso.Count})\n");

            foreach (var scrim in scrimsEnProceso)
            {
                Console.WriteLine($"🔍 Evaluando Scrim ID: {scrim.idScrim}");

                bool team1Respondio = scrim.team1_reported_at.HasValue && scrim.team1_result_reported.HasValue;
                bool team2Respondio = scrim.team2_reported_at.HasValue && scrim.team2_result_reported.HasValue;

                Console.WriteLine($"   🟦 Team 1 respondió: {team1Respondio} | Valor: {scrim.team1_result_reported}");
                Console.WriteLine($"   🟥 Team 2 respondió: {team2Respondio} | Valor: {scrim.team2_result_reported}");

                if (!team1Respondio || !team2Respondio)
                {
                    Console.WriteLine($"⏳ Scrim ID {scrim.idScrim} aún no tiene respuesta de ambos equipos.\n");
                    continue;
                }

                // Ambos equipos ya respondieron
                if (scrim.team1_result_reported == scrim.team2_result_reported)
                {
                    Console.WriteLine($"⚠ Scrim ID {scrim.idScrim} está en disputa ❌ (ambos equipos reportaron el mismo resultado).");

                    scrim.status = 10; // Estado: En disputa
                    scrim.result_verification = "Disputed";

                    await _scrimRepository.UpdateScrim(scrim);
                    Console.WriteLine($"📌 Estado actualizado a 10 (Disputed)\n");
                }
                else
                {
                    Console.WriteLine($"✅ Scrim ID {scrim.idScrim} tiene resultados opuestos. Marcada para verificación manual 🔍");

                    scrim.result_verification = "ReadyForValidation";
                    scrim.status = (int)ScrimStatus.Completed;
                    _processingQueue.Enqueue(scrim);
                    await _scrimRepository.UpdateScrim(scrim);
                    Console.WriteLine($"📌 Marcada como 'ReadyForValidation'\n");
                }
            }

            Console.WriteLine("✅ Finalizó la verificación de scrims reportadas\n");
        }


    }
}
