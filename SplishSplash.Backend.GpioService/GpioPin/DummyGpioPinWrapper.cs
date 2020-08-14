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
        
        public int PhysicalPinNumber { get; set; }
        
        public bool Value { get; set; }
        
        public GpioPinDriveMode Mode { get; set; }

        #endregion

        #region Ctor

        public DummyGpioPinWrapper(BcmPin bcmPin, ILogger<GpioPinWrapper> logger)
        {
            _logger = logger;

            GpioPinNumber = (int) bcmPin;
        }

        #endregion

        #region Methods

        public void WriteOutput(bool value)
        {
            Mode = GpioPinDriveMode.Output;
            Value = value;
            _logger.LogInformation($"Set Pin {GpioPinNumber} to Mode {Mode} at Value {Value}");
        }

        #endregion

    }
}
