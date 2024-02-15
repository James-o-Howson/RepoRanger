using MediatR;
using Microsoft.EntityFrameworkCore;
using RepoRanger.Application.Abstractions.Interfaces.Persistence;
using RepoRanger.Application.Abstractions.Pagination;
using RepoRanger.Domain.Source;

namespace RepoRanger.Application.Dependencies.Queries.SearchDependenciesWithPagination;

public sealed record SearchDependenciesWithPaginationQuery : PaginatedRequest<DependencyVm>
{
    public IReadOnlyCollection<Guid>? SourceIds { get; init; }
    public IReadOnlyCollection<Guid>? RepositoryIds { get; init; }
    public IReadOnlyCollection<Guid>? ProjectIds { get; init; }
}

internal sealed class SearchDependenciesWithPaginationQueryHandler : IRequestHandler<SearchDependenciesWithPaginationQuery, PaginatedList<DependencyVm>>
{
    private readonly IApplicationDbContext _context;

    public SearchDependenciesWithPaginationQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<DependencyVm>> Handle(SearchDependenciesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Dependency> query;
        
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
            query = _context.Dependencies.AsNoTracking();
        }

        return await query.Select(d => new DependencyVm
            {
                Id = d.Id,
                Name = d.Name,
                Version = d.Version
            })
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
    
    private IQueryable<Dependency> QueryProjectDependencies(IReadOnlyCollection<Guid> projectIds) =>
        _context.Projects
            .Include(p => p.Dependencies)
            .Where(p => projectIds.Contains(p.Id))
            .SelectMany(p => p.Dependencies);
    
    private IQueryable<Dependency> QueryRepositoryDependencies(IReadOnlyCollection<Guid> repositoryIds) =>
        _context.Repositories
            .Include(r => r.Branches)
            .ThenInclude(b => b.Projects)
            .ThenInclude(p => p.Dependencies)
            .Where(r => repositoryIds.Contains(r.Id))
            .SelectMany(r => r.Dependencies);

    private IQueryable<Dependency> QuerySourceDependencies(IReadOnlyCollection<Guid> sourceIds) =>
        _context.Sources
            .Include(r => r.Repositories)
            .ThenInclude(r => r.Branches)
            .ThenInclude(b => b.Projects)
            .ThenInclude(p => p.Dependencies)
            .Where(r => sourceIds.Contains(r.Id))
            .SelectMany(r => r.Dependencies);
}