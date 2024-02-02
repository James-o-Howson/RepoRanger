using FluentValidation;
using RepoRanger.Application.Sources.Commands.Common.Models;

namespace RepoRanger.Application.Sources.Commands.Common.Validators;

internal sealed class BranchDtoValidator : AbstractValidator<BranchDto>
{
    public BranchDtoValidator()
    {
        RuleFor(b => b.Name)
            .NotEmpty();
        
        RuleFor(b => b.Projects)
            .Must(r => r.Any());

        RuleForEach(b => b.Projects)
            .SetValidator(new ProjectDtoValidator());
    }
}