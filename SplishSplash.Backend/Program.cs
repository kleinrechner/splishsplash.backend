using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Kleinrechner.SplishSplash.Backend.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;

namespace Kleinrechner.SplishSplash.Backend
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting Kleinrechner.SplishSplash.Backend...");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var configPath = Path.Combine(hostingContext.HostingEnvironment.ContentRootPath, "App_Data");
                    config.SetBasePath(configPath);
                    config.AddJsonFile("appsettings.json", false, true);
                    config.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true);
                    config.AddMultipleJsonFiles(configPath);
                    config.AddEnvironmentVariables();
                })
                .UseSerilog((hostingContext, config) =>
                {
                    config.ReadFrom.Configuration(hostingContext.Configuration);
                    config.Enrich.FromLogContext();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
