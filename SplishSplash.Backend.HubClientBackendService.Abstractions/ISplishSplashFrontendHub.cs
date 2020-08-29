using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions.Models;

namespace Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions
{
    public interface ISplishSplashFrontendHub
    {
        Task SendUpdateSettings(SettingsHubModel settingsHubModel);

        Task SendChangeGpioPin(ChangeGpioPinModel changeGpioPinModel);
    }
}
