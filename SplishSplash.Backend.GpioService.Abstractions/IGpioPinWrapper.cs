using System;
using System.Collections.Generic;
using System.Text;
using Unosquare.RaspberryIO.Abstractions;

namespace Kleinrechner.SplishSplash.Backend.GpioService.Abstractions
{
    public interface IGpioPinWrapper
    {
        int GpioPinNumber { get; }

        int PhysicalPinNumber { get; }

        bool Value { get; }

        GpioPinDriveMode Mode { get; }

        void WriteOutput(bool value);
    }
}
