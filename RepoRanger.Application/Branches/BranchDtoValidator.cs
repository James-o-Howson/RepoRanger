using FluentValidation;
using RepoRanger.Application.Projects.Common;

namespace RepoRanger.Application.Branches;

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