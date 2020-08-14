﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Kleinrechner.SplishSplash.Backend.GpioService.Contract;

namespace Kleinrechner.SplishSplash.Backend.GpioService
{
    public class GpioService : IGpioService
    {
        #region Fields

        private readonly IGpioPinWrapperFactory _gpioPinWrapperFactory;
        private readonly ILogger<GpioService> _logger;

        #endregion

        #region Ctor

        public GpioService(IGpioPinWrapperFactory gpioPinWrapperFactory, ILogger<GpioService> logger)
        {
            _gpioPinWrapperFactory = gpioPinWrapperFactory;
            _logger = logger;
        }

        #endregion

        #region Methods

        public IEnumerable<IGpioPinWrapper> GetAllGpioPins()
        {
            return _gpioPinWrapperFactory.GetAll();
        }

        public IGpioPinWrapper GetGpioPin(int gpioPinNumber)
        {
            return _gpioPinWrapperFactory.Get(gpioPinNumber);
        }

        #endregion
    }
}
