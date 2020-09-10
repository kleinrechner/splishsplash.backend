using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kleinrechner.SplishSplash.Backend.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Kleinrechner.SplishSplash.Backend.GpioService;
using Kleinrechner.SplishSplash.Backend.GpioService.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Kleinrechner.SplishSplash.Backend
{
    public class Startup
    {
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                // configure SwaggerDoc and others

                // add Basic Authentication
                var basicSecurityScheme = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    Reference = new OpenApiReference { Id = "BasicAuth", Type = ReferenceType.SecurityScheme }
                };
                c.AddSecurityDefinition(basicSecurityScheme.Reference.Id, basicSecurityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {basicSecurityScheme, new string[] { }}
                });
            });

            services.AddControllers();

            //Hub.Authentication.Infrastructure.Startup.ConfigureServices(services, Configuration);
            GpioService.Infrastructure.Startup.ConfigureServices(services, Configuration);
            SettingsService.Infrastructure.Startup.ConfigureServices(services, Configuration);
            EventPublisher.Infrastructure.Startup.ConfigureServices(services, Configuration);
            HubClientBackgroundService.Infrastructure.Startup.ConfigureServices(services, Configuration);
            SchedulerBackgroundService.Infrastructure.Startup.ConfigureServices(services, Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Add this line; you'll need `using Serilog;` up the top, too
            app.UseSerilogRequestLogging();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SplishSplash Backend API V1");
            });

            app.UseRouting();

            //Hub.Authentication.Infrastructure.Startup.Configure(app, env);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapGet("/", async context =>
                {
                    var pinWrapperMode = env.IsProduction() ? "Production" : "Dummy";
                    await context.Response.WriteAsync($"Welcome to SplishSplash.Backend{Environment.NewLine}" +
                                                      $"PinWrapper mode: {pinWrapperMode}{Environment.NewLine}" +
                                                      $"Assembly {this.GetType().Assembly.GetName().Name}{Environment.NewLine}" +
                                                      $"Version {this.GetType().Assembly.GetName().Version}{Environment.NewLine}" +
                                                      $".NET Core {Environment.Version}{Environment.NewLine}" +
                                                      $"Environment.OSVersion: {Environment.OSVersion}{Environment.NewLine}" +
                                                      $"Environment.Is64BitOperatingSystem: {Environment.Is64BitOperatingSystem}{Environment.NewLine}" +
                                                      $"Environment.Is64BitProcess: {Environment.Is64BitProcess}", Encoding.UTF8);
                });
            });
        }
    }
}
