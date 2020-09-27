using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Kleinrechner.SplishSplash.Backend.GpioService.Abstractions;
using Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions;
using Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions.Models;
using Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Adapters;
using Kleinrechner.SplishSplash.Backend.SettingsService.Abstractions;
using Kleinrechner.SplishSplash.Backend.SettingsService.Abstractions.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SplishSplash.Backend.EventPublisher.Abstractions;

namespace Kleinrechner.SplishSplash.Backend.HubClientBackgroundService
{
    public class HubClientBackgroundService : BackgroundService
    {
        #region Fields

        private readonly IHubClientConnectionService _hubClientConnectionService;
        private readonly ILogger<HubClientBackgroundService> _logger;

        #endregion

        #region Ctor

        public HubClientBackgroundService(IHubClientConnectionService hubClientConnectionService,
                                            ILogger<HubClientBackgroundService> logger)
        {
            _hubClientConnectionService = hubClientConnectionService;
            _logger = logger;

            _logger.LogInformation($"Starting {nameof(HubClientBackgroundService)}...");
        }

        #endregion

        #region Methods

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _hubClientConnectionService.StartConnectionAsync(stoppingToken);
        }


        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _hubClientConnectionService.StopConnectionAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _logger.LogInformation($"Dispose service \"{nameof(HubClientBackgroundService)}\"...");

            _hubClientConnectionService.DisposeAsync().GetAwaiter().GetResult();

            base.Dispose();
        }

        #endregion


    }
}
