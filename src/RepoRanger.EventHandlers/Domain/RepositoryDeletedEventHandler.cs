using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Abstractions.Interfaces;
using RepoRanger.Domain.VersionControlSystems.Events;

namespace RepoRanger.EventHandlers.Domain;

internal sealed class RepositoryDeletedEventHandler : INotificationHandler<RepositoryDeletedDomainEvent>
{
    private readonly IApplicationDbContext _dbContext;

    public RepositoryDeletedEventHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(RepositoryDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        await DeleteOrphanedVersionsAsync(cancellationToken);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task DeleteOrphanedVersionsAsync(CancellationToken cancellationToken)
    {
        await _dbContext.DependencyVersions
            .Where(v => v.ProjectDependencies.Count == 0)
            .ExecuteDeleteAsync(cancellationToken: cancellationToken);
    }
}