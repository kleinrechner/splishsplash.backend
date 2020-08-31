using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kleinrechner.SplishSplash.Backend.GpioService.Abstractions;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SplishSplash.Backend.EventPublisher.Abstractions;

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
            services.Configure<HubClientBackgroundServiceSettings>(configuration.GetSection(HubClientBackgroundServiceSettings.SectionName));

            services.AddSingleton<HubClientBackgroundService>();
            services.AddTransient<IConsumer<GpioPinChangedEvent>>(x =>
                x.GetRequiredService<HubClientBackgroundService>());

            services.AddSingleton<IRetryPolicy, KeepTryingReconnect>();
            services.AddHostedService<HubClientBackgroundService>(x => x.GetRequiredService<HubClientBackgroundService>());
        }

        #endregion
    }
}
