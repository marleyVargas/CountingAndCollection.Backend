using Cronos;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DistributedServices.SchedulerManagerServices.Services
{
    public class CronJobService : IHostedService, IDisposable
    {
        private System.Timers.Timer _timer;
        private readonly CronExpression _expression;
        private readonly TimeZoneInfo _timeZoneInfo;

        protected CronJobService(string cronExpression, TimeZoneInfo timeZoneInfo)
        {
            _expression = CronExpression.Parse(cronExpression);
            _timeZoneInfo = timeZoneInfo;
        }

        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            await ScheduleTask(cancellationToken);
        }

        protected virtual async Task ScheduleTask(CancellationToken cancellationToken)
        {
            var next = _expression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);

            if (next.HasValue)
            {
                var delay = next.Value - DateTimeOffset.Now;

                _timer = new System.Timers.Timer(delay.TotalMilliseconds);

                _timer.Elapsed += async (sender, args) =>
                {
                    // Resetear timer
                    _timer.Stop();

                    await DoWorkAsync(cancellationToken);

                    // Reprogramar siguiente tarea
                    await ScheduleTask(cancellationToken);    
                };

                _timer.Start();
            }

            await Task.CompletedTask;
        }

        public virtual async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            // Realizar trabajo.
            await Task.Delay(5000, cancellationToken);  
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Stop();
            await Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
