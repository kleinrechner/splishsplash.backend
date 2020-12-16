using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kleinrechner.SplishSplash.Backend.Core.Extensions;
using Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions;
using Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions.Models;
using Kleinrechner.SplishSplash.Backend.SchedulerBackgroundService.Abstractions;
using Kleinrechner.SplishSplash.Backend.SchedulerBackgroundService.Abstractions.Models;
using Kleinrechner.SplishSplash.Backend.SettingsService.Abstractions;
using Kleinrechner.SplishSplash.Backend.SettingsService.Abstractions.Models;

namespace Kleinrechner.SplishSplash.Backend.HubClientBackgroundService
{
    public class ImportBackendSettingsService : IImportBackendSettingsService
    {
        private readonly ISettingsService _settingsService;
        private readonly ICronExpressionService _cronExpressionService;

        public ImportBackendSettingsService(ISettingsService settingsService, ICronExpressionService cronExpressionService)
        {
            _settingsService = settingsService;
            _cronExpressionService = cronExpressionService;
        }

        public void ImportBackendSettingsHubModel(BackendSettingsHubModel backendSettingsHubModel)
        {
            var backendSettings = _settingsService.GetSettings();
            backendSettings.DisplayName = backendSettingsHubModel.DisplayName;
            backendSettings.Icon = backendSettingsHubModel.Icon;
            backendSettings.OrderNumber = backendSettingsHubModel.OrderNumber;

            if (backendSettings.SchedulerSettings == null)
            {
                backendSettings.SchedulerSettings = new List<SchedulerTaskSettings>();
            }

            backendSettings.PinMap = backendSettingsHubModel.PinMap.EmptyIfNull().Select(x => new PinMap()
            {
                DisplayName = x.DisplayName,
                GpioPinNumber = x.GpioPinNumber,
                OrderNumber = x.OrderNumber,
                Icon = x.Icon
            }).ToList();

            UpdateSchedulerSettings(backendSettings.SchedulerSettings, backendSettingsHubModel.SchedulerSettings.EmptyIfNull());

            _settingsService.Save(backendSettings);
        }

        private void UpdateSchedulerSettings(List<SchedulerTaskSettings> backendSettingsSchedulerSettings, List<SchedulerTaskSettings> hubSchedulerSettings)
        {
            var comparisonResult = backendSettingsSchedulerSettings.Compare(hubSchedulerSettings, x => x.Id, x => x.Id);

            foreach (var addedSchedulerSetting in comparisonResult.Added)
            {
                if (!string.IsNullOrWhiteSpace(addedSchedulerSetting.CronExpression))
                {
                    var nextRunTime = _cronExpressionService.GetNextExecutenTime(addedSchedulerSetting.CronExpression);
                    if (nextRunTime.HasValue)
                    {
                        addedSchedulerSetting.NextRuntime = nextRunTime.Value;
                        backendSettingsSchedulerSettings.Add(addedSchedulerSetting);
                    }
                }
            }

            foreach (var updatedSchedulerSetting in comparisonResult.Updated)
            {
                var originalSchedulerSetting = updatedSchedulerSetting.Outer;
                var hubSchedulerSetting = updatedSchedulerSetting.Inner;

                if (!hubSchedulerSetting.NextRuntime.HasValue && !string.IsNullOrWhiteSpace(hubSchedulerSetting.CronExpression))
                {
                    var nextRunTime = _cronExpressionService.GetNextExecutenTime(hubSchedulerSetting.CronExpression);
                    if (nextRunTime.HasValue)
                    {
                        originalSchedulerSetting.DisplayName = hubSchedulerSetting.DisplayName;
                        originalSchedulerSetting.OrderNumber = hubSchedulerSetting.OrderNumber;
                        originalSchedulerSetting.Icon = hubSchedulerSetting.Icon;
                        originalSchedulerSetting.CronExpression = hubSchedulerSetting.CronExpression;
                        originalSchedulerSetting.NextRuntime = nextRunTime.Value;
                        originalSchedulerSetting.ChangeGpioPins = hubSchedulerSetting.ChangeGpioPins;
                    }
                }
            }

            foreach (var removedSchedulerSetting in comparisonResult.Removed)
            {
                backendSettingsSchedulerSettings.Remove(removedSchedulerSetting);
            }
        }
    }
}
