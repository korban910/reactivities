using Application.Interfaces;
using Infrastructure.Constant;
using Infrastructure.Photos;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

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
        
        services.AddTransient<IAuthorizationHandler, IsHostRequirementHandler>();
        services.AddScoped<IPhotoService, PhotoService>();
        services.AddScoped<IUserAccessor, UserAccessor>();
    }
}