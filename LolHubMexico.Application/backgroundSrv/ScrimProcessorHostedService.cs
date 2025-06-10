using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using LolHubMexico.Domain.Entities.Scrims;
using Microsoft.Extensions.DependencyInjection;
using LolHubMexico.Application.Interfaces;
using LolHubMexico.Application.dessingPatterns;
using LolHubMexico.Application.ScrimProcessing;

namespace LolHubMexico.Application.backgroundSrv
{
    public class ScrimProcessorHostedService : BackgroundService
    {
        private readonly ScrimProcessingQueue _queue;
        private readonly ILogger<ScrimProcessorHostedService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public ScrimProcessorHostedService(
            ScrimProcessingQueue queue,
            ILogger<ScrimProcessorHostedService> logger,
            IServiceScopeFactory scopeFactory)
        {
            _queue = queue;
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("✅ ScrimProcessor iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                if (_queue.TryDequeue(out var scrim))
                {
                    _logger.LogInformation($"⚙ Procesando scrim ID {scrim.idScrim} con fecha {scrim.scheduled_date} creada por: {scrim.created_by}");

                    using var scope = _scopeFactory.CreateScope();
                    var processor = scope.ServiceProvider.GetRequiredService<IScrimProcessor>();

                    try
                    {
                        await processor.ProcessAsync(scrim, scrim.idMatch1);
                        _logger.LogInformation($"✅ Scrim ID {scrim.idScrim} procesada con éxito.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"❌ Error al procesar scrim ID {scrim.idScrim}");
                    }

                    await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
                }
                else
                {
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken); // Menor espera si no hay tareas
                }
            }
        }
    }
}
