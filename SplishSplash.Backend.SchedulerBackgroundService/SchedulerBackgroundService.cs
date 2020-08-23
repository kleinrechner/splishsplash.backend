using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Kleinrechner.SplishSplash.Backend.SettingsService.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kleinrechner.SplishSplash.Backend.SchedulerBackgroundService
{
    public class SchedulerBackgroundService : IHostedService, IDisposable
    {
        #region Fields

        private readonly ISettingsService _settingsService;
        private readonly ILogger<SchedulerBackgroundService> _logger;
        private Timer _timer;

        #endregion

        #region Ctor

        public SchedulerBackgroundService(ISettingsService settingsService, ILogger<SchedulerBackgroundService> logger)
        {
            _settingsService = settingsService;
            _logger = logger;
        }

        #endregion

        #region Methods

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {

        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        #endregion
    }
}
