using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions.Models;

namespace Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions
{
    public interface ISplishSplashBackendHub
    {
        Task ConnectBackend(BackendSettingsHubModel backendSettingsHubModel);

        Task SendGpioPinChanged(GpioPinChangedModel gpioPinChangedModel);

        Task SettingsUpdated(BackendSettingsHubModel backendSettingsHubModel);

        Task BackendCommandFailed(BackendCommandFailedModel backendCommandFailedModel);
    }
}
