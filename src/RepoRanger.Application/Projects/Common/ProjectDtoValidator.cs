using FluentValidation;
using RepoRanger.Application.DependencyInstances.Common;

namespace RepoRanger.Application.Projects.Common;

internal sealed class ProjectDtoValidator : AbstractValidator<ProjectDto>
{
    public ProjectDtoValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty();
        
        RuleFor(p => p.Version)
            .NotEmpty();
        
        RuleFor(p => p.Dependencies)
            .Must(r => r.Any());

        RuleForEach(p => p.Dependencies)
            .SetValidator(new DependencyInstanceDtoValidator());
    }
}