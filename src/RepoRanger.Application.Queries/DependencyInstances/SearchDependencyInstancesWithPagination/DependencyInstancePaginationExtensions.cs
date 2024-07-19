using System.Linq.Expressions;
using RepoRanger.Application.Abstractions.Pagination;
using RepoRanger.Application.Abstractions.Pagination.Enums;
using RepoRanger.Domain.Entities;

namespace RepoRanger.Application.Queries.DependencyInstances.SearchDependencyInstancesWithPagination;

internal static class DependencyInstancePaginationExtensions
{
    private static readonly Dictionary<string, Func<IQueryable<DependencyInstance>, PaginatedFilter, Expression<Func<DependencyInstance, bool>>>> FilterHandlers =
        new()
        {
            { "Name", ApplyFilterByName },
            { "Version", ApplyFilterByVersion }
        };

    public static IQueryable<DependencyInstance> ApplyFilters(this IQueryable<DependencyInstance> query, IReadOnlyDictionary<string, List<PaginatedFilter>>? paginatedFilters)
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

    private static Expression<Func<DependencyInstance, bool>> ApplyFilterByName(IQueryable<DependencyInstance> dependencyInstances, PaginatedFilter filter)
    {
        return filter.MatchMode switch
        {
            MatchMode.StartsWith => d => d.DependencyName.StartsWith(filter.Value),
            MatchMode.Contains => d => d.DependencyName.Contains(filter.Value),
            MatchMode.NotContains => d => !d.DependencyName.Contains(filter.Value),
            MatchMode.EndsWith => d => d.DependencyName.EndsWith(filter.Value),
            MatchMode.Equals => d => d.DependencyName == filter.Value,
            MatchMode.NotEquals => d => d.DependencyName != filter.Value,
            _ => throw new ArgumentException($"Invalid {nameof(MatchMode)}")
        };
    }

    private static Expression<Func<DependencyInstance, bool>> ApplyFilterByVersion(IQueryable<DependencyInstance> dependencyInstances, PaginatedFilter filter)
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

    private static IQueryable<DependencyInstance> AddOrQueryFilter(IQueryable<DependencyInstance> dependencyInstances, string propertyName, PaginatedFilter filter)
    {
        var predicate = FilterHandlers[propertyName](dependencyInstances, filter);
        
        return dependencyInstances.Where(predicate);
    }
}