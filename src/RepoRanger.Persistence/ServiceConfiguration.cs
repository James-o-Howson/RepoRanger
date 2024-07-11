using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RepoRanger.Application.Common.Interfaces;
using RepoRanger.Application.Common.Interfaces.Persistence;
using RepoRanger.Domain.SourceParsing;
using RepoRanger.Persistence.Common;
using RepoRanger.Persistence.Interceptors;
using RepoRanger.Persistence.Repositories;
using RepoRanger.Persistence.Services;

namespace RepoRanger.Persistence;

public static class ServiceConfiguration
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ConnectionStringOptions>(configuration.GetSection("ConnectionStrings"));
        
        services.AddScoped<ISaveChangesInterceptor, AuditableEntitySaveChangesInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, EventDispatcherSaveChangesInterceptor>();
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var connectionStringOptions = sp.GetRequiredService<IOptions<ConnectionStringOptions>>().Value;
            
            options.UseSqlite(connectionStringOptions.RepoRangerDatabase);
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
        });
        
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddTransient<IResourceNameService, SqlFileResourceNameService>();
        services.AddTransient<ISqlFileExecutorService, SqlFileExecutorService>();
        services.AddTransient<ISourceRepository, SourceRepository>();
    }
}