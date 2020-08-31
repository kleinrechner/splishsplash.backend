using System;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace Kleinrechner.SplishSplash.Backend.HubClientBackgroundService
{
    public class KeepTryingReconnect : IRetryPolicy
    {
        #region Fields

        private readonly ILogger<KeepTryingReconnect> _logger;

        #endregion

        #region Ctor

        public KeepTryingReconnect(ILogger<KeepTryingReconnect> logger)
        {
            _logger = logger;
        }

        #endregion

        #region Methods

        public TimeSpan? NextRetryDelay(RetryContext retryContext)
        {
            if (retryContext.PreviousRetryCount == 0)
            {
                _logger.LogError($"Connection lost, retry to connect to remote hub: {retryContext.RetryReason?.Message}");
            }

            if (retryContext.ElapsedTime < TimeSpan.FromMinutes(1))
            {
                return TimeSpan.FromSeconds(5);
            }
            else
            {
                return TimeSpan.FromSeconds(30);
            }
        }

        #endregion
    }
}