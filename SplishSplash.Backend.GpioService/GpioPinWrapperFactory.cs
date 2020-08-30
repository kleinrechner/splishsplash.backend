using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Kleinrechner.SplishSplash.Backend.GpioService.Abstractions;
using Kleinrechner.SplishSplash.Backend.GpioService.GpioPin;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SplishSplash.Backend.EventPublisher.Abstractions;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace Kleinrechner.SplishSplash.Backend.GpioService
{
    public class GpioPinWrapperFactory : IGpioPinWrapperFactory
    {
        #region Fields

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger<GpioPinWrapperFactory> _logger;
        private readonly ILogger<GpioPinWrapper> _gpioPinWrapperLogger;

        #endregion

        #region Ctor

        public GpioPinWrapperFactory(IWebHostEnvironment webHostEnvironment, IEventPublisher eventPublisher, ILogger<GpioPinWrapper> gpioPinWrapperLogger, ILogger<GpioPinWrapperFactory> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _eventPublisher = eventPublisher;
            _gpioPinWrapperLogger = gpioPinWrapperLogger;
            _logger = logger;

            if (_webHostEnvironment.IsProduction())
            {
                _logger.LogInformation("PinWrapper run in production mode");
                Pi.Init<BootstrapWiringPi>();
            }
            else
            {
                _logger.LogInformation("PinWrapper run in dummy mode");
            }
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
            if (_webHostEnvironment.IsProduction())
            {
                return new GpioPinWrapper(bcmPin, _eventPublisher, _gpioPinWrapperLogger);
            }
            else
            {
                return new DummyGpioPinWrapper(bcmPin, _eventPublisher, _gpioPinWrapperLogger);
            }
        }

#endregion

    }
}
