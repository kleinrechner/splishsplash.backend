using System;
using System.Linq;
using Castle.Core.Logging;
using FluentAssertions;
using Kleinrechner.SplishSplash.Backend.GpioService.Abstractions;
using Kleinrechner.SplishSplash.Backend.GpioService.GpioPin;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using SplishSplash.Backend.EventPublisher.Abstractions;
using Xunit;

namespace Kleinrechner.SplishSplash.Backend.GpioService.Test
{
    public class GpioPinWrapperFactoryTest
    {
        [Fact]
        public void GetGpioWrapper_AsDummy()
        {
            // Arrange
            var gpioPinWrapperFactory = PrepareGpioPinWrapperFactory();
            var pinNumber = 1;

            // Act
            var gpioPin = gpioPinWrapperFactory.Get(pinNumber);

            // Assert
            gpioPin.Should().NotBeNull();
            gpioPin.Should().BeOfType<DummyGpioPinWrapper>();
            gpioPin.GpioPinNumber.Should().Be(pinNumber);
        }

        [Fact]
        public void GetGpioWrapper_ThrowInvalidNegativeGpioNumberException()
        {
            // Arrange
            var gpioPinWrapperFactory = PrepareGpioPinWrapperFactory();
            var pinNumber = -1;

            // Act
            var ex = Assert.Throws<InvalidGpioPinNumberException>(() => gpioPinWrapperFactory.Get(pinNumber));

            // Assert
            ex.Should().NotBeNull();
            ex.Message.Should().Be("Number of gpio pin is not valid!");
        }

        [Fact]
        public void GetGpioWrapper_ThrowInvalidGpioNumberException()
        {
            // Arrange
            var gpioPinWrapperFactory = PrepareGpioPinWrapperFactory();
            var pinNumber = 32;

            // Act
            var ex = Assert.Throws<InvalidGpioPinNumberException>(() => gpioPinWrapperFactory.Get(pinNumber));

            // Assert
            ex.Should().NotBeNull();
            ex.Message.Should().Be("Number of gpio pin is not valid!");
        }

        [Fact]
        public void GetAllGpioWrapper()
        {
            // Arrange
            var gpioPinWrapperFactory = PrepareGpioPinWrapperFactory();

            // Act
            var allGpioPinWrapper = gpioPinWrapperFactory.GetAll();

            // Assert
            allGpioPinWrapper.Should().NotBeNullOrEmpty()
                .And.NotContainNulls()
                .And.OnlyHaveUniqueItems(x => x.GpioPinNumber);
        }

        private IGpioPinWrapperFactory PrepareGpioPinWrapperFactory()
        {
            var webHostEnvironment = new Mock<IWebHostEnvironment>();
            var eventProvider = new Mock<IEventPublisher>();
            var gpioPinWrapperLogger = new Mock<ILogger<GpioPinWrapper>>();
            var logger = new Mock<ILogger<GpioPinWrapperFactory>>();

            var services = new ServiceCollection();
            services.AddTransient<IWebHostEnvironment>(provider => webHostEnvironment.Object);
            services.AddTransient<IEventPublisher>(provider => eventProvider.Object);
            services.AddTransient<ILogger<GpioPinWrapper>>(provider => gpioPinWrapperLogger.Object);
            services.AddTransient<ILogger<GpioPinWrapperFactory>>(provider => logger.Object);
            services.AddTransient<IGpioPinWrapperFactory, GpioPinWrapperFactory>();

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetService<IGpioPinWrapperFactory>();
        }
    }
}
