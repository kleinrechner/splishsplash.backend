using System;
using System.Collections.Generic;
using System.Text;

namespace Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions.Models
{
    public class BackendCommandFailedModel : BaseHubModel
    {
        #region Fields

        public string MethodFailed { get; set; }
        
        public string ErrorMessage { get; set; }

        #endregion

        #region Ctor
        #endregion

        #region Methods
        #endregion
    }
}
