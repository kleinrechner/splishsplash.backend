using System;
using System.Collections.Generic;
using System.Text;

namespace SplishSplash.Backend.EventPublisher.Abstractions
{
    public interface IEventPublisher
    {
        #region Fields
        #endregion

        #region Ctor
        #endregion

        #region Methods

        /// <summary>
        /// Publish event to consumers
        /// </summary>
        /// <typeparam name="TEvent">Type of event</typeparam>
        /// <param name="event">Event object</param>
        void Publish<TEvent>(TEvent @event);

        #endregion
    }
}
