using System;
using System.Threading;
using System.Threading.Tasks;
using LolHubMexico.Application.dessingPatterns;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LolHubMexico.Infrastructure.BackgroundServices
{
    public class ScrimObserverHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ScrimObserverHostedService> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(5);

        public ScrimObserverHostedService(
            IServiceScopeFactory scopeFactory,
            ILogger<ScrimObserverHostedService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("✅ Scrim Observer Service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var scrimObserver = scope.ServiceProvider.GetRequiredService<IScrimObserver>();

                       await scrimObserver.VerificarScrimsPendientesAsync();
                       await scrimObserver.CancelarScrimsInactivasAsync();
                    }

                    _logger.LogInformation("✅ Scrim check completed.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Error in ScrimObserverHostedService");
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}
