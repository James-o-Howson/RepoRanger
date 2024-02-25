using FluentValidation;

namespace RepoRanger.Application.Sources.Queries.GetByName;

internal sealed class GetByNameQueryValidator : AbstractValidator<GetByNameQuery>
{
    public GetByNameQueryValidator()
    {
        RuleFor(q => q.Name)
            .NotEmpty();
    }
}