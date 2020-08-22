using System;
using System.Collections.Generic;
using System.Text;
using Unosquare.RaspberryIO.Abstractions;

namespace Kleinrechner.SplishSplash.Backend.GpioService.Abstractions
{
    public class GpioPinChangedEvent
    {
        private readonly IGpioPinWrapper _gpioPinWrapper;

        public IGpioPinWrapper GpioPin => _gpioPinWrapper;

        public GpioPinChangedEvent(IGpioPinWrapper gpioPinWrapper)
        {
            _gpioPinWrapper = gpioPinWrapper;
        }
    }
}
