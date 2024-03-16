using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Common.Interfaces.Persistence;
using RepoRanger.Application.Common.Pagination;
using RepoRanger.Domain.Entities;

namespace RepoRanger.Application.DependencyInstances.Queries.SearchDependencyInstancesWithPagination;

public sealed record SearchDependencyInstancesWithPaginationQuery : PaginatedRequest<DependencyInstanceVm>
{
    public IReadOnlyCollection<int>? SourceIds { get; init; } = Array.Empty<int>();
    public IReadOnlyCollection<int>? RepositoryIds { get; init; } = Array.Empty<int>();
    public IReadOnlyCollection<int>? ProjectIds { get; init; } = Array.Empty<int>();
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
            query = _context.DependencyInstances;
        }

        return query;
    }
    
    private IQueryable<DependencyInstance> QueryProjectDependencies(IReadOnlyCollection<int> projectIds) =>
        _context.Projects
            .Include(p => p.DependencyInstances)
            .Where(p => projectIds.Contains(p.Id))
            .SelectMany(p => p.DependencyInstances);
    
    private IQueryable<DependencyInstance> QueryRepositoryDependencies(IReadOnlyCollection<int> repositoryIds) =>
        _context.Repositories
            .Include(b => b.Projects)
            .ThenInclude(p => p.DependencyInstances)
            .Where(r => repositoryIds.Contains(r.Id))
            .SelectMany(r => r.DependencyInstances);

    private IQueryable<DependencyInstance> QuerySourceDependencies(IReadOnlyCollection<int> sourceIds) =>
        _context.Sources
            .Include(r => r.Repositories)
            .ThenInclude(b => b.Projects)
            .ThenInclude(p => p.DependencyInstances)
            .Where(r => sourceIds.Contains(r.Id))
            .SelectMany(r => r.DependencyInstances);
}