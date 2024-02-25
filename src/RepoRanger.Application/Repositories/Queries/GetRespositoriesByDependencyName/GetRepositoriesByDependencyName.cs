using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Branches;
using RepoRanger.Application.Common.Interfaces.Persistence;
using RepoRanger.Application.Projects.Common;
using RepoRanger.Application.Repositories.Common;

namespace RepoRanger.Application.Repositories.Queries.GetRespositoriesByDependencyName;

public sealed record GetRepositoriesByDependencyNameQuery : IRequest<RepositoriesDto>
{
    public string DependencyName { get; init; } = string.Empty;
}

internal sealed class GetRepositoriesByDependencyNameQueryHandler : IRequestHandler<GetRepositoriesByDependencyNameQuery, RepositoriesDto>
{
    private readonly IApplicationDbContext _context;

    public GetRepositoriesByDependencyNameQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RepositoriesDto> Handle(GetRepositoriesByDependencyNameQuery request, CancellationToken cancellationToken)
    {
        var dependencyInstances = await _context.Repositories
            .AsNoTracking()
            .AsSplitQuery()
            .Include(r => r.Projects)
                .ThenInclude(p => p.DependencyInstances)
            .Where(r => r.DependencyInstances.Any(di => di.DependencyName == request.DependencyName))
            .Select(di => new RepositoryDto(di.Name, di.RemoteUrl, di.DefaultBranch.ToDto(), di.Projects.ToDtos()))
            .ToListAsync(cancellationToken);

        return new RepositoriesDto(dependencyInstances);
    }
}