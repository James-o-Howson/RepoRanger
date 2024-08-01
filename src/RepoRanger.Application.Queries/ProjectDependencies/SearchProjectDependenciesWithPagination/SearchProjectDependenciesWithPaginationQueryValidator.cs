using FluentValidation;
using RepoRanger.Application.Abstractions.Pagination;
using RepoRanger.Application.Contracts.ProjectDependencies;

namespace RepoRanger.Application.Queries.ProjectDependencies.SearchProjectDependenciesWithPagination;

internal sealed class SearchProjectDependenciesWithPaginationQueryValidator : AbstractValidator<SearchProjectDependenciesWithPaginationQuery>
{
    public SearchProjectDependenciesWithPaginationQueryValidator()
    {
        Include(new PaginatedRequestValidator<ProjectDependencyVm>());
    }
}