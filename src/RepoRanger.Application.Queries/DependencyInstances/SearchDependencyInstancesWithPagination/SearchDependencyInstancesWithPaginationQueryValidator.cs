using FluentValidation;
using RepoRanger.Application.Abstractions.Pagination;
using RepoRanger.Application.Contracts.DependencyInstances;

namespace RepoRanger.Application.Queries.DependencyInstances.SearchDependencyInstancesWithPagination;

internal sealed class SearchDependencyInstancesWithPaginationQueryValidator : AbstractValidator<SearchDependencyInstancesWithPaginationQuery>
{
    public SearchDependencyInstancesWithPaginationQueryValidator()
    {
        Include(new PaginatedRequestValidator<DependencyInstanceVm>());
    }
}