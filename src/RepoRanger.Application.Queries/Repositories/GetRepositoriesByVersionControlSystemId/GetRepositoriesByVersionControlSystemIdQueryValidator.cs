using FluentValidation;

namespace RepoRanger.Application.Queries.Repositories.GetRepositoriesByVersionControlSystemId;

internal sealed class GetRepositoriesByVersionControlSystemIdQueryValidator : AbstractValidator<GetRepositoriesByVersionControlSystemIdQuery>
{
    public GetRepositoriesByVersionControlSystemIdQueryValidator()
    {
        RuleFor(q => q.VersionControlSystemId)
            .NotEmpty();
    }
}