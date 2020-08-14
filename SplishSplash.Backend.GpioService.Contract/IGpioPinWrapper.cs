using System;
using System.Collections.Generic;
using System.Text;

namespace Kleinrechner.SplishSplash.Backend.GpioService.Contract
{
    public interface IGpioPinWrapper
    {
        public int GpioPinNumber { get; }
    }
}
