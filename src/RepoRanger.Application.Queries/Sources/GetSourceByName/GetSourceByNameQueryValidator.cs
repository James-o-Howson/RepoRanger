using FluentValidation;

namespace RepoRanger.Application.Queries.Sources.GetSourceByName;

internal sealed class GetSourceByNameQueryValidator : AbstractValidator<GetSourceByNameQuery>
{
    public GetSourceByNameQueryValidator()
    {
        RuleFor(q => q.Name)
            .NotEmpty();
    }
}