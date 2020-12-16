using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using Kleinrechner.SplishSplash.Backend.SettingsService.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using SplishSplash.Backend.EventPublisher.Abstractions;

namespace Kleinrechner.SplishSplash.Backend.SettingsService
{
    public class SettingsService : ISettingsService
    {
        #region Fields

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger<SettingsService> _logger;
        private BackendSettings _settings;

        #endregion

        #region Ctor

        public SettingsService(IWebHostEnvironment webHostEnvironment, 
            IEventPublisher eventPublisher,
            IOptions<BackendSettings> settings,
            ILogger<SettingsService> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _eventPublisher = eventPublisher;
            _settings = settings.Value;
            _logger = logger;
        }

        #endregion

        #region Methods

        public BackendSettings GetSettings()
        {
            return _settings;
        }

        public void Save(BackendSettings value)
        {
            var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "App_Data", "SettingsService.json");

            BackendSettingsModel backendSettingsModel = null;
            lock (this)
            {
                var jsonString = System.IO.File.ReadAllText(filePath);
                backendSettingsModel = JsonConvert.DeserializeObject<BackendSettingsModel>(jsonString);
                backendSettingsModel.BackendSettings = value;

                //serialize the new updated object to a string
                var toWrite = JsonConvert.SerializeObject(backendSettingsModel, Formatting.Indented);

                //overwrite the file and it wil contain the new data
                System.IO.File.WriteAllText(filePath, toWrite);
            }

            _eventPublisher.Publish(new SettingsUpdatedEvent(backendSettingsModel.BackendSettings));

            _settings = value;
        }

        #endregion
    }
}
