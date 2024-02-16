using FluentValidation;
using RepoRanger.Application.Sources.Commands.Common.Models;

namespace RepoRanger.Application.Sources.Commands.Common.Validators;

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