using System;
using System.Collections.Generic;
using System.Text;

namespace Kleinrechner.SplishSplash.Backend.SettingsService.Abstractions
{
    public interface ISettingsService
    {
        BackendSettings GetSettings();

        void Save(BackendSettings value);
    }
}
