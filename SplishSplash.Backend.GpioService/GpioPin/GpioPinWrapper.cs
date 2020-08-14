using System;
using System.Collections.Generic;
using System.Text;
using Kleinrechner.SplishSplash.Backend.GpioService.Contract;
using Microsoft.Extensions.Logging;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;

namespace Kleinrechner.SplishSplash.Backend.GpioService.GpioPin
{
    public class GpioPinWrapper : IGpioPinWrapper
    {
        #region Fields

        private readonly ILogger<GpioPinWrapper> _logger;
        private IGpioPin _gpioPin;

        public int GpioPinNumber => _gpioPin.BcmPinNumber;

        #endregion

        #region Ctor

        public GpioPinWrapper(BcmPin bcmPin, ILogger<GpioPinWrapper> logger)
        {
            _logger = logger;
            _gpioPin = Pi.Gpio[bcmPin];
        }

        #endregion

        #region Methods
        #endregion
    }
}
