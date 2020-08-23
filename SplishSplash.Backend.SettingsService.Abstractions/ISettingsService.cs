using System;
using System.Collections.Generic;
using System.Text;

namespace Kleinrechner.SplishSplash.Backend.SettingsService.Abstractions
{
    public interface ISettingsService
    {
        SettingsServiceSettings GetSettings();

        void Save(SettingsServiceSettings value);
    }
}
