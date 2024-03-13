using FluentValidation;

namespace RepoRanger.Application.DependencyInstances.Common;

internal sealed class DependencyInstanceDtoValidator : AbstractValidator<DependencyInstanceVm>
{
    public DependencyInstanceDtoValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty();
        
        RuleFor(p => p.Version)
            .NotEmpty();
    }
}