using FluentValidation;

namespace RepoRanger.Application.Sources.Queries.GetByName;

internal sealed class GetByNameQueryValidator : AbstractValidator<GetSourceByNameQuery>
{
    public GetByNameQueryValidator()
    {
        RuleFor(q => q.Name)
            .NotEmpty();
    }
}