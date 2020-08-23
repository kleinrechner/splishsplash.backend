using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Kleinrechner.SplishSplash.Backend.GpioService.Abstractions;
using Kleinrechner.SplishSplash.Backend.SettingsService.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace Kleinrechner.SplishSplash.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService _settingsService;
        private readonly ILogger<SettingsController> _logger;

        public SettingsController(ISettingsService settingsService, ILogger<SettingsController> logger)
        {
            _settingsService = settingsService;
            _logger = logger;
        }

        [HttpGet]
        public SettingsServiceSettings Get()
        {
            var settingsServiceSettings = _settingsService.GetSettings();
            return settingsServiceSettings;
        }

        [HttpPost]
        [HttpPut]
        public void Save([FromBody] SettingsServiceSettings value)
        {
            _settingsService.Save(value);
        }
    }
}
