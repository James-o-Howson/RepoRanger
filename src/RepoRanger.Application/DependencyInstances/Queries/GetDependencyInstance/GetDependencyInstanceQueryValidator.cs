using FluentValidation;

namespace RepoRanger.Application.DependencyInstances.Queries.GetDependencyInstance;

internal sealed class GetDependencyInstanceQueryValidator : AbstractValidator<GetDependencyQuery>
{
    public GetDependencyInstanceQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();
    }
}