using System;
using System.Collections.Generic;
using System.Text;

namespace Kleinrechner.SplishSplash.Backend.SettingsService.Abstractions
{
    public class SettingsUpdatedEvent
    {
        #region Fields

        private readonly BackendSettings _settingsServiceSettings;

        public BackendSettings SettingsServiceSettings => _settingsServiceSettings;

        #endregion

        #region Ctor

        public SettingsUpdatedEvent(BackendSettings settingsServiceSettings)
        {
            _settingsServiceSettings = settingsServiceSettings;
        }

        #endregion

        #region Methods

        #endregion
    }
}
