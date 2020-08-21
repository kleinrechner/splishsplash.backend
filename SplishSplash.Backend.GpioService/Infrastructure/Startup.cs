using System;
using System.Collections.Generic;
using System.Text;
using Kleinrechner.SplishSplash.Backend.GpioService.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kleinrechner.SplishSplash.Backend.GpioService.Infrastructure
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
            services.AddTransient<IGpioService, GpioService>();
            services.AddTransient<IGpioPinWrapperFactory, GpioPinWrapperFactory>();
        }

        #endregion
    }
}
