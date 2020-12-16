using System;
using System.Collections.Generic;
using System.Text;

namespace Kleinrechner.SplishSplash.Backend.SchedulerBackgroundService.Abstractions
{
    public interface ICronExpressionService
    {
        DateTime? GetNextExecutenTime(string cronExpression);
    }
}
