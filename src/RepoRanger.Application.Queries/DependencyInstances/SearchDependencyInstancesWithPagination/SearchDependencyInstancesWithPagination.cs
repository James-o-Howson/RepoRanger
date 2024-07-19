using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;
using RepoRanger.Application.Abstractions.Pagination;
using RepoRanger.Application.Contracts.DependencyInstances;
using RepoRanger.Domain.Entities;

namespace RepoRanger.Application.Queries.DependencyInstances.SearchDependencyInstancesWithPagination;

public sealed record SearchDependencyInstancesWithPaginationQuery : PaginatedRequest<DependencyInstanceVm>
{
    public IReadOnlyCollection<Guid>? SourceIds { get; init; } = Array.Empty<Guid>();
    public IReadOnlyCollection<Guid>? RepositoryIds { get; init; } = Array.Empty<Guid>();
    public IReadOnlyCollection<Guid>? ProjectIds { get; init; } = Array.Empty<Guid>();
}

internal sealed class SearchDependencyInstancesWithPaginationQueryHandler : IRequestHandler<SearchDependencyInstancesWithPaginationQuery, PaginatedList<DependencyInstanceVm>>
{
    private readonly IApplicationDbContext _context;

    public SearchDependencyInstancesWithPaginationQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<DependencyInstanceVm>> Handle(SearchDependencyInstancesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await GetDependencyInstances(request)
            .ApplyFilters(request.Filters)
            .Select(d => new DependencyInstanceVm
            {
                Id = d.Id,
                Source = d.Source,
                Name = d.DependencyName,
                Version = d.Version,
                ProjectName = d.Project.Name,
                RepositoryName = d.Project.Repository.Name
            })
            .ToPaginatedListAsync(request, cancellationToken);
    }

    private IQueryable<DependencyInstance> GetDependencyInstances(SearchDependencyInstancesWithPaginationQuery request)
    {
        IQueryable<DependencyInstance> query;
        if (request.ProjectIds != null && request.ProjectIds.Count != 0)
        {
            query = QueryProjectDependencies(request.ProjectIds);
        }
        else if (request.RepositoryIds != null && request.RepositoryIds.Count != 0)
        {
            query = QueryRepositoryDependencies(request.RepositoryIds);
        }
        else if (request.SourceIds != null && request.SourceIds.Count != 0)
        {
            query = QuerySourceDependencies(request.SourceIds);
        }
        else
        {
            query = _context.DependencyInstances
                .Include(di => di.Project)
                .ThenInclude(p => p.Repository);
        }

        return query;
    }
    
    private IQueryable<DependencyInstance> QueryProjectDependencies(IReadOnlyCollection<Guid> projectIds) =>
        _context.Projects
            .Include(p => p.DependencyInstances)
            .Include(p => p.Repository)
            .Where(p => projectIds.Contains(p.Id))
            .SelectMany(p => p.DependencyInstances);
    
    private IQueryable<DependencyInstance> QueryRepositoryDependencies(IReadOnlyCollection<Guid> repositoryIds) =>
        _context.Repositories
            .Include(b => b.Projects)
            .ThenInclude(p => p.DependencyInstances)
            .Where(r => repositoryIds.Contains(r.Id))
            .SelectMany(r => r.DependencyInstances);

    private IQueryable<DependencyInstance> QuerySourceDependencies(IReadOnlyCollection<Guid> sourceIds) =>
        _context.Sources
            .Include(r => r.Repositories)
            .ThenInclude(b => b.Projects)
            .ThenInclude(p => p.DependencyInstances)
            .Where(r => sourceIds.Contains(r.Id))
            .SelectMany(r => r.DependencyInstances);
}