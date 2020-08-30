using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Kleinrechner.SplishSplash.Backend.GpioService.Abstractions;
using Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions;
using Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions.Models;
using Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Adapters;
using Kleinrechner.SplishSplash.Backend.SettingsService.Abstractions;
using Kleinrechner.SplishSplash.Backend.SettingsService.Abstractions.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SplishSplash.Backend.EventPublisher.Abstractions;

namespace Kleinrechner.SplishSplash.Backend.HubClientBackgroundService
{
    public class HubClientBackgroundService : BackgroundService, ISplishSplashBackendHubClient, IConsumer<GpioPinChangedEvent>
    {
        #region Fields
        private readonly ISettingsService _settingsService;
        private readonly IGpioService _gpioService;
        private readonly ILogger<HubClientBackgroundService> _logger;
        private readonly IOptions<HubClientBackgroundServiceSettings> _settings;

        private HubConnection _hubConnection;
        private CancellationToken _cancellationToken;

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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _cancellationToken = stoppingToken;
            _logger.LogInformation($"Starting {nameof(HubClientBackgroundService)}...");
            await ConnectToHub(stoppingToken);
        }

        private async Task ConnectToHub(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting connection to \"{_settings.Value.HubUrl}\" with User \"{_settings.Value.User}\"...");

            try
            {
                var credential = Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(_settings.Value.User + ":" + _settings.Value.Password));
                _hubConnection = new HubConnectionBuilder()
                    .WithUrl($"{_settings.Value.HubUrl}/splishsplashhub",
                        options =>
                        {
                            options.Headers.Add("Authorization", $"Basic {credential}");
                        })
                    .WithAutomaticReconnect()
                    //.AddMessagePackProtocol()
                    .Build();

                _hubConnection.On<BaseHubModel>(nameof(FrontendConntected), FrontendConntected);
                _hubConnection.On<SettingsHubModel>(nameof(UpdateSettingsReceived), UpdateSettingsReceived);
                _hubConnection.On<ChangeGpioPinModel>(nameof(ChangeGpioPinReceived), ChangeGpioPinReceived);

                await _hubConnection.StartAsync(cancellationToken);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, $"Failed to connect to Hub \"{_settings.Value.HubUrl}\": {exc.Message}");
            }
        }

        public override void Dispose()
        {
            _logger.LogInformation($"Stopping service \"{nameof(HubClientBackgroundService)}\"...");

            _hubConnection?.DisposeAsync().Wait(_cancellationToken);

            base.Dispose();
        }

        #endregion

        public void HandleEvent(GpioPinChangedEvent eventMessage)
        {
            var pinMap = _settingsService.GetSettings().PinMap
                .FirstOrDefault(x => x.Pin == eventMessage.GpioPin.GpioPinNumber);

            if (pinMap != null)
            {
                _hubConnection.InvokeAsync(nameof(ISplishSplashBackendHub.SendGpioPinChanged), new GpioPinChangedModel()
                {
                    PinMap = new PinMapModel(pinMap)
                    {
                        GpioPin = new IGpioPinWrapperToGpioPinModelAdapter(eventMessage.GpioPin)
                    }
                }, 
                _cancellationToken).Wait(_cancellationToken);
            }
        }

        public async Task FrontendConntected(BaseHubModel hubModel)
        {
            var settingsServiceSettings = _settingsService.GetSettings();
            var pinMapList = settingsServiceSettings.PinMap.Select(x => new PinMapModel(x)
            {
                GpioPin = new IGpioPinWrapperToGpioPinModelAdapter(_gpioService.GetGpioPin(x.Pin))
            }).ToList();

            var settingsHubModel = new SettingsHubModel();
            settingsHubModel.ReceiverUserName = hubModel.SenderUserName;
            settingsHubModel.PinMap = pinMapList;
            settingsHubModel.SchedulerSettings = settingsServiceSettings.SchedulerSettings;

            await _hubConnection.InvokeAsync(nameof(ISplishSplashBackendHub.ConnectBackend), 
                settingsHubModel,
                _cancellationToken);
        }

        public Task UpdateSettingsReceived(SettingsHubModel settingsHubModel)
        {
            var settingsServiceSettings = new SettingsServiceSettings();
            settingsServiceSettings.PinMap = settingsHubModel.PinMap.Select(x => new PinMap() {DisplayName = x.DisplayName, Pin = x.Pin}).ToList();
            settingsServiceSettings.SchedulerSettings = settingsHubModel.SchedulerSettings;

            _settingsService.Save(settingsServiceSettings);
            return Task.CompletedTask;
        }

        public Task ChangeGpioPinReceived(ChangeGpioPinModel changeGpioPinModel)
        {
            var gpioPin = _gpioService.GetGpioPin(changeGpioPinModel.GpioPinNumber);
            gpioPin.WriteOutput(changeGpioPinModel.Value);
            return Task.CompletedTask;
        }
    }
}
