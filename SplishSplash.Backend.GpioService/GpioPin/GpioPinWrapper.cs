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
        private readonly IGpioPin _gpioPin;

        public int GpioPinNumber => _gpioPin.BcmPinNumber;

        public int PhysicalPinNumber => _gpioPin.PhysicalPinNumber;

        public bool Value => _gpioPin.Value;

        public GpioPinDriveMode Mode => _gpioPin.PinMode;

        #endregion

        #region Ctor

        public GpioPinWrapper(BcmPin bcmPin, ILogger<GpioPinWrapper> logger)
        {
            _logger = logger;
            _gpioPin = Pi.Gpio[bcmPin];
        }

        #endregion

        #region Methods

        public void WriteOutput(bool value)
        {
            try
            {
                _gpioPin.PinMode = GpioPinDriveMode.Output;
                _gpioPin.Write(value);
                _logger.LogInformation($"Set Pin {GpioPinNumber} to Mode {Mode} at Value {Value}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to set Pin {GpioPinNumber} to Mode {GpioPinDriveMode.Output} at Value {value}");
                throw;
            }
        }

        #endregion
    }
}
