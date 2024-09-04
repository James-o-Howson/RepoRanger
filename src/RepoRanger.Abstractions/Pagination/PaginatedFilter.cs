using RepoRanger.Abstractions.Pagination.Enums;

namespace RepoRanger.Abstractions.Pagination;

public record PaginatedFilter
{
    public MatchMode MatchMode { get; init; } = MatchMode.StartsWith;
    public FilterOperator Operator { get; init; } = FilterOperator.And;
    public string Value { get; init; } = string.Empty;
}