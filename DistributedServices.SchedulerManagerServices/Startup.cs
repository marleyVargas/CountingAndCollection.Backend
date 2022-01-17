using Application.PrincipalContext.Interfaces.Application;
using Application.PrincipalContext.Interfaces.Notification;
using Application.PrincipalContext.Interfaces.Orchestrator;
using Application.PrincipalContext.Interfaces.SchedulerManager;
using Application.PrincipalContext.Interfaces.Transactional;
using Application.PrincipalContext.Services.Application;
using Application.PrincipalContext.Services.Notification;
using Application.PrincipalContext.Services.OrchestratorServices;
using Application.PrincipalContext.Services.SchedulerManager;
using Application.PrincipalContext.Services.Transactional;
using DistributedServices.SchedulerManagerServices.Tasks;
using Domain.Nucleus.Interfaces;
using Infraestructure.Transversal.Mail;
using Infraestructure.Transversal.SMS;
using Insfraestructure.PrincipalContext.Data;
using Insfraestructure.PrincipalContext.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace DistributedServices.SchedulerManagerServices
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration
        {
            get;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddScoped<ILoggerService, LoggerService>();
            services.AddScoped<IVehicleService, Application.PrincipalContext.Services.Transactional.VehicleService>();
            services.AddScoped<ILogRequestAndResponseService, LogRequestAndResponseService>();
            services.AddScoped<ICorrespondenceService, CorrespondenceService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddSingleton<IMailHandler>(_ => new MailHandler(new MailServer()));
            services.AddSingleton<ISMSHandler>(_ => new SMSHandler(new SMSSendRequest()));
            services.AddScoped<ISchedulerManagerService, SchedulerManagerService>();
            services.AddTransient(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddHttpContextAccessor();

            //services.AddDBContext(Configuration);
            services.AddDbContext<CountingAndCollectionContext>(options => options.UseSqlServer(Configuration.GetConnectionString("PaymentButton")), ServiceLifetime.Transient);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DistributedServices SchedulerManagerServices", Version = "v1" });
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddCronJob<ProcessTransaction>((config) =>
            {
                config.CronExpression = Configuration["ProcessTransactionCronExpression"];
                config.TimeZoneInfo = TimeZoneInfo.Local;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DistributedServices.SchedulerManagerServices v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
