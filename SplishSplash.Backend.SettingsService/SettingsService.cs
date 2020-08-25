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

namespace Kleinrechner.SplishSplash.Backend.SettingsService
{
    public class SettingsService : ISettingsService
    {
        #region Fields

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<SettingsService> _logger;
        private SettingsServiceSettings _settings;

        #endregion

        #region Ctor

        public SettingsService(IWebHostEnvironment webHostEnvironment,
            IOptions<SettingsServiceSettings> settings,
            ILogger<SettingsService> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _settings = settings.Value;
            _logger = logger;
        }

        #endregion

        #region Methods

        public SettingsServiceSettings GetSettings()
        {
            return _settings;
        }

        public void Save(SettingsServiceSettings value)
        {
            var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "App_Data", "SettingsService.json");

            var jsonString = System.IO.File.ReadAllText(filePath);
            var settingsServiceSettingsModel = JsonConvert.DeserializeObject<SettingsServiceSettingsModel>(jsonString);
            settingsServiceSettingsModel.SettingsServiceSettings = value;

            //serialize the new updated object to a string
            var toWrite = JsonConvert.SerializeObject(settingsServiceSettingsModel, Formatting.Indented);

            //overwrite the file and it wil contain the new data
            System.IO.File.WriteAllText(filePath, toWrite);

            _settings = value;
        }

        #endregion
    }
}
