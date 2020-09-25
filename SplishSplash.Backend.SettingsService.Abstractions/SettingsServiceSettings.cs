using System;
using System.Collections.Generic;
using System.Text;
using Kleinrechner.SplishSplash.Backend.SchedulerBackgroundService.Abstractions.Models;
using Kleinrechner.SplishSplash.Backend.SettingsService.Abstractions.Models;

namespace Kleinrechner.SplishSplash.Backend.SettingsService.Abstractions
{
    public class SettingsServiceSettings
    {
        public const string SectionName = "SettingsServiceSettings";

        public List<PinMap> PinMap { get; set; }

        public SchedulerBackgroundServiceSettings SchedulerSettings { get; set; }
    }
}
