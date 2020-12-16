using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Kleinrechner.SplishSplash.Backend.GpioService.Abstractions;
using Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions;
using Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions.Models;
using Kleinrechner.SplishSplash.Backend.SettingsService.Abstractions;
using Kleinrechner.SplishSplash.Hub.Authentication.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace Kleinrechner.SplishSplash.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = nameof(LoginUserRoles.Administrator))]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService _settingsService;
        private readonly IImportBackendSettingsService _importBackendSettingsService;
        private readonly ILogger<SettingsController> _logger;

        public SettingsController(ISettingsService settingsService, 
                                IImportBackendSettingsService importBackendSettingsService, 
                                ILogger<SettingsController> logger)
        {
            _settingsService = settingsService;
            _importBackendSettingsService = importBackendSettingsService;
            _logger = logger;
        }

        [HttpGet]
        public BackendSettings Get()
        {
            var settingsServiceSettings = _settingsService.GetSettings();
            return settingsServiceSettings;
        }

        [HttpPost]
        [HttpPut]
        public void Save([FromBody] BackendSettings value)
        {
            _settingsService.Save(value);
        }


        [HttpPost("ImportBackendSettingsHubModel")]
        [HttpPut("ImportBackendSettingsHubModel")]
        public void ImportBackendSettingsHubModel([FromBody] BackendSettingsHubModel value)
        {
            _importBackendSettingsService.ImportBackendSettingsHubModel(value);
        }
    }
}
