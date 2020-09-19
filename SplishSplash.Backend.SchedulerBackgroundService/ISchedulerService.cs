using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kleinrechner.SplishSplash.Backend.SchedulerBackgroundService
{
    public interface ISchedulerService
    {
        void ExecuteScheduler(object state);
    }
}
