using System;
using System.Collections.Generic;
using System.Text;

namespace Kleinrechner.SplishSplash.Backend.GpioService.Contract
{
    public interface IGpioService
    {
        IEnumerable<IGpioPinWrapper> GetAllGpioPins();

        IEnumerable<IGpioPinWrapper> ClearAll();

        IGpioPinWrapper GetGpioPin(int gpioPinNumber);

        IGpioPinWrapper WriteGpioPinValue(int gpioPinNumber, bool value);

        IGpioPinWrapper Clear(int gpioPinNumber);
    }
}
