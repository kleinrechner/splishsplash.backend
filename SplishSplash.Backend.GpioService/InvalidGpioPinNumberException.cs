using System;
using System.Collections.Generic;
using System.Text;

namespace Kleinrechner.SplishSplash.Backend.GpioService
{
    public class InvalidGpioPinNumberException : Exception
    {
        #region Fields
        #endregion

        #region Ctor

        public InvalidGpioPinNumberException() : base("Number of gpio pin is not valid!")
        {

        }

        #endregion

        #region Methods
        #endregion
    }
}
