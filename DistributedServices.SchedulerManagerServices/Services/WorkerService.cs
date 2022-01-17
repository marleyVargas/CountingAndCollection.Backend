using Application.PrincipalContext.Interfaces.SchedulerManager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace DistributedServices.SchedulerManagerServices.Services
{
    public class WorkerService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<WorkerService> _logger;

        public WorkerService(ILogger<WorkerService> logger, IServiceProvider serviceProvider)
        {
            this._logger = logger;
            this._serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            this._logger.LogDebug($"{nameof(WorkerService)} is starting.");
            await DoWorkAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            this._logger.LogDebug($"{nameof(WorkerService)} is stopping.");
            await StopAsync(cancellationToken);
        }

        private async Task DoWorkAsync()
        {
            _logger.LogDebug($"{nameof(WorkerService)} is working.");

            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                ISchedulerManagerService schedulerManagerService =
                    scope.ServiceProvider.GetRequiredService<ISchedulerManagerService>();

                await schedulerManagerService.DoWorkAsync();
            }

        }
    }
}
