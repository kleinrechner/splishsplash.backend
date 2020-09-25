using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Kleinrechner.SplishSplash.Backend.SchedulerBackgroundService.Abstractions;
using Kleinrechner.SplishSplash.Backend.SettingsService.Abstractions;
using Microsoft.Extensions.Logging;

namespace Kleinrechner.SplishSplash.Backend.SchedulerBackgroundService
{
    public class SchedulerService : ISchedulerService
    {
        #region Fields

        private readonly ISettingsService _settingsService;
        private readonly ILogger<SchedulerService> _logger;
        private Timer _timer;

        #endregion

        #region Ctor

        public SchedulerService(ISettingsService settingsService, ILogger<SchedulerService> logger)
        {
            _settingsService = settingsService;
            _logger = logger;
        }

        #endregion

        #region Methods

        public void ExecuteScheduler(object state)
        {

        }

        #endregion
    }
}
