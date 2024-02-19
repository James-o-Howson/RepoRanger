using System.Linq.Expressions;
using RepoRanger.Application.Common.Pagination;
using RepoRanger.Application.Common.Pagination.Enums;
using RepoRanger.Domain.Entities;

namespace RepoRanger.Application.Dependencies.Queries.SearchDependenciesWithPagination;

internal static class DependencyPaginationExtensions
{
    private static readonly Dictionary<string, Func<IQueryable<Dependency>, PaginatedFilter, Expression<Func<Dependency, bool>>>> FilterHandlers =
        new()
        {
            { "Name", ApplyFilterByName },
            { "Version", ApplyFilterByVersion }
        };

    public static IQueryable<Dependency> ApplyFilters(this IQueryable<Dependency> query, IReadOnlyDictionary<string, List<PaginatedFilter>>? paginatedFilters)
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

    private static Expression<Func<Dependency, bool>> ApplyFilterByName(IQueryable<Dependency> dependencies, PaginatedFilter filter)
    {
        return filter.MatchMode switch
        {
            MatchMode.StartsWith => d => d.Name.StartsWith(filter.Value),
            MatchMode.Contains => d => d.Name.Contains(filter.Value),
            MatchMode.NotContains => d => !d.Name.Contains(filter.Value),
            MatchMode.EndsWith => d => d.Name.EndsWith(filter.Value),
            MatchMode.Equals => d => d.Name == filter.Value,
            MatchMode.NotEquals => d => d.Name != filter.Value,
            _ => throw new ArgumentException($"Invalid {nameof(MatchMode)}")
        };
    }

    private static Expression<Func<Dependency, bool>> ApplyFilterByVersion(IQueryable<Dependency> dependencies, PaginatedFilter filter)
    {
        return filter.MatchMode switch
        {
            MatchMode.StartsWith => d => d.Version.StartsWith(filter.Value),
            MatchMode.Contains => d => d.Version.Contains(filter.Value),
            MatchMode.NotContains => d => !d.Version.Contains(filter.Value),
            MatchMode.EndsWith => d => d.Version.EndsWith(filter.Value),
            MatchMode.Equals => d => d.Version == filter.Value,
            MatchMode.NotEquals => d => d.Version != filter.Value,
            _ => throw new ArgumentException($"Invalid {nameof(MatchMode)}")
        };
    }

    private static IQueryable<Dependency> AddOrQueryFilter(IQueryable<Dependency> dependencies, string propertyName, PaginatedFilter filter)
    {
        var predicate = FilterHandlers[propertyName](dependencies, filter);
        
        return dependencies.Where(predicate);
    }
}