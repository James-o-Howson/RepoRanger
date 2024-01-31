using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepoRanger.Application.Abstractions.Interfaces;
using RepoRanger.Application.Options;
using RepoRanger.Persistence.Interceptors;

namespace RepoRanger.Persistence;

public static class ServiceConfiguration
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISaveChangesInterceptor, AuditableEntitySaveChangesInterceptor>();
        
        var connectionStringOptions = services.RegisterOptions<ConnectionStringsOptions>(configuration);
        
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseSqlite(connectionStringOptions.RepoRangerDatabase);
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
        });
        
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
    }
}