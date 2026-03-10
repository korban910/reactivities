using Application.Interfaces;
using Domain;
using Infrastructure.Constant;
using Infrastructure.Email;
using Infrastructure.Photos;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Resend;

namespace Infrastructure;

public static class ServiceRegistration
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyName.IsActivityHost, policy =>
            {
                policy.Requirements.Add(new IsHostRequirement());
            });
        });

        services.Configure<CloudinarySettings>(options =>
        {
            options.CloudName = Environment.GetEnvironmentVariable("CLOUD_NAME")!;
            options.ApiKey = Environment.GetEnvironmentVariable("CLOUD_API_KEY")!;
            options.ApiSecret = Environment.GetEnvironmentVariable("CLOUD_API_SECRET")!;
        });

        services.AddHttpClient<ResendClient>();

        services.Configure<ResendClientOptions>(options =>
        {
            options.ApiToken = Environment.GetEnvironmentVariable("RESEND_API_KEY")!;
        });
        
        services.AddTransient<IAuthorizationHandler, IsHostRequirementHandler>();
        services.AddTransient<IResend, ResendClient>();
        services.AddTransient<IEmailSender<User>, EmailSender>();
        services.AddScoped<IPhotoService, PhotoService>();
        services.AddScoped<IUserAccessor, UserAccessor>();
    }
}