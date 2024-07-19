using MediatR;
using RepoRanger.Application.Abstractions.Pagination.Enums;

namespace RepoRanger.Application.Abstractions.Pagination;

public abstract record PaginatedRequest<TData> : IRequest<PaginatedList<TData>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 25;
    public string SortField { get; init; } = string.Empty;
    public SortOrder SortOrder { get; init; } = SortOrder.Ascending;

    public IReadOnlyDictionary<string, List<PaginatedFilter>>? Filters { get; init; } = null;
}