using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;
using RepoRanger.Application.Abstractions.Pagination;
using RepoRanger.Application.Contracts.ProjectDependencies;
using RepoRanger.Domain.VersionControlSystems.Entities;

namespace RepoRanger.Application.Queries.ProjectDependencies.SearchProjectDependenciesWithPagination;

public sealed record SearchProjectDependenciesWithPaginationQuery : PaginatedRequest<ProjectDependencyVm>
{
    public IReadOnlyCollection<Guid>? VersionControlSystemIds { get; init; } = Array.Empty<Guid>();
    public IReadOnlyCollection<Guid>? RepositoryIds { get; init; } = Array.Empty<Guid>();
    public IReadOnlyCollection<Guid>? ProjectIds { get; init; } = Array.Empty<Guid>();
}

internal sealed class SearchProjectDependenciesWithPaginationQueryHandler : IRequestHandler<SearchProjectDependenciesWithPaginationQuery, PaginatedList<ProjectDependencyVm>>
{
    private readonly IApplicationDbContext _context;

    public SearchProjectDependenciesWithPaginationQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<ProjectDependencyVm>> Handle(SearchProjectDependenciesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await GetProjectDependencies(request)
            .ApplyFilters(request.Filters)
            .Select(d => new ProjectDependencyVm
            {
                Id = d.Id,
                // Source = d.Version,
                Name = d.Dependency.Name,
                Version = d.Version.Value,
                ProjectName = d.Project.Name,
                RepositoryName = d.Project.Repository.Name
            })
            .ToPaginatedListAsync(request, cancellationToken);
    }

    private IQueryable<ProjectDependency> GetProjectDependencies(SearchProjectDependenciesWithPaginationQuery request)
    {
        IQueryable<ProjectDependency> query;
        if (request.ProjectIds != null && request.ProjectIds.Count != 0)
        {
            query = QueryProjectDependencies(request.ProjectIds);
        }
        else if (request.RepositoryIds != null && request.RepositoryIds.Count != 0)
        {
            query = QueryRepositoryDependencies(request.RepositoryIds);
        }
        else if (request.VersionControlSystemIds != null && request.VersionControlSystemIds.Count != 0)
        {
            query = QueryVersionControlSystemDependencies(request.VersionControlSystemIds);
        }
        else
        {
            query = _context.ProjectDependencies
                .Include(di => di.Project)
                .ThenInclude(p => p.Repository);
        }

        return query;
    }
    
    private IQueryable<ProjectDependency> QueryProjectDependencies(IReadOnlyCollection<Guid> ids) =>
        _context.Projects
            .Include(p => p.ProjectDependencies)
            .Include(p => p.Repository)
            .Where(p => ids.Contains(p.Id))
            .SelectMany(p => p.ProjectDependencies);
    
    private IQueryable<ProjectDependency> QueryRepositoryDependencies(IReadOnlyCollection<Guid> ids) =>
        _context.Repositories
            .Include(b => b.Projects)
            .ThenInclude(p => p.ProjectDependencies)
            .Where(r => ids.Contains(r.Id))
            .SelectMany(r => r.Dependencies);

    private IQueryable<ProjectDependency> QueryVersionControlSystemDependencies(IReadOnlyCollection<Guid> ids) =>
        _context.VersionControlSystems
            .Include(r => r.Repositories)
            .ThenInclude(b => b.Projects)
            .ThenInclude(p => p.ProjectDependencies)
            .Where(r => ids.Contains(r.Id))
            .SelectMany(r => r.Dependencies);
}