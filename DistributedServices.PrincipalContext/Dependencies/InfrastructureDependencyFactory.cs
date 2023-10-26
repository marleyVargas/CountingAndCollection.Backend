
using Application.PrincipalContext.Interfaces.Application;
using Application.PrincipalContext.Interfaces.Transactional;
using Application.PrincipalContext.Services.Application;
using Application.PrincipalContext.Services.TransactionalServices;
using Domain.Nucleus.Interfaces;
using Infraestructure.Transversal.Mail;
using Infraestructure.Transversal.SMS;
using Insfraestructure.PrincipalContext.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedServices.Api.Dependencies
{
    public static class InfrastructureDependencyFactory
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<ValidationActionFilter>();

            services.AddScoped<IParameterService, ParameterService>();

            services.AddScoped<IVehicleService, VehicleService>();

            services.AddScoped<ILogRequestAndResponseService, LogRequestAndResponseService>();

            services.AddSingleton<IMailHandler>(_ => new MailHandler(new MailServer()));
            services.AddSingleton<ISMSHandler>(_ => new SMSHandler(new SMSSendRequest()));

            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }

    }
}