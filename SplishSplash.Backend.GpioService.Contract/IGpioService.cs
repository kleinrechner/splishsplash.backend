using System;
using System.Collections.Generic;
using System.Text;

namespace Kleinrechner.SplishSplash.Backend.GpioService.Contract
{
    public interface IGpioService
    {
        IEnumerable<IGpioPinWrapper> GetAllGpioPins();
        IGpioPinWrapper GetGpioPin(int gpioPinNumber);
    }
}
