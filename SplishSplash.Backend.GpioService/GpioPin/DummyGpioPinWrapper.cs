using System;
using System.Collections.Generic;
using System.Text;
using Kleinrechner.SplishSplash.Backend.GpioService.Contract;
using Microsoft.Extensions.Logging;
using Unosquare.RaspberryIO.Abstractions;

namespace Kleinrechner.SplishSplash.Backend.GpioService.GpioPin
{
    public class DummyGpioPinWrapper : IGpioPinWrapper
    {
        #region Fields

        private readonly ILogger<GpioPinWrapper> _logger;

        public int GpioPinNumber { get; set; }

        #endregion

        #region Ctor

        public DummyGpioPinWrapper(BcmPin bcmPin, ILogger<GpioPinWrapper> logger)
        {
            _logger = logger;

            GpioPinNumber = (int) bcmPin;
        }

        #endregion

        #region Methods
        #endregion

    }
}
