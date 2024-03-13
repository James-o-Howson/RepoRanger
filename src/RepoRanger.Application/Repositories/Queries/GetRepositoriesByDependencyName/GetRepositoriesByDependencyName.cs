using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Common.Interfaces.Persistence;
using RepoRanger.Application.Projects.Common;
using RepoRanger.Application.Repositories.Common;

namespace RepoRanger.Application.Repositories.Queries.GetRepositoriesByDependencyName;

public sealed record GetRepositoriesByDependencyNameQuery : IRequest<RepositoryAggregatesVm>
{
    public string DependencyName { get; init; } = string.Empty;
}

internal sealed class GetRepositoriesByDependencyNameQueryHandler : IRequestHandler<GetRepositoriesByDependencyNameQuery, RepositoryAggregatesVm>
{
    private readonly IApplicationDbContext _context;

    public GetRepositoriesByDependencyNameQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RepositoryAggregatesVm> Handle(GetRepositoriesByDependencyNameQuery request, CancellationToken cancellationToken)
    {
        var dependencyInstances = await _context.Repositories
            .AsNoTracking()
            .AsSplitQuery()
            .Include(r => r.Projects)
                .ThenInclude(p => p.DependencyInstances)
            .Where(r => r.DependencyInstances.Any(di => di.DependencyName == request.DependencyName))
            .Select(r => new RepositoryAggregateVm(r.Id, r.Name, r.RemoteUrl, r.DefaultBranch, r.Projects.ToDtos()))
            .ToListAsync(cancellationToken);

        return new RepositoryAggregatesVm(dependencyInstances);
    }
}