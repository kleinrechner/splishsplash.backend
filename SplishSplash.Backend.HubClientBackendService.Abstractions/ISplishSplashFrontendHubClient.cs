using System;
using System.Collections.Generic;
using System.Text;
using Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions.Models;

namespace Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions
{
    public interface ISplishSplashFrontendHubClient
    {
        void BackendConnected(SettingsHubModel settingsHubModel);

        void BackendDisconnected(BaseHubModel hubModel);

        void GpioPinChangedReceived(GpioPinChangedModel gpioPinChangedModel);
    }
}
