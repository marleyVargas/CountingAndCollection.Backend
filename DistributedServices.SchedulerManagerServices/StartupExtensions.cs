using DistributedServices.SchedulerManagerServices.Interfaces;
using DistributedServices.SchedulerManagerServices.Services;
using DistributedServices.SchedulerManagerServices.Tasks;
using Insfraestructure.PrincipalContext.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistributedServices.SchedulerManagerServices
{
    public static class StartupExtensions
    {
        public static void AddDBContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("PaymentButton");

            int timeOutSeconds = 1200;

            string timeOutMinutes = configuration.GetValue<String>("TimeOutSettings:DataBase");

            timeOutSeconds = Convert.ToInt32(timeOutMinutes) * 60;

            services.AddDbContext<CountingAndCollectionContext>(options => options.UseSqlServer(connectionString, sqlServerOptions => sqlServerOptions.CommandTimeout(timeOutSeconds)));
        }

        public static void AddCronJob<T>(this IServiceCollection services, Action<ISchedulerConfig<T>> options)
            where T : CronJobService
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options), @"Please provide Scheduler Configurations.");
            }

            var config = new SchedulerConfig<T>();
            options.Invoke(config);

            if (string.IsNullOrWhiteSpace(config.CronExpression))
            {
                throw new ArgumentNullException(nameof(SchedulerConfig<T>.CronExpression), @"Empty Cron Expression is not allowed.");
            }

            services.AddSingleton<ISchedulerConfig<T>>(config);
            services.AddHostedService<T>();
        }
    }
}
