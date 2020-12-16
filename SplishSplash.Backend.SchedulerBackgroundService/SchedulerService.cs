using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Kleinrechner.SplishSplash.Backend.Core.Extensions;
using Kleinrechner.SplishSplash.Backend.GpioService.Abstractions;
using Kleinrechner.SplishSplash.Backend.SchedulerBackgroundService.Abstractions;
using Kleinrechner.SplishSplash.Backend.SchedulerBackgroundService.Abstractions.Models;
using Kleinrechner.SplishSplash.Backend.SettingsService.Abstractions;
using Microsoft.Extensions.Logging;

namespace Kleinrechner.SplishSplash.Backend.SchedulerBackgroundService
{
    public class SchedulerService : ISchedulerService
    {
        #region Fields

        private readonly IChangeGpioPinCommandService _changeGpioPinCommandService;
        private readonly ICronExpressionService _cronExpressionService;
        private readonly ISettingsService _settingsService;
        private readonly ILogger<SchedulerService> _logger;

        private readonly ConcurrentQueue<SchedulerTaskSettings> _schedulerTaskSettingsQueue = new ConcurrentQueue<SchedulerTaskSettings>();
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private Task _executingTask;

        #endregion

        #region Ctor

        public SchedulerService(IChangeGpioPinCommandService changeGpioPinCommandService, 
            ICronExpressionService cronExpressionService, 
            ISettingsService settingsService, 
            ILogger<SchedulerService> logger)
        {
            _settingsService = settingsService;
            _logger = logger;
            _cronExpressionService = cronExpressionService;
            _changeGpioPinCommandService = changeGpioPinCommandService;
        }

        #endregion

        #region Methods

        public void ExecuteScheduler(object state)
        {
            var now = DateTime.Now;
            var executionTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
            
            foreach (var schedulerSetting in _settingsService.GetSettings()
                .SchedulerSettings
                .EmptyIfNull()
                .Where(x => x.NextRuntime.HasValue &&
                            executionTime >= x.NextRuntime.Value))
            {
                if (!_cancellationTokenSource.IsCancellationRequested)
                {
                    schedulerSetting.NextRuntime =
                        _cronExpressionService.GetNextExecutenTime(schedulerSetting.CronExpression);

                    _schedulerTaskSettingsQueue.Enqueue(schedulerSetting);

                    if (_executingTask == null && _schedulerTaskSettingsQueue.Any())
                    {
                        _executingTask = Task.Run(ExecuteSchedulerTasks, _cancellationTokenSource.Token);
                        _executingTask
                            .GetAwaiter()
                            .OnCompleted(() =>
                            {
                                _executingTask = null;
                                _settingsService.Save(_settingsService.GetSettings());
                            });
                    }
                }
            }
        }

        private async Task ExecuteSchedulerTasks()
        {
            foreach (var schedulerTask in _schedulerTaskSettingsQueue)
            {
                if (!_cancellationTokenSource.IsCancellationRequested)
                {
                    try
                    {
                        foreach (var changeGpioPinCommand in schedulerTask.ChangeGpioPins)
                        {
                            if (!_cancellationTokenSource.IsCancellationRequested)
                            {
                                await _changeGpioPinCommandService.ExecuteChangeGpioPinCommandAsync(
                                    changeGpioPinCommand);
                            }
                        }

                        schedulerTask.LastRunTimeSucceeded = DateTime.Now;
                    }
                    catch (Exception exc)
                    {
                        _logger.LogError(exc, $"Execute SchedulerTask failed: {exc.Message}");
                        schedulerTask.LastRunTimeFailed = DateTime.Now;
                    }
                }
            }
        }

        public async Task Stop()
        {
            _cancellationTokenSource.Cancel();

            // Wait until the task completes or the stop token triggers
            await Task.WhenAny(_executingTask, Task.Delay(-1, _cancellationTokenSource.Token));
        }

        #endregion
    }
}
