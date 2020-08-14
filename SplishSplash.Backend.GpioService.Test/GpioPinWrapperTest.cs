using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Kleinrechner.SplishSplash.Backend.GpioService.GpioPin;
using Microsoft.Extensions.Logging;
using Moq;
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
            var logger = new Mock<ILogger<GpioPinWrapper>>();
            var gpioPinWrapper = new DummyGpioPinWrapper(BcmPin.Gpio00, logger.Object);
            gpioPinWrapper.Value = false;
            gpioPinWrapper.Mode = GpioPinDriveMode.Input;
            var value = true;

            // Act
            gpioPinWrapper.WriteOutput(value);

            // Assert
            gpioPinWrapper.Mode.Should().Be(GpioPinDriveMode.Output);
            gpioPinWrapper.Value.Should().Be(value);
        }

        #endregion
    }
}
