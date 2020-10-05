using System;
using System.Collections.Generic;
using System.Text;
using Kleinrechner.SplishSplash.Backend.GpioService.Abstractions;
using Microsoft.Extensions.Logging;
using SplishSplash.Backend.EventPublisher.Abstractions;
using Unosquare.RaspberryIO.Abstractions;

namespace Kleinrechner.SplishSplash.Backend.GpioService.GpioPin
{
    public class DummyGpioPinWrapper : IGpioPinWrapper
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger<GpioPinWrapper> _logger;

        private int _gpioPinNumber;
        private int _physicalPinNumber;
        private bool _value;
        private GpioPinDriveMode _mode;

        public virtual int GpioPinNumber => _gpioPinNumber;

        public virtual int PhysicalPinNumber => _physicalPinNumber;

        public virtual bool Value => _value;

        public virtual GpioPinDriveMode Mode => _mode;

        #endregion

        #region Ctor

        public DummyGpioPinWrapper(BcmPin bcmPin, IEventPublisher eventPublisher, ILogger<GpioPinWrapper> logger)
        {
            _eventPublisher = eventPublisher;
            _logger = logger;

            _gpioPinNumber = (int) bcmPin;
            _physicalPinNumber = (int) bcmPin;
            _mode = GpioPinDriveMode.Input;
            _value = false;
        }

        #endregion

        #region Methods

        public void WriteOutput(bool value)
        {
            try
            {
                SetGpioPinOutputValue(value);
                _logger.LogInformation($"Set Pin {GpioPinNumber} to Mode {Mode} at Value {Value}");

                _eventPublisher.Publish(new GpioPinChangedEvent(this));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to set Pin {GpioPinNumber} to Mode {GpioPinDriveMode.Output} at Value {value}");
                throw;
            }
        }

        internal DummyGpioPinWrapper SetMode(GpioPinDriveMode gpioPinDriveMode)
        {
            _mode = gpioPinDriveMode;
            return this;
        }

        internal DummyGpioPinWrapper SetValue(bool value)
        {
            _value = value;
            return this;
        }

        protected virtual void SetGpioPinOutputValue(bool value)
        {
            _mode = GpioPinDriveMode.Output;
            _value = value;
        }

        #endregion

    }
}
