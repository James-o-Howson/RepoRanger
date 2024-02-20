using FluentValidation;

namespace RepoRanger.Application.Projects.Queries.GetProjectsByRepositoryIds;

internal sealed class GetProjectsByRepositoryIdsQueryValidator : AbstractValidator<GetProjectsByRepositoryIdsQuery>
{
    public GetProjectsByRepositoryIdsQueryValidator()
    {
        RuleFor(q => q.RepositoryIds)
            .NotEmpty();
    }
}