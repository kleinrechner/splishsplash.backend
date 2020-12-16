using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions.Models;

namespace Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions
{
    public interface ISplishSplashBackendHubClient
    {
        Task FrontendConntected(BaseHubModel hubModel);

        Task UpdateSettingsReceived(BackendSettingsHubModel backendSettingsHubModel);

        Task ChangeGpioPinReceived(ChangeGpioPinHubModel changeGpioPinModel);
    }
}
