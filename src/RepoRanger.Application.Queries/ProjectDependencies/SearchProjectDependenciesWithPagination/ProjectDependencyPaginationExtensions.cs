using System.Linq.Expressions;
using RepoRanger.Application.Abstractions.Pagination;
using RepoRanger.Application.Abstractions.Pagination.Enums;
using RepoRanger.Domain.VersionControlSystems.Entities;

namespace RepoRanger.Application.Queries.ProjectDependencies.SearchProjectDependenciesWithPagination;

internal static class ProjectDependencyPaginationExtensions
{
    private static readonly Dictionary<string, Func<IQueryable<ProjectDependency>, PaginatedFilter, Expression<Func<ProjectDependency, bool>>>> FilterHandlers =
        new()
        {
            { "Name", ApplyFilterByName },
            { "Version", ApplyFilterByVersion }
        };

    public static IQueryable<ProjectDependency> ApplyFilters(this IQueryable<ProjectDependency> query, IReadOnlyDictionary<string, List<PaginatedFilter>>? paginatedFilters)
    {
        if (paginatedFilters is null || paginatedFilters.Count == 0)
            return query;

        foreach (var (propertyName, filters) in paginatedFilters)
        {
            for (var i = 0; i < filters.Count; i++)
            {
                var filter = filters[i];
                var useAndCondition = i == 0 || filter.Operator == FilterOperator.And;
                
                if (useAndCondition)
                {
                    query = FilterHandlers.TryGetValue(propertyName, out var handler)
                        ? query.Where(handler(query, filter))
                        : query;
                }
                else
                {
                    query = AddOrQueryFilter(query, propertyName, filter);
                }
            }
        }

        return query;
    }

    private static Expression<Func<ProjectDependency, bool>> ApplyFilterByName(IQueryable<ProjectDependency> projectDependencies, PaginatedFilter filter)
    {
        return filter.MatchMode switch
        {
            MatchMode.StartsWith => d => d.Dependency.Name.StartsWith(filter.Value),
            MatchMode.Contains => d => d.Dependency.Name.Contains(filter.Value),
            MatchMode.NotContains => d => !d.Dependency.Name.Contains(filter.Value),
            MatchMode.EndsWith => d => d.Dependency.Name.EndsWith(filter.Value),
            MatchMode.Equals => d => d.Dependency.Name == filter.Value,
            MatchMode.NotEquals => d => d.Dependency.Name != filter.Value,
            _ => throw new ArgumentException($"Invalid {nameof(MatchMode)}")
        };
    }

    private static Expression<Func<ProjectDependency, bool>> ApplyFilterByVersion(IQueryable<ProjectDependency> projectDependencies, PaginatedFilter filter)
    {
        return filter.MatchMode switch
        {
            MatchMode.StartsWith => d => d.Version.Value.StartsWith(filter.Value),
            MatchMode.Contains => d => d.Version.Value.Contains(filter.Value),
            MatchMode.NotContains => d => !d.Version.Value.Contains(filter.Value),
            MatchMode.EndsWith => d => d.Version.Value.EndsWith(filter.Value),
            MatchMode.Equals => d => d.Version.Value == filter.Value,
            MatchMode.NotEquals => d => d.Version.Value != filter.Value,
            _ => throw new ArgumentException($"Invalid {nameof(MatchMode)}")
        };
    }

    private static IQueryable<ProjectDependency> AddOrQueryFilter(IQueryable<ProjectDependency> projectDependencies, string propertyName, PaginatedFilter filter)
    {
        var predicate = FilterHandlers[propertyName](projectDependencies, filter);
        
        return projectDependencies.Where(predicate);
    }
}