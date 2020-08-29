using System;
using System.Collections.Generic;
using System.Text;
using Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions.Models;

namespace Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions
{
    public interface ISplishSplashBackendHub
    {
        void ConnectBackend(SettingsHubModel settingsHubModel);

        void SendGpioPinChanged(GpioPinChangedModel gpioPinChangedModel);
    }
}
