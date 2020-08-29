using System;
using System.Collections.Generic;
using System.Text;
using Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions.Models;

namespace Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions
{
    public interface ISplishSplashFrontendHub
    {
        void SendUpdateSettings(SettingsHubModel settingsHubModel);

        void SendChangeGpioPin(ChangeGpioPinModel changeGpioPinModel);
    }
}
