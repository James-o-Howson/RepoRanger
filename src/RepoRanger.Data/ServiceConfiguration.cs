using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RepoRanger.Abstractions.Interfaces.Data;
using RepoRanger.Data.Abstractions;
using RepoRanger.Data.Interceptors;
using RepoRanger.Data.Repositories;
using RepoRanger.Domain.OutboxMessages;
using SharedKernel.Abstractions;

namespace RepoRanger.Data;

public static class ServiceConfiguration
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ConnectionStringOptions>(configuration.GetSection(ConnectionStringOptions.SectionKey));
        
        services.AddScoped<ISaveChangesInterceptor, AuditableEntitySaveChangesInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DomainEventsSaveChangesInterceptor>();
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var connectionStringOptions = sp.GetRequiredService<IOptions<ConnectionStringOptions>>().Value;
            
            options.UseSqlite(connectionStringOptions.RepoRangerDatabase).LogTo(Console.WriteLine, LogLevel.Information);
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
        });
        
        services.AddScoped<IApplicationDbContext>(GetDbContext);
        services.AddScoped<IUnitOfWork>(GetDbContext);

        services.AddScoped<IOutboxMessageRepository, OutboxMessageRepository>();
    }

    private static ApplicationDbContext GetDbContext(IServiceProvider provider) => provider.GetRequiredService<ApplicationDbContext>();
}