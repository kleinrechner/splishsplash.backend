using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Kleinrechner.SplishSplash.Backend.GpioService.Abstractions;
using Kleinrechner.SplishSplash.Backend.GpioService.Abstractions.Models;

namespace Kleinrechner.SplishSplash.Backend.GpioService
{
    public class ChangeGpioPinCommandService : IChangeGpioPinCommandService
    {
        private readonly IGpioService _gpioService;

        public ChangeGpioPinCommandService(IGpioService gpioService)
        {
            _gpioService = gpioService;
        }

        public Task ExecuteChangeGpioPinCommandAsync(ChangeGpioPinModel changeGpioPinModel)
        {
            var gpioPin = _gpioService.GetGpioPin(changeGpioPinModel.GpioPinNumber);
            gpioPin.WriteOutput(changeGpioPinModel.Value);

            return Task.CompletedTask;
        }
    }
}
