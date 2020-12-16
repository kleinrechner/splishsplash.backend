using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Kleinrechner.SplishSplash.Backend.GpioService.Abstractions;
using Kleinrechner.SplishSplash.Hub.Authentication.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace Kleinrechner.SplishSplash.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = nameof(LoginUserRoles.Administrator))]
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

        [HttpGet("Clean")]
        public IEnumerable<IGpioPinWrapper> Clean()
        {
            var gpioPins = _gpioService.ClearAll();
            return gpioPins;
        }

        [HttpGet("pin/{gpioPinNumber}")]
        public IGpioPinWrapper Get([FromRoute] int gpioPinNumber)
        {
            var gpioPin = _gpioService.GetGpioPin(gpioPinNumber);
            return gpioPin;
        }

        [HttpGet("pin/{gpioPinNumber}/clean")]
        public IGpioPinWrapper Clean([FromRoute] int gpioPinNumber)
        {
            var gpioPin = _gpioService.Clear(gpioPinNumber);
            return gpioPin;
        }

        [HttpPost("pin/{gpioPinNumber}")]
        [HttpPut("pin/{gpioPinNumber}")]
        public IGpioPinWrapper Write([FromRoute] int gpioPinNumber, [FromBody] bool value)
        {
            var gpioPin = _gpioService.WriteGpioPinValue(gpioPinNumber, value);
            return gpioPin;
        }
    }
}
