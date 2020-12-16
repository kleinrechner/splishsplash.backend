using System;
using System.Collections.Generic;
using System.Text;
using Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions.Models;

namespace Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions
{
    public interface IImportBackendSettingsService
    {
        void ImportBackendSettingsHubModel(BackendSettingsHubModel backendSettingsHubModel);
    }
}
