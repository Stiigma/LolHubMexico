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
        

        public ScrimObserver(IScrimRepository scrimRepository)
        {
            _scrimRepository = scrimRepository;

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

            foreach (var scrim in scrimsEnCurso)
            {
                var tiempoDesdeInicio = DateTime.Now - scrim.scheduled_date;

                if (tiempoDesdeInicio.TotalHours >= 2)
                {
                    Console.WriteLine($"❌ Scrim ID {scrim.idScrim} lleva más de 2 horas activa. Será cancelada.");
                    scrim.status = (int)ScrimStatus.Cancelled; // Estado cancelada
                    await _scrimRepository.UpdateScrim(scrim);
                }
                else
                {
                    Console.WriteLine($"⏳ Scrim ID {scrim.idScrim} aún está dentro del tiempo límite.");
                }
            }
        }

        public async Task VerificarScrimsReportadasAsync()
        {
            var scrimsEnProceso = await _scrimRepository.GetScrimsPorEstadoAsync((int)ScrimStatus.InProgress);

            foreach (var scrim in scrimsEnProceso)
            {
                // Validar si ambos equipos ya reportaron
                bool team1Respondio = scrim.team1_reported_at.HasValue && scrim.team1_result_reported.HasValue;
                bool team2Respondio = scrim.team2_reported_at.HasValue && scrim.team2_result_reported.HasValue;

                if (!team1Respondio || !team2Respondio)
                {
                    Console.WriteLine($"🕒 Scrim {scrim.idScrim} aún no tiene respuesta de ambos equipos.");
                    continue;
                }

                // Validación: si los resultados coinciden, hay disputa
                if (scrim.team1_result_reported == scrim.team2_result_reported)
                {
                    Console.WriteLine($"⚠ Scrim {scrim.idScrim} está en disputa (ambos equipos dijeron lo mismo).");

                    scrim.status = 10; // ejemplo: 10, o enum Disputed
                    scrim.result_verification = "Disputed";

                    await _scrimRepository.UpdateScrim(scrim);
                }
                else
                {
                    Console.WriteLine($"✅ Scrim {scrim.idScrim} tiene resultados opuestos. Verificada para revisión posterior.");
                    scrim.result_verification = "ReadyForValidation"; // pendiente de comparar con API o resolver
                    await _scrimRepository.UpdateScrim(scrim);
                }
            }
        }

    }
}
