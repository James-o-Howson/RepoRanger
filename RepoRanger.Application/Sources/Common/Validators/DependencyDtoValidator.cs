using FluentValidation;
using RepoRanger.Application.Sources.Common.Models;

namespace RepoRanger.Application.Sources.Common.Validators;

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