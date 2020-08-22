using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Kleinrechner.SplishSplash.Backend.GpioService.Abstractions;
using Kleinrechner.SplishSplash.Backend.GpioService.GpioPin;
using Microsoft.Extensions.Logging;
using Moq;
using SplishSplash.Backend.EventPublisher.Abstractions;
using Unosquare.RaspberryIO.Abstractions;
using Xunit;

namespace Kleinrechner.SplishSplash.Backend.GpioService.Test
{
    public class GpioPinWrapperTest
    {
        #region Fields
        #endregion

        #region Ctor
        #endregion

        #region Methods

        [Fact]
        public void GpioWrapper_WriteValue()
        {
            // Arrange
            var value = true;

            var eventPublisher = new Mock<IEventPublisher>();
            var logger = new Mock<ILogger<GpioPinWrapper>>();
            var gpioPinWrapper = new DummyGpioPinWrapper(BcmPin.Gpio00, eventPublisher.Object, logger.Object)
                .SetMode(GpioPinDriveMode.Input)
                .SetValue(!value);

            // Act
            gpioPinWrapper.WriteOutput(value);

            // Assert
            gpioPinWrapper.Mode.Should().Be(GpioPinDriveMode.Output);
            gpioPinWrapper.Value.Should().Be(value);

            eventPublisher.Verify(x => x.Publish(It.IsAny<GpioPinChangedEvent>()), Times.Once);
        }

        #endregion
    }
}
