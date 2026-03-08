using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;

namespace Persistence;

public static class ServiceRegistration
{
    public static void AddPersistence(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseSqlServer(Environment.GetEnvironmentVariable("DATABASE_URL"),
                b =>
                {
                    b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                    b.CommandTimeout(300);
                });
        });
    }
}