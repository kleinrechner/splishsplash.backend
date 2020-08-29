using System;
using System.Collections.Generic;
using System.Text;
using Unosquare.RaspberryIO.Abstractions;

namespace Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions.Models
{
    public class GpioPinModel
    {
        public virtual int GpioPinNumber { get; set; }

        public virtual int PhysicalPinNumber { get; set; }

        public virtual bool Value { get; set; }

        public virtual GpioPinDriveMode Mode { get; set; }

        #region Ctor
        #endregion

        #region Methods
        #endregion
    }
}
