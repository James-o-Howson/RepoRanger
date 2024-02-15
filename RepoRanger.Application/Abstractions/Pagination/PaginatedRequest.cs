using MediatR;

namespace RepoRanger.Application.Abstractions.Pagination;

public record PaginatedRequest<TData> : IRequest<PaginatedList<TData>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}