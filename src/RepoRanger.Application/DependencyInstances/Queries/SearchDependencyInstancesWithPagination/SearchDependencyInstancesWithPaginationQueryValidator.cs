using FluentValidation;
using RepoRanger.Application.Common.Pagination;

namespace RepoRanger.Application.DependencyInstances.Queries.SearchDependencyInstancesWithPagination;

internal sealed class SearchDependencyInstancesWithPaginationQueryValidator : AbstractValidator<SearchDependencyInstancesWithPaginationQuery>
{
    public SearchDependencyInstancesWithPaginationQueryValidator()
    {
        Include(new PaginatedRequestValidator<DependencyInstanceVm>());
    }
}