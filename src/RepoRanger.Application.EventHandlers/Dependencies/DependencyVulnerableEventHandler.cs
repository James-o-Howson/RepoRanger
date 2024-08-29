using MediatR;
using RepoRanger.Application.Abstractions.Exceptions;
using RepoRanger.Application.Abstractions.Interfaces;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;
using RepoRanger.Domain.Dependencies.Events;

namespace RepoRanger.Application.EventHandlers.Dependencies;

internal sealed class DependencyVulnerableEventHandler : INotificationHandler<DependencyVulnerableEvent>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IExternalVulnerabilityService _vulnerabilityService;

    public DependencyVulnerableEventHandler(IApplicationDbContext dbContext, IExternalVulnerabilityService vulnerabilityService)
    {
        _dbContext = dbContext;
        _vulnerabilityService = vulnerabilityService;
    }

    public async Task Handle(DependencyVulnerableEvent notification, CancellationToken cancellationToken = default)
    {
        var vulnerability = await _dbContext.Vulnerabilities
            .FindAsync(notification.VulnerabilityId, cancellationToken);
        
        if(vulnerability is null) throw new NotFoundException($"Vulnerability with Id {notification.VulnerabilityId} not found");
        var vulnerabilityData = await _vulnerabilityService.QueryAsync(vulnerability.OsvId, cancellationToken);
        
        vulnerability.Update(vulnerabilityData.Summary, vulnerabilityData.Details, vulnerability.Published, vulnerability.Withdrawn);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}