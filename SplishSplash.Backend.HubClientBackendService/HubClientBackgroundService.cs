using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Kleinrechner.SplishSplash.Backend.GpioService.Abstractions;
using Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions;
using Kleinrechner.SplishSplash.Backend.SettingsService.Abstractions;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SplishSplash.Backend.EventPublisher.Abstractions;

namespace Kleinrechner.SplishSplash.Backend.HubClientBackgroundService
{
    public class HubClientBackgroundService : BackgroundService, ISplishSplashHubClient, IConsumer<GpioPinChangedEvent>
    {
        #region Fields

        private readonly ISettingsService _settingsService;
        private readonly IGpioService _gpioService;
        private readonly ILogger<HubClientBackgroundService> _logger;
        private readonly IOptions<HubClientBackgroundServiceSettings> _settings;

        private HubConnection _hubConnection;

        #endregion

        #region Ctor

        public HubClientBackgroundService(IOptions<HubClientBackgroundServiceSettings> settings,
                                            ISettingsService settingsService,
                                            IGpioService gpioService, 
                                            ILogger<HubClientBackgroundService> logger)
        {
            _settings = settings;
            _gpioService = gpioService;
            _logger = logger;
            _settingsService = settingsService;
        }

        #endregion

        #region Methods

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Starting connection to {_settings.Value.HubUrl}");
            //var credential = Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(_settings.Value.User + ":" + _settings.Value.Password));
            //_hubConnection = new HubConnectionBuilder()
            //    .WithUrl($"{_settings.Value.HubUrl}/splishsplashhub",
            //        options =>
            //        {
            //            options.Headers.Add("Authorization", $"Basic {credential}");
            //        })
            //    .WithAutomaticReconnect()
            //    //.AddMessagePackProtocol()
            //    .Build();
            
            //return _hubConnection.StartAsync(stoppingToken);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _hubConnection?.DisposeAsync().Wait();

            base.Dispose();
        }

        #endregion

        public void HandleEvent(GpioPinChangedEvent eventMessage)
        {
            throw new NotImplementedException();
        }
    }
}
