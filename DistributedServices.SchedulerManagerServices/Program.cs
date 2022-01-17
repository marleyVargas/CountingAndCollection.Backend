using DistributedServices.SchedulerManagerServices.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistributedServices.SchedulerManagerServices
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(@"C:\Temp\SchedulerManagerService\LogFile.txt")
                .CreateLogger();

            try
            {
                Log.Information("Starting...");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception Ex)
            {
                Log.Information($"Error Starting - {Ex.Message}");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.ConfigureLogging(logging =>
                //{
                //    logging.AddEventLog(eventLogSettings =>
                //    {
                //        eventLogSettings.SourceName = "PaymentButtonAutomaticTask";
                //    });
                //})
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                //.ConfigureServices((hostContext, services)=> {
                //    services.AddHostedService<WorkerService>();
                //})
                .UseSerilog();
                //.UseWindowsService();
    }
}
