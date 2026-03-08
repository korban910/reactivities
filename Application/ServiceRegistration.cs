using Application.Activities.Validators;
using Application.Core;
using Application.Core.Mapping;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        }); 
        
        services.AddAutoMapper(cfg => { }, typeof(ServiceRegistration).Assembly);
        services.AddValidatorsFromAssemblyContaining<CreateActivityValidator>();
    }
}