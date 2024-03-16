using FluentValidation;

namespace RepoRanger.Application.Sources.Queries.GetSourceByName;

internal sealed class GetSourceByNameQueryValidator : AbstractValidator<GetSourceByNameQuery>
{
    public GetSourceByNameQueryValidator()
    {
        RuleFor(q => q.Name)
            .NotEmpty();
    }
}