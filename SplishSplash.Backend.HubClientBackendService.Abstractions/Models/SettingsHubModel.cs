using System;
using System.Collections.Generic;
using System.Text;
using Kleinrechner.SplishSplash.Backend.SchedulerBackgroundService.Abstractions.Models;

namespace Kleinrechner.SplishSplash.Backend.HubClientBackgroundService.Abstractions.Models
{
    public class SettingsHubModel : BaseHubModel
    {
        #region Fields

        public string DisplayName { get; set; }

        public int OrderNumber { get; set; }

        public string Icon { get; set; }
        
        public List<PinMapModel> PinMap { get; set; }

        public SchedulerBackgroundServiceSettings SchedulerSettings { get; set; }

        #endregion

        #region Ctor
        #endregion

        #region Methods
        #endregion
    }
}
