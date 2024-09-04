using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RepoRanger.Abstractions.Interfaces.Persistence;
using RepoRanger.Data.Common;
using RepoRanger.Data.Interceptors;

namespace RepoRanger.Data;

public static class ServiceConfiguration
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ConnectionStringOptions>(configuration.GetSection("ConnectionStrings"));
        
        services.AddScoped<ISaveChangesInterceptor, AuditableEntitySaveChangesInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, PersistEventsSaveChangesInterceptor>();
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var connectionStringOptions = sp.GetRequiredService<IOptions<ConnectionStringOptions>>().Value;
            
            options.UseSqlite(connectionStringOptions.RepoRangerDatabase).LogTo(Console.WriteLine, LogLevel.Information);
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
        });
        
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
    }
}