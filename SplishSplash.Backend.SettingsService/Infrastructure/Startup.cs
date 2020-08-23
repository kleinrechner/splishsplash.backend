using System;
using System.Collections.Generic;
using System.Text;
using Kleinrechner.SplishSplash.Backend.SettingsService.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kleinrechner.SplishSplash.Backend.SettingsService.Infrastructure
{
    public class Startup
    {
        #region Fields
        #endregion

        #region Ctor
        #endregion

        #region Methods

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SettingsServiceSettings>(configuration.GetSection(SettingsServiceSettings.SectionName));

            services.AddTransient<ISettingsService, SettingsService>();
        }

        #endregion
    }
}
