using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kleinrechner.SplishSplash.Backend.SchedulerBackgroundService.Infrastructure
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
            services.AddHostedService<SchedulerBackgroundService>();
        }

        #endregion
    }
}
