using System;
using System.Collections.Generic;
using System.Text;
using Kleinrechner.SplishSplash.Backend.SchedulerBackgroundService.Abstractions;
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
            services.AddSingleton<ICronExpressionService, CronExpressionService>();
            services.AddSingleton<ISchedulerService, SchedulerService>();
            services.AddHostedService<SchedulerBackgroundService>();
        }

        #endregion
    }
}
