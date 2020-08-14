using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Kleinrechner.SplishSplash.Backend.GpioService.Contract;

namespace Kleinrechner.SplishSplash.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GpioController : ControllerBase
    {
        private readonly ILogger<GpioController> _logger;
        private readonly IGpioService _gpioService;

        public GpioController(IGpioService gpioService, ILogger<GpioController> logger)
        {
            _gpioService = gpioService;
            _logger = logger;
        }

        [HttpGet("Get")]
        public IEnumerable<IGpioPinWrapper> Get()
        {
            var gpioPins = _gpioService.GetAllGpioPins();
            return gpioPins;
        }

        [HttpGet("Get/{gpioPinNumber}")]
        public IGpioPinWrapper Get([FromRoute] int gpioPinNumber)
        {
            var gpioPin = _gpioService.GetGpioPin(gpioPinNumber);
            return gpioPin;
        }
    }
}
