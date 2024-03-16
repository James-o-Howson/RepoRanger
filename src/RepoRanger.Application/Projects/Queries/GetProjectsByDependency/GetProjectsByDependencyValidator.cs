using FluentValidation;

namespace RepoRanger.Application.Projects.Queries.GetProjectsByDependency;

internal sealed class GetProjectsByDependencyValidator : AbstractValidator<GetProjectsByDependencyQuery>
{
    public GetProjectsByDependencyValidator()
    {
        RuleFor(q => q.DependencyName)
            .NotEmpty();
    }
}