using System;
using System.Collections.Generic;
using System.Text;
using Kleinrechner.SplishSplash.Backend.SchedulerBackgroundService.Abstractions.Models;
using Kleinrechner.SplishSplash.Backend.SettingsService.Abstractions.Models;

namespace Kleinrechner.SplishSplash.Backend.SettingsService.Abstractions
{
    public class BackendSettings
    {
        public const string SectionName = "BackendSettings";

        public string DisplayName { get; set; }

        public int OrderNumber { get; set; }

        public string Icon { get; set; }

        public List<PinMap> PinMap { get; set; }

        public List<SchedulerTaskSettings> SchedulerSettings { get; set; }
    }
}
