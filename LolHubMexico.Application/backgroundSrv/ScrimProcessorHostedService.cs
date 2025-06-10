using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LolHubMexico.Application.dessingPatterns;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LolHubMexico.Application.backgroundSrv
{
    public class ScrimProcessorHostedService : BackgroundService
    {
        private readonly ScrimProcessingQueue _queue;
        private readonly ILogger<ScrimProcessorHostedService> _logger;

        public ScrimProcessorHostedService(ScrimProcessingQueue queue, ILogger<ScrimProcessorHostedService> logger)
        {
            _queue = queue;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("✅ ScrimProcessor iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                if (_queue.TryDequeue(out var scrim))
                {
                    _logger.LogInformation($"⚙ Procesando scrim ID {scrim.idScrim} con fecha {scrim.scheduled_date} creada por: {scrim.created_by}");
                    // Aquí llamas a tu lógica para procesar la scrim finalizada

                    await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
                }
                else
                {
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken); // Esperar menos si está vacía
                }
            }
        }
    }

}
