using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepoRanger.Abstractions.Events.Domain;
using RepoRanger.Abstractions.Interfaces.Data;
using RepoRanger.Domain.VersionControlSystems.Events;

namespace RepoRanger.EventHandlers.Domain;

internal sealed class RepositoryDeletedEventHandler : DomainEventHandler<RepositoryDeletedDomainEvent>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ILogger<RepositoryDeletedEventHandler> _logger;

    public RepositoryDeletedEventHandler(IApplicationDbContext dbContext, ILogger<RepositoryDeletedEventHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    protected override async Task HandleAsync(RepositoryDeletedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await DeleteOrphanedVersionsAsync(cancellationToken);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Repository with Id = {Id} has been deleted. Orphaned Dependencies cleaned successfully", domainEvent.RepositoryId);
    }

    private async Task DeleteOrphanedVersionsAsync(CancellationToken cancellationToken)
    {
        await _dbContext.DependencyVersions
            .Where(v => v.ProjectDependencies.Count == 0)
            .ExecuteDeleteAsync(cancellationToken: cancellationToken);
    }
}