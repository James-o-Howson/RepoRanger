using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Abstractions.Interfaces.Persistence;
using RepoRanger.Domain.VersionControlSystems.Events;

namespace RepoRanger.EventHandlers.Implementations;

internal sealed class RepositoryDeletedEventHandler : INotificationHandler<RepositoryDeletedEvent>
{
    private readonly IApplicationDbContext _dbContext;

    public RepositoryDeletedEventHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(RepositoryDeletedEvent notification, CancellationToken cancellationToken)
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