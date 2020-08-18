using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Infrastructure
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
            services.Configure<HubClientBackgroundServiceSettings>(configuration.GetSection(HubClientBackgroundServiceSettings.Position));

            services.AddHostedService<HubClientBackgroundService>();
        }

        #endregion
    }
}
