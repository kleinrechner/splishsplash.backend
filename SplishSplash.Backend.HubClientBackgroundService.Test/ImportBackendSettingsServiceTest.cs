using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Kleinrechner.SplishSplash.Backend.GpioService.Abstractions.Models;
using Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions;
using Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions.Models;
using Kleinrechner.SplishSplash.Backend.SchedulerBackgroundService.Abstractions;
using Kleinrechner.SplishSplash.Backend.SchedulerBackgroundService.Abstractions.Models;
using Kleinrechner.SplishSplash.Backend.SettingsService.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Unosquare.RaspberryIO.Abstractions;
using Xunit;

namespace Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Test
{
    public class ImportBackendSettingsServiceTest
    {
        private readonly DateTime getNextExecutenTime = DateTime.Now.Date.AddYears(1);

        [Fact]
        public void Import_BaseDataWithEmptyLists()
        {
            // Arrange
            var backendSettings = new BackendSettings();

            var settingsService = new Mock<ISettingsService>();
            settingsService.Setup(x => x.GetSettings())
                .Returns(backendSettings);

            var importBackendSettingsService = PrepareService(settingsService.Object);

            var backendSettingsHubModel = new BackendSettingsHubModel();
            backendSettingsHubModel.DisplayName = "DisplayName";
            backendSettingsHubModel.Icon = "Icon";
            backendSettingsHubModel.OrderNumber = 1;
            
            // Act
            importBackendSettingsService.ImportBackendSettingsHubModel(backendSettingsHubModel);

            // Assert
            settingsService.Verify(x => x.Save(It.IsAny<BackendSettings>()), Times.Once);


            backendSettings.DisplayName.Should().Be(backendSettingsHubModel.DisplayName);
            backendSettings.Icon.Should().Be(backendSettingsHubModel.Icon);
            backendSettings.OrderNumber.Should().Be(backendSettingsHubModel.OrderNumber);
            backendSettings.PinMap.Should().BeNullOrEmpty();
            backendSettings.SchedulerSettings.Should().BeNullOrEmpty();
        }

        [Fact]
        public void Import_PinMap()
        {
            // Arrange
            var backendSettings = new BackendSettings();

            var settingsService = new Mock<ISettingsService>();
            settingsService.Setup(x => x.GetSettings())
                .Returns(backendSettings);

            var importBackendSettingsService = PrepareService(settingsService.Object);

            var pinMap = new PinMapModel();
            pinMap.DisplayName = "DisplayName";
            pinMap.GpioPinNumber = 9;
            pinMap.OrderNumber = 10;
            pinMap.Icon = "Icon";

            var backendSettingsHubModel = new BackendSettingsHubModel();
            backendSettingsHubModel.PinMap = new List<PinMapModel>(new []{ pinMap });

            // Act
            importBackendSettingsService.ImportBackendSettingsHubModel(backendSettingsHubModel);

            // Assert
            settingsService.Verify(x => x.Save(It.IsAny<BackendSettings>()), Times.Once);

            backendSettings.PinMap.Should().NotBeNullOrEmpty();
            backendSettings.PinMap.Should().HaveCount(1);
            backendSettings.PinMap.Single().DisplayName.Should().Be(pinMap.DisplayName);
            backendSettings.PinMap.Single().GpioPinNumber.Should().Be(pinMap.GpioPinNumber);
            backendSettings.PinMap.Single().OrderNumber.Should().Be(pinMap.OrderNumber);
            backendSettings.PinMap.Single().Icon.Should().Be(pinMap.Icon);
            backendSettings.SchedulerSettings.Should().BeNullOrEmpty();
        }

        [Fact]
        public void Import_DontImportEmptySchedulerTaskSettings()
        {
            // Arrange
            var backendSettings = new BackendSettings();

            var settingsService = new Mock<ISettingsService>();
            settingsService.Setup(x => x.GetSettings())
                .Returns(backendSettings);

            var importBackendSettingsService = PrepareService(settingsService.Object);

            var schedulerTaskSettings = new SchedulerTaskSettings();

            var backendSettingsHubModel = new BackendSettingsHubModel();
            backendSettingsHubModel.SchedulerSettings = new List<SchedulerTaskSettings>(new[] { schedulerTaskSettings });

            // Act
            importBackendSettingsService.ImportBackendSettingsHubModel(backendSettingsHubModel);

            // Assert
            settingsService.Verify(x => x.Save(It.IsAny<BackendSettings>()), Times.Once);

            backendSettings.SchedulerSettings.Should().BeNullOrEmpty();
        }

        [Fact]
        public void Import_AddSchedulerTaskSettings()
        {
            // Arrange
            var backendSettings = new BackendSettings();

            var settingsService = new Mock<ISettingsService>();
            settingsService.Setup(x => x.GetSettings())
                .Returns(backendSettings);

            var importBackendSettingsService = PrepareService(settingsService.Object);

            var schedulerTaskSettings = new SchedulerTaskSettings();
            schedulerTaskSettings.Id = Guid.NewGuid();
            schedulerTaskSettings.DisplayName = "DisplayName";
            schedulerTaskSettings.OrderNumber = 9;
            schedulerTaskSettings.Icon = "Icon";
            schedulerTaskSettings.CronExpression = "CronExpression";
            schedulerTaskSettings.NextRuntime = null;
            schedulerTaskSettings.LastRunTimeSucceeded = null;
            schedulerTaskSettings.LastRunTimeFailed = null;
            schedulerTaskSettings.ChangeGpioPins = null;

            var backendSettingsHubModel = new BackendSettingsHubModel();
            backendSettingsHubModel.SchedulerSettings = new List<SchedulerTaskSettings>(new[] { schedulerTaskSettings });

            // Act
            importBackendSettingsService.ImportBackendSettingsHubModel(backendSettingsHubModel);

            // Assert
            settingsService.Verify(x => x.Save(It.IsAny<BackendSettings>()), Times.Once);

            backendSettings.SchedulerSettings.Should().NotBeNullOrEmpty();
            backendSettings.SchedulerSettings.Should().HaveCount(1);
            backendSettings.SchedulerSettings.Single().Id.Should().Be(schedulerTaskSettings.Id);
            backendSettings.SchedulerSettings.Single().DisplayName.Should().Be(schedulerTaskSettings.DisplayName);
            backendSettings.SchedulerSettings.Single().OrderNumber.Should().Be(schedulerTaskSettings.OrderNumber);
            backendSettings.SchedulerSettings.Single().Icon.Should().Be(schedulerTaskSettings.Icon);
            backendSettings.SchedulerSettings.Single().CronExpression.Should().Be(schedulerTaskSettings.CronExpression);
            backendSettings.SchedulerSettings.Single().NextRuntime.Should().Be(getNextExecutenTime);
            backendSettings.SchedulerSettings.Single().LastRunTimeSucceeded.Should().Be(schedulerTaskSettings.LastRunTimeSucceeded);
            backendSettings.SchedulerSettings.Single().LastRunTimeFailed.Should().Be(schedulerTaskSettings.LastRunTimeFailed);
            backendSettings.SchedulerSettings.Single().ChangeGpioPins.Should().BeNullOrEmpty();
        }

        [Fact]
        public void Import_AddSchedulerTaskSettingsWithChangeGpioPins()
        {
            // Arrange
            var backendSettings = new BackendSettings();

            var settingsService = new Mock<ISettingsService>();
            settingsService.Setup(x => x.GetSettings())
                .Returns(backendSettings);

            var importBackendSettingsService = PrepareService(settingsService.Object);

            var changeGpioPinModel = new ChangeGpioPinModel()
            {
                Value = true,
                Mode = GpioPinDriveMode.Output,
                GpioPinNumber = 9
            };

            var schedulerTaskSettings = new SchedulerTaskSettings();
            schedulerTaskSettings.Id = Guid.NewGuid();
            schedulerTaskSettings.CronExpression = "CronExpression";
            schedulerTaskSettings.ChangeGpioPins = new List<ChangeGpioPinModel>(new []{ changeGpioPinModel });

            var backendSettingsHubModel = new BackendSettingsHubModel();
            backendSettingsHubModel.SchedulerSettings = new List<SchedulerTaskSettings>(new[] { schedulerTaskSettings });

            // Act
            importBackendSettingsService.ImportBackendSettingsHubModel(backendSettingsHubModel);

            // Assert
            settingsService.Verify(x => x.Save(It.IsAny<BackendSettings>()), Times.Once);

            backendSettings.SchedulerSettings.Should().NotBeNullOrEmpty();
            backendSettings.SchedulerSettings.Should().HaveCount(1);
            backendSettings.SchedulerSettings.Single().Id.Should().Be(schedulerTaskSettings.Id);
            backendSettings.SchedulerSettings.Single().CronExpression.Should().Be(schedulerTaskSettings.CronExpression);
            backendSettings.SchedulerSettings.Single().ChangeGpioPins.Should().NotBeNullOrEmpty();
            backendSettings.SchedulerSettings.Single().ChangeGpioPins.Should().HaveCount(1);
            backendSettings.SchedulerSettings.Single().ChangeGpioPins.Single().Value.Should().Be(changeGpioPinModel.Value);
            backendSettings.SchedulerSettings.Single().ChangeGpioPins.Single().Mode.Should().Be(changeGpioPinModel.Mode);
            backendSettings.SchedulerSettings.Single().ChangeGpioPins.Single().GpioPinNumber.Should().Be(changeGpioPinModel.GpioPinNumber);
        }

        [Fact]
        public void Import_DontUpdateSchedulerTaskSettingsBasics()
        {
            // Arrange
            var backendSettingsId = Guid.NewGuid();

            var schedulerTaskSettings = new SchedulerTaskSettings();
            schedulerTaskSettings.Id = backendSettingsId;
            schedulerTaskSettings.DisplayName = "asfasfafaf";
            schedulerTaskSettings.OrderNumber = 9423423;
            schedulerTaskSettings.Icon = "afafasfd";
            schedulerTaskSettings.CronExpression = "afafasfdasfda";
            schedulerTaskSettings.NextRuntime = DateTime.Now;
            schedulerTaskSettings.LastRunTimeSucceeded = DateTime.Now;
            schedulerTaskSettings.LastRunTimeFailed = DateTime.Now;

            var backendSettings = new BackendSettings();
            backendSettings.SchedulerSettings = new List<SchedulerTaskSettings>(new[] { schedulerTaskSettings });

            var settingsService = new Mock<ISettingsService>();
            settingsService.Setup(x => x.GetSettings())
                .Returns(backendSettings);

            var importBackendSettingsService = PrepareService(settingsService.Object);

            var newSchedulerTaskSettings = new SchedulerTaskSettings();
            newSchedulerTaskSettings.Id = backendSettingsId;
            newSchedulerTaskSettings.DisplayName = "DisplayName";
            newSchedulerTaskSettings.OrderNumber = 9;
            newSchedulerTaskSettings.Icon = "Icon";
            newSchedulerTaskSettings.CronExpression = "CronExpression";
            newSchedulerTaskSettings.NextRuntime = getNextExecutenTime;
            newSchedulerTaskSettings.LastRunTimeSucceeded = getNextExecutenTime;
            newSchedulerTaskSettings.LastRunTimeFailed = getNextExecutenTime;
            newSchedulerTaskSettings.ChangeGpioPins = null;

            var backendSettingsHubModel = new BackendSettingsHubModel();
            backendSettingsHubModel.SchedulerSettings = new List<SchedulerTaskSettings>(new[] { newSchedulerTaskSettings });

            // Act
            importBackendSettingsService.ImportBackendSettingsHubModel(backendSettingsHubModel);

            // Assert
            settingsService.Verify(x => x.Save(It.IsAny<BackendSettings>()), Times.Once);

            backendSettings.SchedulerSettings.Should().NotBeNullOrEmpty();
            backendSettings.SchedulerSettings.Should().HaveCount(1);
            backendSettings.SchedulerSettings.Single().Id.Should().Be(schedulerTaskSettings.Id);
            backendSettings.SchedulerSettings.Single().DisplayName.Should().Be(schedulerTaskSettings.DisplayName);
            backendSettings.SchedulerSettings.Single().OrderNumber.Should().Be(schedulerTaskSettings.OrderNumber);
            backendSettings.SchedulerSettings.Single().Icon.Should().Be(schedulerTaskSettings.Icon);
            backendSettings.SchedulerSettings.Single().CronExpression.Should().Be(schedulerTaskSettings.CronExpression);
            backendSettings.SchedulerSettings.Single().NextRuntime.Should().Be(schedulerTaskSettings.NextRuntime);
            backendSettings.SchedulerSettings.Single().LastRunTimeSucceeded.Should().Be(schedulerTaskSettings.LastRunTimeSucceeded);
            backendSettings.SchedulerSettings.Single().LastRunTimeFailed.Should().Be(schedulerTaskSettings.LastRunTimeFailed);
        }

        [Fact]
        public void Import_UpdateSchedulerTaskSettingsBasics()
        {
            // Arrange
            var backendSettingsId = Guid.NewGuid();

            var schedulerTaskSettings = new SchedulerTaskSettings();
            schedulerTaskSettings.Id = backendSettingsId;
            schedulerTaskSettings.CronExpression = "afafasfdasfda";
            schedulerTaskSettings.NextRuntime = DateTime.Now;
            schedulerTaskSettings.LastRunTimeSucceeded = DateTime.Now;
            schedulerTaskSettings.LastRunTimeFailed = DateTime.Now;

            var backendSettings = new BackendSettings();
            backendSettings.SchedulerSettings = new List<SchedulerTaskSettings>(new[] { schedulerTaskSettings });

            var settingsService = new Mock<ISettingsService>();
            settingsService.Setup(x => x.GetSettings())
                .Returns(backendSettings);

            var importBackendSettingsService = PrepareService(settingsService.Object);

            var newSchedulerTaskSettings = new SchedulerTaskSettings();
            newSchedulerTaskSettings.Id = backendSettingsId;
            newSchedulerTaskSettings.CronExpression = "CronExpression";
            newSchedulerTaskSettings.NextRuntime = null;
            newSchedulerTaskSettings.LastRunTimeSucceeded = getNextExecutenTime;
            newSchedulerTaskSettings.LastRunTimeFailed = getNextExecutenTime;

            var backendSettingsHubModel = new BackendSettingsHubModel();
            backendSettingsHubModel.SchedulerSettings = new List<SchedulerTaskSettings>(new[] { newSchedulerTaskSettings });

            // Act
            importBackendSettingsService.ImportBackendSettingsHubModel(backendSettingsHubModel);

            // Assert
            settingsService.Verify(x => x.Save(It.IsAny<BackendSettings>()), Times.Once);

            backendSettings.SchedulerSettings.Should().NotBeNullOrEmpty();
            backendSettings.SchedulerSettings.Should().HaveCount(1);
            backendSettings.SchedulerSettings.Single().Id.Should().Be(newSchedulerTaskSettings.Id);
            backendSettings.SchedulerSettings.Single().CronExpression.Should().Be(newSchedulerTaskSettings.CronExpression);
            backendSettings.SchedulerSettings.Single().NextRuntime.Should().Be(getNextExecutenTime);
            backendSettings.SchedulerSettings.Single().LastRunTimeSucceeded.Should().Be(schedulerTaskSettings.LastRunTimeSucceeded);
            backendSettings.SchedulerSettings.Single().LastRunTimeFailed.Should().Be(schedulerTaskSettings.LastRunTimeFailed);
        }

        [Fact]
        public void Import_UpdateSchedulerTaskSettingsDontUpdateTaskTimes()
        {
            // Arrange
            var backendSettingsId = Guid.NewGuid();

            var schedulerTaskSettings = new SchedulerTaskSettings();
            schedulerTaskSettings.Id = backendSettingsId;
            schedulerTaskSettings.DisplayName = "asfasfafaf";
            schedulerTaskSettings.OrderNumber = 9423423;
            schedulerTaskSettings.Icon = "afafasfd";
            schedulerTaskSettings.CronExpression = "afafasfdasfda";
            schedulerTaskSettings.NextRuntime = null;

            var backendSettings = new BackendSettings();
            backendSettings.SchedulerSettings = new List<SchedulerTaskSettings>(new[] { schedulerTaskSettings });

            var settingsService = new Mock<ISettingsService>();
            settingsService.Setup(x => x.GetSettings())
                .Returns(backendSettings);

            var importBackendSettingsService = PrepareService(settingsService.Object);

            var newSchedulerTaskSettings = new SchedulerTaskSettings();
            newSchedulerTaskSettings.Id = backendSettingsId;
            newSchedulerTaskSettings.DisplayName = "DisplayName";
            newSchedulerTaskSettings.OrderNumber = 9;
            newSchedulerTaskSettings.Icon = "Icon";
            newSchedulerTaskSettings.CronExpression = "CronExpression";
            newSchedulerTaskSettings.NextRuntime = null;
            newSchedulerTaskSettings.LastRunTimeSucceeded = null;
            newSchedulerTaskSettings.LastRunTimeFailed = null;
            newSchedulerTaskSettings.ChangeGpioPins = null;

            var backendSettingsHubModel = new BackendSettingsHubModel();
            backendSettingsHubModel.SchedulerSettings = new List<SchedulerTaskSettings>(new[] { newSchedulerTaskSettings });

            // Act
            importBackendSettingsService.ImportBackendSettingsHubModel(backendSettingsHubModel);

            // Assert
            settingsService.Verify(x => x.Save(It.IsAny<BackendSettings>()), Times.Once);

            backendSettings.SchedulerSettings.Should().NotBeNullOrEmpty();
            backendSettings.SchedulerSettings.Should().HaveCount(1);
            backendSettings.SchedulerSettings.Single().Id.Should().Be(newSchedulerTaskSettings.Id);
            backendSettings.SchedulerSettings.Single().DisplayName.Should().Be(newSchedulerTaskSettings.DisplayName);
            backendSettings.SchedulerSettings.Single().OrderNumber.Should().Be(newSchedulerTaskSettings.OrderNumber);
            backendSettings.SchedulerSettings.Single().Icon.Should().Be(newSchedulerTaskSettings.Icon);
            backendSettings.SchedulerSettings.Single().CronExpression.Should().Be(newSchedulerTaskSettings.CronExpression);
            backendSettings.SchedulerSettings.Single().NextRuntime.Should().Be(getNextExecutenTime);
        }

        [Fact]
        public void Import_RemoveSchedulerTaskSettings()
        {
            // Arrange
            var schedulerTaskSettings = new SchedulerTaskSettings();
            schedulerTaskSettings.Id = Guid.NewGuid();
            var backendSettings = new BackendSettings();
            backendSettings.SchedulerSettings = new List<SchedulerTaskSettings>(new []{ schedulerTaskSettings });

            var settingsService = new Mock<ISettingsService>();
            settingsService.Setup(x => x.GetSettings())
                .Returns(backendSettings);

            var importBackendSettingsService = PrepareService(settingsService.Object);

            var backendSettingsHubModel = new BackendSettingsHubModel();

            // Act
            importBackendSettingsService.ImportBackendSettingsHubModel(backendSettingsHubModel);

            // Assert
            settingsService.Verify(x => x.Save(It.IsAny<BackendSettings>()), Times.Once);

            backendSettings.SchedulerSettings.Should().BeNullOrEmpty();
        }

        private IImportBackendSettingsService PrepareService(ISettingsService settingsService)
        {
            var cronExpressionService = new Mock<ICronExpressionService>();
            cronExpressionService.Setup(x => x.GetNextExecutenTime(It.IsAny<string>()))
                .Returns(getNextExecutenTime);

            var services = new ServiceCollection();

            services.AddTransient<ISettingsService>(x => settingsService);
            services.AddTransient<ICronExpressionService>(x => cronExpressionService.Object);
            services.AddTransient<IImportBackendSettingsService, ImportBackendSettingsService>();

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetService<IImportBackendSettingsService>();
        }
    }
}
