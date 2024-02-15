using FluentValidation;
using RepoRanger.Application.Abstractions.Pagination;

namespace RepoRanger.Application.Dependencies.Queries.SearchDependenciesWithPagination;

internal sealed class SearchDependenciesWithPaginationQueryValidator : AbstractValidator<SearchDependenciesWithPaginationQuery>
{
    public SearchDependenciesWithPaginationQueryValidator()
    {
        Include(new PaginatedRequestValidator<DependencyVm>());
    }
}