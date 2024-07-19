using FluentValidation;

namespace RepoRanger.Application.Queries.Projects.GetProjectsByDependency;

internal sealed class GetProjectsByDependencyValidator : AbstractValidator<GetProjectsByDependencyQuery>
{
    public GetProjectsByDependencyValidator()
    {
        RuleFor(q => q.DependencyName)
            .NotEmpty();
    }
}