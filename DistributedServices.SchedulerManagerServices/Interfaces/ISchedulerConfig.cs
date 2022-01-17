using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistributedServices.SchedulerManagerServices.Interfaces
{
    public interface ISchedulerConfig<T>
    {
        string CronExpression
        {
            get; set;
        }

        TimeZoneInfo TimeZoneInfo
        {
            get; set;
        }
    }
}
