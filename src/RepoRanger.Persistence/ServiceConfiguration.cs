using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RepoRanger.Application.Abstractions.Interfaces;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;
using RepoRanger.Persistence.Common;
using RepoRanger.Persistence.Interceptors;
using RepoRanger.Persistence.Services;

namespace RepoRanger.Persistence;

public static class ServiceConfiguration
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ConnectionStringOptions>(configuration.GetSection("ConnectionStrings"));
        
        services.AddScoped<ISaveChangesInterceptor, AuditableEntitySaveChangesInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, EventDispatcherSaveChangesInterceptor>();
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var connectionStringOptions = sp.GetRequiredService<IOptions<ConnectionStringOptions>>().Value;
            
            options.UseSqlite(connectionStringOptions.RepoRangerDatabase).LogTo(Console.WriteLine, LogLevel.Information);
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
        });
        
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddTransient<IResourceNameService, SqlFileResourceNameService>();
        services.AddTransient<ISqlFileExecutorService, SqlFileExecutorService>();
    }
}