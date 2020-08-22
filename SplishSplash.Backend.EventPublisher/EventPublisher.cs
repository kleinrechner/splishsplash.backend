using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SplishSplash.Backend.EventPublisher.Abstractions;

namespace Kleinrechner.SplishSplash.Backend.EventPublisher
{
    public class EventPublisher : IEventPublisher
    {
        #region Fields

        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EventPublisher> _logger;

        #endregion

        #region Ctor

        public EventPublisher(IServiceProvider serviceProvider, ILogger<EventPublisher> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        #endregion

        #region Methods

        public void Publish<TEvent>(TEvent @event)
        {
            var consumers = _serviceProvider.GetServices<IConsumer<TEvent>>();
            foreach (var consumer in consumers)
            {
                try
                {
                    //try to handle published event
                    consumer.HandleEvent(@event);
                }
                catch (Exception exception)
                {
                    //log error, we put in to nested try-catch to prevent possible cyclic (if some error occurs)
                    try
                    {
                        _logger.LogError(exception, exception.Message);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }

        #endregion
    }
}
