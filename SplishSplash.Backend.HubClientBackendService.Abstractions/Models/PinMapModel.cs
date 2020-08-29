using System;
using System.Collections.Generic;
using System.Text;
using Kleinrechner.SplishSplash.Backend.SettingsService.Abstractions.Models;

namespace Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions.Models
{
    public class PinMapModel : PinMap
    {
        #region Fields

        public GpioPinModel GpioPin { get; set; }

        #endregion

        #region Ctor

        public PinMapModel()
        {

        }

        public PinMapModel(PinMap pinMap)
        {
            this.Pin = pinMap.Pin;
            this.DisplayName = pinMap.DisplayName;
        }

        #endregion

        #region Methods
        #endregion
    }
}
