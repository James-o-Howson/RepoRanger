using FluentValidation;

namespace RepoRanger.Application.Queries.Projects.GetProjectsByRepositoryIds;

internal sealed class GetProjectsByRepositoryIdsQueryValidator : AbstractValidator<GetProjectsByRepositoryIdsQuery>
{
    public GetProjectsByRepositoryIdsQueryValidator()
    {
        RuleFor(q => q.RepositoryIds)
            .NotEmpty();
    }
}