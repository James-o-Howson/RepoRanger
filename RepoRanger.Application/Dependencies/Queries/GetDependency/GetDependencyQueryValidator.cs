using FluentValidation;

namespace RepoRanger.Application.Dependencies.Queries.GetDependency;

internal sealed class GetDependencyQueryValidator : AbstractValidator<GetDependencyQuery>
{
    public GetDependencyQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();
    }
}