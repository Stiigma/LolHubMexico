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
            var scrimsPorVerificar = await _scrimRepository.GetScrimsPorEstadoAsync(2);

            foreach (var scrim in scrimsPorVerificar)
            {
                // 1. ¿Ya pasó la hora programada?
                if (scrim.scheduled_date <= DateTime.Now)
                {
                    Console.WriteLine($"⏰ Scrim ID {scrim.idScrim} ya comenzó (fecha programada: {scrim.scheduled_date})");

                    scrim.status = (int)ScrimStatus.InProgress; ; // Estado 3 = partida iniciada
                    await _scrimRepository.UpdateScrim(scrim);
                    continue;
                }

                // 2. (Opcional) ¿Tiene idMatch y está lista para procesarse?
                if (!string.IsNullOrEmpty(scrim.result))
                {
                    Console.WriteLine($"✅ Scrim ID {scrim.idScrim} ya fue procesada.");
                    continue;
                }

                Console.WriteLine($"🟡 Scrim ID {scrim.idScrim} aún está programada (fecha futura: {scrim.scheduled_date})");
            }
        }

        public async Task CancelarScrimsInactivasAsync()
        {
            var scrimsEnCurso = await _scrimRepository.GetScrimsPorEstadoAsync(3);

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
    }
}
