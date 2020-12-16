using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Kleinrechner.SplishSplash.Backend.SchedulerBackgroundService.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kleinrechner.SplishSplash.Backend.SchedulerBackgroundService
{
    public class SchedulerBackgroundService : IHostedService, IDisposable
    {
        #region Fields

        private readonly ISchedulerService _schedulerService;
        private readonly ILogger<SchedulerBackgroundService> _logger;
        private Timer _timer;

        #endregion

        #region Ctor

        public SchedulerBackgroundService(ISchedulerService schedulerService, ILogger<SchedulerBackgroundService> logger)
        {
            _schedulerService = schedulerService;
            _logger = logger;
        }

        #endregion

        #region Methods

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Starting {nameof(SchedulerBackgroundService)}...");

            _timer = new Timer(_schedulerService.ExecuteScheduler, 
                                null, 
                                TimeSpan.Zero,
                                TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Stopping service {nameof(SchedulerBackgroundService)}...");

            _timer?.Change(Timeout.Infinite, 0);

            await _schedulerService.Stop();
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        #endregion
    }
}
