using System;
using System.Collections.Generic;
using System.Text;
using Kleinrechner.SplishSplash.Backend.GpioService.Abstractions;
using Microsoft.Extensions.Logging;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;

namespace Kleinrechner.SplishSplash.Backend.GpioService.GpioPin
{
    public class GpioPinWrapper : DummyGpioPinWrapper
    {
        #region Fields

        private readonly IGpioPin _gpioPin;

        public override int GpioPinNumber => _gpioPin.BcmPinNumber;

        public override int PhysicalPinNumber => _gpioPin.PhysicalPinNumber;

        public override bool Value => _gpioPin.Value;

        public override GpioPinDriveMode Mode => _gpioPin.PinMode;

        #endregion

        #region Ctor

        public GpioPinWrapper(BcmPin bcmPin, ILogger<GpioPinWrapper> logger) : base(bcmPin, logger)
        {
            _gpioPin = Pi.Gpio[bcmPin];
        }

        #endregion

        #region Methods

        protected override void SetGpioPinOutputValue(bool value)
        {
            _gpioPin.PinMode = GpioPinDriveMode.Output;
            _gpioPin.Write(value);
        }

        #endregion
    }
}
