using FluentValidation;
using RepoRanger.Application.Projects.Common;

namespace RepoRanger.Application.Branches;

internal sealed class BranchDtoValidator : AbstractValidator<BranchDto>
{
    public BranchDtoValidator()
    {
        RuleFor(b => b.Name)
            .NotEmpty();
    }
}