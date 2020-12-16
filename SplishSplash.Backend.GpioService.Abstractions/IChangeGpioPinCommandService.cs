using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Kleinrechner.SplishSplash.Backend.GpioService.Abstractions.Models;

namespace Kleinrechner.SplishSplash.Backend.GpioService.Abstractions
{
    public interface IChangeGpioPinCommandService
    {
        public Task ExecuteChangeGpioPinCommandAsync(ChangeGpioPinModel changeGpioPinModel);
    }
}
