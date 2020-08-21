using System;
using System.Collections.Generic;
using System.Text;

namespace Kleinrechner.SplishSplash.Backend.GpioService.Abstractions
{
    public interface IGpioPinWrapperFactory
    {
        IEnumerable<IGpioPinWrapper> GetAll();
        IGpioPinWrapper Get(in int gpioPinNumber);
    }
}
