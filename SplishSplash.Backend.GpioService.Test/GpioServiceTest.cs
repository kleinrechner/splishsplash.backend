using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Kleinrechner.SplishSplash.Backend.GpioService.Abstractions;
using Kleinrechner.SplishSplash.Backend.GpioService.GpioPin;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Unosquare.RaspberryIO.Abstractions;
using Xunit;
using Range = Moq.Range;

namespace Kleinrechner.SplishSplash.Backend.GpioService.Test
{
    public class GpioServiceTest
    {
        #region Fields
        #endregion

        #region Ctor
        #endregion

        #region Methods

        [Fact]
        public void ClearAll()
        {
            // Arrange
            var gpioPinWrapperLogger = new Mock<ILogger<GpioPinWrapper>>();

            var oldGpioPins = Enumerable.Range(0, 31)
                .Select(x => new DummyGpioPinWrapper((BcmPin) x, gpioPinWrapperLogger.Object)
                    .SetMode(GpioPinDriveMode.Input)
                    .SetValue(true))
                .ToList();

            var gpioPinWrapperFactory = new Mock<IGpioPinWrapperFactory>();
            gpioPinWrapperFactory.Setup(x => x.GetAll()).Returns(oldGpioPins);

            var gpioService = PrepareService(gpioPinWrapperFactory.Object);

            // Act
            var gpioPins = gpioService.ClearAll();

            // Assert
            gpioPins.Should().NotBeNullOrEmpty();
            gpioPins.All(x => x.Mode == GpioPinDriveMode.Output).Should().BeTrue();
            gpioPins.All(x => !x.Value).Should().BeTrue();
        }

        [Fact]
        public void ClearGpioPin()
        {
            // Arrange
            var gpioPinWrapperLogger = new Mock<ILogger<GpioPinWrapper>>();
            var bcmPinNumber = 10;
            var oldGpioPin = new DummyGpioPinWrapper((BcmPin)bcmPinNumber, gpioPinWrapperLogger.Object)
                .SetMode(GpioPinDriveMode.Input)
                .SetValue(true);

            var gpioPinWrapperFactory = new Mock<IGpioPinWrapperFactory>();
            gpioPinWrapperFactory.Setup(x => x.Get(bcmPinNumber)).Returns(oldGpioPin);

            var gpioService = PrepareService(gpioPinWrapperFactory.Object);

            // Act
            var gpioPin = gpioService.Clear(bcmPinNumber);

            // Assert
            gpioPin.Should().NotBeNull();
            gpioPin.Mode.Should().Be(GpioPinDriveMode.Output);
            gpioPin.Value.Should().BeFalse();
        }

        [Fact]
        public void GpioPin_Write_High()
        {
            // Arrange
            var gpioPinWrapperLogger = new Mock<ILogger<GpioPinWrapper>>();
            var bcmPinNumber = 10;
            var value = true;
            var oldGpioPin = new DummyGpioPinWrapper((BcmPin)bcmPinNumber, gpioPinWrapperLogger.Object)
                .SetMode(GpioPinDriveMode.Input)
                .SetValue(!value);

            var gpioPinWrapperFactory = new Mock<IGpioPinWrapperFactory>();
            gpioPinWrapperFactory.Setup(x => x.Get(bcmPinNumber)).Returns(oldGpioPin);

            var gpioService = PrepareService(gpioPinWrapperFactory.Object);

            // Act
            var gpioPin = gpioService.WriteGpioPinValue(bcmPinNumber, value);

            // Assert
            gpioPin.Should().NotBeNull();
            gpioPin.Mode.Should().Be(GpioPinDriveMode.Output);
            gpioPin.Value.Should().Be(value);
        }

        [Fact]
        public void GpioPin_Write_Low()
        {
            // Arrange
            var gpioPinWrapperLogger = new Mock<ILogger<GpioPinWrapper>>();
            var bcmPinNumber = 10;
            var value = false;
            var oldGpioPin = new DummyGpioPinWrapper((BcmPin)bcmPinNumber, gpioPinWrapperLogger.Object)
                .SetMode(GpioPinDriveMode.Input)
                .SetValue(!value);

            var gpioPinWrapperFactory = new Mock<IGpioPinWrapperFactory>();
            gpioPinWrapperFactory.Setup(x => x.Get(bcmPinNumber)).Returns(oldGpioPin);

            var gpioService = PrepareService(gpioPinWrapperFactory.Object);

            // Act
            var gpioPin = gpioService.WriteGpioPinValue(bcmPinNumber, value);

            // Assert
            gpioPin.Should().NotBeNull();
            gpioPin.Mode.Should().Be(GpioPinDriveMode.Output);
            gpioPin.Value.Should().Be(value);
        }

        private IGpioService PrepareService(IGpioPinWrapperFactory gpioPinWrapperFactory)
        {
            var logger = new Mock<ILogger<GpioService>>();

            var services = new ServiceCollection();
            services.AddTransient<ILogger<GpioService>>(provider => logger.Object);
            services.AddTransient<IGpioPinWrapperFactory>(provider => gpioPinWrapperFactory);
            services.AddTransient<IGpioService, GpioService>();

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetService<IGpioService>();
        }

        #endregion
    }
}
