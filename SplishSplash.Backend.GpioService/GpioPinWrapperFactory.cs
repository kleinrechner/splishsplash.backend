﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Kleinrechner.SplishSplash.Backend.GpioService.Contract;
using Kleinrechner.SplishSplash.Backend.GpioService.GpioPin;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace Kleinrechner.SplishSplash.Backend.GpioService
{
    public class GpioPinWrapperFactory : IGpioPinWrapperFactory
    {
        #region Fields

        private readonly ILogger<GpioPinWrapperFactory> _logger;
        private readonly ILogger<GpioPinWrapper> _gpioPinWrapperLogger;

        #endregion

        #region Ctor

        public GpioPinWrapperFactory(ILogger<GpioPinWrapper> gpioPinWrapperLogger, ILogger<GpioPinWrapperFactory> logger)
        {
            _gpioPinWrapperLogger = gpioPinWrapperLogger;
            _logger = logger;

#if RELEASE
            Pi.Init<BootstrapWiringPi>();
#endif
        }

        #endregion

        #region Methods

        public IEnumerable<IGpioPinWrapper> GetAll()
        {
            return Enum.GetValues(typeof(BcmPin)).Cast<BcmPin>().Select(GetGpioPinWrapper);
        }

        public IGpioPinWrapper Get(in int gpioPinNumber)
        {
            var values = Enum.GetValues(typeof(BcmPin)).Cast<int>();
            if (gpioPinNumber >= values.Min() && gpioPinNumber <= values.Max())
            {
                return GetGpioPinWrapper((BcmPin)gpioPinNumber);
            }
            else
            {
                throw new InvalidGpioPinNumberException();
            }
        }

        private IGpioPinWrapper GetGpioPinWrapper(BcmPin bcmPin)
        {
#if RELEASE
            return new GpioPinWrapper(bcmPin, _gpioPinWrapperLogger);
#else
            return new DummyGpioPinWrapper(bcmPin, _gpioPinWrapperLogger);
#endif
        }

#endregion

    }
}
