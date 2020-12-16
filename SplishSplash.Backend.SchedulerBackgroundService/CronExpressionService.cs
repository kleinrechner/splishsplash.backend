using System;
using System.Collections.Generic;
using System.Text;
using Kleinrechner.SplishSplash.Backend.SchedulerBackgroundService.Abstractions;
using NCrontab;
using DateTime = System.DateTime;

namespace Kleinrechner.SplishSplash.Backend.SchedulerBackgroundService
{
    public class CronExpressionService : ICronExpressionService
    {
        public DateTime? GetNextExecutenTime(string cronExpression)
        {
            var crontabSchedule = CrontabSchedule.TryParse(cronExpression);
            if (crontabSchedule != null)
            {
                var nextOccurrence = crontabSchedule.GetNextOccurrence(DateTime.Now);
                return new DateTime(nextOccurrence.Year, nextOccurrence.Month, nextOccurrence.Day, nextOccurrence.Hour, nextOccurrence.Minute, 0);
            }

            return null;
        }
    }
}
