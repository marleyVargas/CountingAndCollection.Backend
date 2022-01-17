using DistributedServices.SchedulerManagerServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistributedServices.SchedulerManagerServices.Tasks
{
    public class SchedulerConfig<T> : ISchedulerConfig<T>
    {
        public string CronExpression
        {
            get; set;
        }

        public TimeZoneInfo TimeZoneInfo
        {
            get; set;
        }

        public int Delay
        {
            get; set;
        }
    }
}
