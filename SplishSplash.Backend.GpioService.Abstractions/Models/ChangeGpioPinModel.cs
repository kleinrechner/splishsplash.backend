using System;
using System.Collections.Generic;
using System.Text;
using Unosquare.RaspberryIO.Abstractions;

namespace Kleinrechner.SplishSplash.Backend.GpioService.Abstractions.Models
{
    public class ChangeGpioPinModel
    {
        #region Fields

        public virtual int GpioPinNumber { get; set; }

        public virtual bool Value { get; set; }

        public virtual GpioPinDriveMode Mode { get; set; }

        #endregion

        #region Ctor
        #endregion

        #region Methods
        #endregion
    }
}
