using FluentValidation;

namespace RepoRanger.Queries.VersionControlSystems.GetVersionControlSystemByName;

internal sealed class GetVersionControlSystemByNameQueryValidator : AbstractValidator<GetVersionControlSystemByNameQuery>
{
    public GetVersionControlSystemByNameQueryValidator()
    {
        RuleFor(q => q.Name)
            .NotEmpty();
    }
}