using API.Middleware;
using Domain;
using Microsoft.AspNetCore.Identity;
using Persistence.Contexts;

namespace API;

public static class ServiceRegistration
{
    public static void AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("Development", builder =>
                builder.WithOrigins("http://localhost:3000", "https://localhost:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .AllowCredentials());
        });
    }
    
    public static void AddMiddleWare(this IServiceCollection services)
    {
        services.AddTransient<ExceptionMiddleware>();
        services.AddIdentityApiEndpoints<User>(opt =>
        {
            opt.User.RequireUniqueEmail = true;
            opt.SignIn.RequireConfirmedEmail = true;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>();
        services.AddSignalR();
    }
}