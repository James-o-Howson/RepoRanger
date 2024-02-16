using FluentValidation;

namespace RepoRanger.Application.Projects.Queries.GetProjectsByRepositoryId;

internal sealed class GetProjectsByRepositoryIdQueryValidator : AbstractValidator<GetProjectsByRepositoryIdQuery>
{
    public GetProjectsByRepositoryIdQueryValidator()
    {
        RuleFor(q => q.RepositoryId)
            .NotEmpty();
    }
}