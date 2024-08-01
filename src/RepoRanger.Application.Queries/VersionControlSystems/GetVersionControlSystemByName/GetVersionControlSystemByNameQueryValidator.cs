using FluentValidation;

namespace RepoRanger.Application.Queries.VersionControlSystems.GetVersionControlSystemByName;

internal sealed class GetVersionControlSystemByNameQueryValidator : AbstractValidator<GetVersionControlSystemByNameQuery>
{
    public GetVersionControlSystemByNameQueryValidator()
    {
        RuleFor(q => q.Name)
            .NotEmpty();
    }
}