using FluentValidation;

namespace RepoRanger.Queries.Repositories.GetRepositoriesByVersionControlSystemId;

internal sealed class GetRepositoriesByVersionControlSystemIdQueryValidator : AbstractValidator<GetRepositoriesByVersionControlSystemIdQuery>
{
    public GetRepositoriesByVersionControlSystemIdQueryValidator()
    {
        RuleFor(q => q.VersionControlSystemId)
            .NotEmpty();
    }
}