using FluentValidation;

namespace RepoRanger.Application.Dependencies.Common;

internal sealed class DependencyDtoValidator : AbstractValidator<DependencyDto>
{
    public DependencyDtoValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty();
        
        RuleFor(p => p.Version)
            .NotEmpty();
    }
}