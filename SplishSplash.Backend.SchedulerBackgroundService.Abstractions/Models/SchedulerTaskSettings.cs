using System;
using System.Collections.Generic;
using System.Text;
using Kleinrechner.SplishSplash.Backend.GpioService.Abstractions.Models;

namespace Kleinrechner.SplishSplash.Backend.SchedulerBackgroundService.Abstractions.Models
{
    public class SchedulerTaskSettings
    {
        #region Fields

        public Guid Id { get; set; }
        
        public string DisplayName { get; set; }

        public int OrderNumber { get; set; }

        public string Icon { get; set; }

        public string CronExpression { get; set; }

        public DateTime? NextRuntime { get; set; }

        public DateTime? LastRunTimeSucceeded { get; set; }

        public DateTime? LastRunTimeFailed { get; set; }

        public List<ChangeGpioPinModel> ChangeGpioPins { get; set; }

        #endregion

        #region Ctor
        #endregion

        #region Methods
        #endregion
    }
}
