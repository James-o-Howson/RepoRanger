using FluentValidation;
using RepoRanger.Abstractions.Pagination;
using RepoRanger.Contracts.ProjectDependencies;

namespace RepoRanger.Queries.ProjectDependencies.SearchProjectDependenciesWithPagination;

internal sealed class SearchProjectDependenciesWithPaginationQueryValidator : AbstractValidator<SearchProjectDependenciesWithPaginationQuery>
{
    public SearchProjectDependenciesWithPaginationQueryValidator()
    {
        Include(new PaginatedRequestValidator<ProjectDependencyVm>());
    }
}