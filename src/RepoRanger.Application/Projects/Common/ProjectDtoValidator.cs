using FluentValidation;
using RepoRanger.Application.DependencyInstances.Common;

namespace RepoRanger.Application.Projects.Common;

internal sealed class ProjectDtoValidator : AbstractValidator<ProjectVm>
{
    public ProjectDtoValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty();
        
        RuleFor(p => p.Version)
            .NotEmpty();
        
        RuleFor(p => p.DependencyInstances)
            .Must(r => r.Any());

        RuleForEach(p => p.DependencyInstances)
            .SetValidator(new DependencyInstanceDtoValidator());
    }
}