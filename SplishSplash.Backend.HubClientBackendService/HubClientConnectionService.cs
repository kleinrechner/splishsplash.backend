using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Kleinrechner.SplishSplash.Backend.GpioService.Abstractions;
using Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions;
using Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions.Models;
using Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Adapters;
using Kleinrechner.SplishSplash.Backend.SettingsService.Abstractions;
using Kleinrechner.SplishSplash.Backend.SettingsService.Abstractions.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SplishSplash.Backend.EventPublisher.Abstractions;

namespace Kleinrechner.SplishSplash.Backend.HubClientBackgroundService
{
    public class HubClientConnectionService : IHubClientConnectionService, ISplishSplashBackendHubClient, IConsumer<GpioPinChangedEvent>, IConsumer<SettingsUpdatedEvent>
    {
        #region Fields

        private readonly ISettingsService _settingsService;
        private readonly IGpioService _gpioService;
        private readonly IRetryPolicy _retryPolicy;
        private readonly IOptions<HubClientBackgroundServiceSettings> _settings;
        private readonly ILogger<HubClientConnectionService> _logger;

        private readonly HubConnection _hubConnection;
        private CancellationToken _cancellationToken;

        #endregion

        #region Ctor

        public HubClientConnectionService(ISettingsService settingsService, 
                                            IGpioService gpioService, 
                                            IRetryPolicy retryPolicy, 
                                            IOptions<HubClientBackgroundServiceSettings> settings, 
                                            ILogger<HubClientConnectionService> logger)
        {
            _settingsService = settingsService;
            _gpioService = gpioService;
            _retryPolicy = retryPolicy;
            _settings = settings;
            _logger = logger;

            _hubConnection = CreateConnection();
        }

        #endregion

        #region Methods

        public async Task StartConnectionAsync(CancellationToken stoppingToken)
        {
            _cancellationToken = stoppingToken;
            var connected = await ConnectToHub(stoppingToken);
            if (connected)
            {
                _logger.LogInformation("Connection to hub established!");
            }
            else
            {
                _logger.LogWarning("Connecting to hub canceled!");
            }
        }

        public async Task StopConnectionAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Stop connection to \"{_settings.Value.HubUrl}\"...");
                await _hubConnection.StopAsync(cancellationToken);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, $"Failed to stop connection: {exc.Message}");
                throw;
            }
        }

        public void HandleEvent(GpioPinChangedEvent eventMessage)
        {
            try
            {
                var pinMap = _settingsService.GetSettings().PinMap?
                    .FirstOrDefault(x => x.GpioPinNumber == eventMessage.GpioPin.GpioPinNumber);

                if (pinMap != null)
                {
                    _hubConnection.InvokeAsync(nameof(ISplishSplashBackendHub.SendGpioPinChanged),
                        new GpioPinChangedModel()
                        {
                            PinMap = new PinMapModel(pinMap)
                            {
                                GpioPin = new IGpioPinWrapperToGpioPinModelAdapter(eventMessage.GpioPin)
                            }
                        },
                        _cancellationToken).Wait(_cancellationToken);
                }
            }
            catch (Exception exc)
            {
                _logger.LogError($"Handle GpioPinChangedEvent failed: {exc.Message}", exc);
            }
        }

        public void HandleEvent(SettingsUpdatedEvent eventMessage)
        {
            try
            {
                var settingsHubModel = GetBackendSettingsHubModel(eventMessage.SettingsServiceSettings);

                    _hubConnection.InvokeAsync(nameof(ISplishSplashBackendHub.SettingsUpdated),
                        settingsHubModel,
                        _cancellationToken).Wait(_cancellationToken);
            }
            catch (Exception exc)
            {
                _logger.LogError($"Handle SettingsUpdatedEvent failed: {exc.Message}", exc);
            }
        }

        public async Task FrontendConntected(BaseHubModel hubModel)
        {
            try
            {
                var backendSettings = _settingsService.GetSettings();
                var backendSettingsHubModel = GetBackendSettingsHubModel(backendSettings);
                backendSettingsHubModel.ReceiverUserName = hubModel.SenderUserName;

                await _hubConnection.InvokeAsync(nameof(ISplishSplashBackendHub.ConnectBackend),
                    backendSettingsHubModel,
                    _cancellationToken);
            }
            catch (Exception exc)
            {
                await HandleCommandFailed(nameof(FrontendConntected), hubModel, exc);
            }
        }

        public async Task UpdateSettingsReceived(BackendSettingsHubModel backendSettingsHubModel)
        {
            try
            {
                var backendSettings = new BackendSettings();
                backendSettings.DisplayName = backendSettingsHubModel.DisplayName;
                backendSettings.Icon = backendSettingsHubModel.Icon;
                backendSettings.OrderNumber = backendSettingsHubModel.OrderNumber;
                backendSettings.SchedulerSettings = backendSettingsHubModel.SchedulerSettings;

                backendSettings.PinMap = backendSettingsHubModel.PinMap.Select(x => new PinMap()
                {
                    DisplayName = x.DisplayName,
                    GpioPinNumber = x.GpioPinNumber,
                    OrderNumber = x.OrderNumber,
                    Icon = x.Icon
                }).ToList();

                _settingsService.Save(backendSettings);
            }
            catch (Exception exc)
            {
                await HandleCommandFailed(nameof(UpdateSettingsReceived), backendSettingsHubModel, exc);
            }
        }

        public async Task ChangeGpioPinReceived(ChangeGpioPinModel changeGpioPinModel)
        {
            try
            {
                var gpioPin = _gpioService.GetGpioPin(changeGpioPinModel.GpioPinNumber);
                gpioPin.WriteOutput(changeGpioPinModel.Value);
            }
            catch (Exception exc)
            {
                await HandleCommandFailed(nameof(ChangeGpioPinReceived), changeGpioPinModel, exc);
            }
        }

        private HubConnection CreateConnection()
        {
            try
            {

                var hubUri = new Uri(_settings.Value.HubUrl);
                _logger.LogInformation($"Starting create connection to \"{hubUri}\" with User \"{_settings.Value.User}\"...");

                var credential = Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1")
                    .GetBytes(_settings.Value.User + ":" + _settings.Value.Password));

                var hubConnection = new HubConnectionBuilder()
                    .WithUrl(new Uri(hubUri, "splishsplashhub"),
                        options => { options.Headers.Add("Authorization", $"Basic {credential}"); })
                    .WithAutomaticReconnect(_retryPolicy)
                    //.AddMessagePackProtocol()
                    .Build();

                hubConnection.On<BaseHubModel>(nameof(FrontendConntected), FrontendConntected);
                hubConnection.On<BackendSettingsHubModel>(nameof(UpdateSettingsReceived), UpdateSettingsReceived);
                hubConnection.On<ChangeGpioPinModel>(nameof(ChangeGpioPinReceived), ChangeGpioPinReceived);
                return hubConnection;
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, $"Failed to create connection to Hub \"{_settings.Value.HubUrl}\": {exc.Message}");
                throw;
            }
        }

        private async Task<bool> ConnectToHub(CancellationToken cancellationToken)
        {
            // Keep trying to until we can start or the token is canceled.
            while (true)
            {
                try
                {
                    _logger.LogInformation($"Starting connection to \"{_settings.Value.HubUrl}\"...");
                    await _hubConnection.StartAsync(cancellationToken);
                    Debug.Assert(_hubConnection.State == HubConnectionState.Connected);
                    return true;
                }
                catch when (cancellationToken.IsCancellationRequested)
                {
                    return false;
                }
                catch (Exception exc)
                {
                    _logger.LogError(exc, $"Failed to connect to Hub \"{_settings.Value.HubUrl}\": {exc.Message}");

                    // Failed to connect, trying again in 5000 ms.
                    Debug.Assert(_hubConnection.State == HubConnectionState.Disconnected);
                    await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
                }
            }
        }

        private BackendSettingsHubModel GetBackendSettingsHubModel(BackendSettings settingsServiceSettings)
        {
            var pinMapList = settingsServiceSettings.PinMap?.Select(x => new PinMapModel(x)
            {
                GpioPin = new IGpioPinWrapperToGpioPinModelAdapter(_gpioService.GetGpioPin(x.GpioPinNumber))
            }).ToList();

            var backendSettingsHubModel = new BackendSettingsHubModel();
            backendSettingsHubModel.PinMap = pinMapList;
            backendSettingsHubModel.SchedulerSettings = settingsServiceSettings.SchedulerSettings;

            return backendSettingsHubModel;
        }

        private async Task HandleCommandFailed(string methodFailed, BaseHubModel hubModel, Exception exception)
        {
            try
            {
                _logger.LogError($"{nameof(HubClientConnectionService)}.{methodFailed} failed: {exception.Message}", exception);

                if (!string.IsNullOrWhiteSpace(hubModel.SenderUserName))
                {
                    var backendCommandFailedModel = new BackendCommandFailedModel();
                    backendCommandFailedModel.SenderUserName = hubModel.SenderUserName;
                    backendCommandFailedModel.MethodFailed = $"{nameof(HubClientConnectionService)}.{methodFailed}";
                    backendCommandFailedModel.ErrorMessage = exception.Message;

                    await _hubConnection.InvokeAsync(nameof(ISplishSplashBackendHub.BackendCommandFailed),
                        backendCommandFailedModel,
                        _cancellationToken);
                }
            }
            catch (Exception exc)
            {
                _logger.LogError($"Send CommandError failed: {exc.Message}/ Original exception: {exception.Message}", exc);
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.DisposeAsync();
            }
        }

        #endregion
    }
}
