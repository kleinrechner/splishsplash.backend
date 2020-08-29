using System;
using System.Collections.Generic;
using System.Text;
using Kleinrechner.SplishSplash.Backend.GpioService.Abstractions;
using Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions.Models;
using Unosquare.RaspberryIO.Abstractions;

namespace Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Adapters
{
    public class IGpioPinWrapperToGpioPinModelAdapter : GpioPinModel
    {
        #region Fields

        private readonly IGpioPinWrapper _gpioPinWrapper;
        private int _gpioPinNumber;
        private int _physicalPinNumber;
        private bool _value;
        private GpioPinDriveMode _mode;

        public override int GpioPinNumber
        {
            get => _gpioPinWrapper.GpioPinNumber;
            set => _gpioPinNumber = value;
        }

        public override int PhysicalPinNumber
        {
            get => _gpioPinWrapper.PhysicalPinNumber;
            set => _physicalPinNumber = value;
        }

        public override bool Value
        {
            get => _gpioPinWrapper.Value;
            set => _value = value;
        }

        public override GpioPinDriveMode Mode
        {
            get => _gpioPinWrapper.Mode;
            set => _mode = value;
        }

        #endregion

        #region Ctor

        public IGpioPinWrapperToGpioPinModelAdapter(IGpioPinWrapper gpioPinWrapper)
        {
            _gpioPinWrapper = gpioPinWrapper;
        }

        #endregion

        #region Methods

        #endregion
    }
}
