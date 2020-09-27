using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions.Models;

namespace Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions
{
    public interface ISplishSplashFrontendHubClient
    {
        Task BackendConnected(SettingsHubModel settingsHubModel);

        Task BackendDisconnected(BaseHubModel hubModel);

        Task GpioPinChangedReceived(GpioPinChangedModel gpioPinChangedModel);

        Task BackendCommandFailedReceived(BackendCommandFailedModel backendCommandFailedModel);
    }
}
